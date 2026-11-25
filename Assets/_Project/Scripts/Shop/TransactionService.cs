using UnityEngine;
/// <summary>
/// Central buy and sell logic, kept separate from any user interface.
/// Every condition is validated before a single value is changed.
/// </summary>
public class TransactionService : MonoBehaviour
{
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private CurrencyManager currency;
    // Explains the result of the last attempt so the UI can show it
    public string LastMessage { get; private set; }
    public bool TryBuy(ItemData item, int price)
    {
        if (item == null)
        {
            return false;
        }
        // Validate everything first, so the player can never pay for nothing
        if (!currency.CanAfford(price))
        {
            LastMessage = "Not enough gold";
            return false;
        }
        if (!inventory.HasSpaceFor(item))
        {
            LastMessage = "Inventory is full";
            return false;
        }
        // Both checks passed, now it is safe to change state

        currency.TrySpend(price);
        inventory.AddItem(item);
        LastMessage = $"Bought {item.displayName}";
        return true;
    }
    public bool TrySell(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            return false;
        }
        if (inventory.CountOf(item) < amount)
        {
            LastMessage = "You do not have that item";
            return false;
        }
        inventory.RemoveItem(item, amount);
        currency.Add(item.sellPrice * amount);
        LastMessage = $"Sold {item.displayName}";
        return true;
    }
}