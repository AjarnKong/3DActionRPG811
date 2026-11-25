using UnityEngine;
/// <summary>
/// Spawns loot when the attached Health component reports a death.
/// </summary>
[RequireComponent(typeof(Health))]
public class LootDropper : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;
    // Used only when an item has no prefab of its own
    [SerializeField] private ItemPickup fallbackPickupPrefab;
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField] private float scatterRadius = 0.6f;
    [SerializeField] private CurrencyManager playerCurrency;

    private Health _health;
    private void Awake()
    {
        _health = GetComponent<Health>();

        // A prefab asset cannot store a reference to a scene object,
        // so the player is located at runtime instead
        if (playerCurrency == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerCurrency = player.GetComponent<CurrencyManager>();
            }
        }
    }
    private void OnEnable()
    {
        _health.OnDeath += DropLoot;
    }
    private void OnDisable()
    {
        _health.OnDeath -= DropLoot;
    }
    private void DropLoot()
    {
        if (playerCurrency != null)
        {
            int gold = Random.Range(lootTable.minGold, lootTable.maxGold + 1);
            playerCurrency.Add(gold);
        }

        if (lootTable == null)
        {
            return;
        }
        LootTable.Entry entry = lootTable.Roll();
        if (entry == null || entry.item == null)
        {
            return;
        }
        int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);

        // Scatter slightly so several drops do not overlap exactly
        Vector2 offset = Random.insideUnitCircle * scatterRadius;
        Vector3 position = transform.position
        + new Vector3(offset.x, spawnHeight, offset.y);
        // Each item carries its own prefab, so every drop looks correct
        LootTable.Entry entry1 = entry;
        ItemPickup prefab = entry.item.worldPrefab != null
            ? entry.item.worldPrefab.GetComponent<ItemPickup>()
            : fallbackPickupPrefab;
        if (prefab == null)
        {
            Debug.LogWarning($"No prefab for {entry.item.displayName}");
            return;
        }
        ItemPickup pickup = Instantiate(prefab, position,
        Quaternion.identity);
        pickup.Setup(entry.item, amount);
    }
}