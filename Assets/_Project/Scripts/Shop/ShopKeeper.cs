using UnityEngine;
/// <summary>
/// Opens the shop window. Reuses the interaction system built in week 4.
/// </summary>
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopInventory shopInventory;
    [SerializeField] private ShopUI shopUI;
    public string InteractionPrompt =>
    shopInventory == null ? string.Empty : $"Talk to {shopInventory.shopName}";
    public void Interact(GameObject interactor)
    {
        shopUI.Open(shopInventory);
    }
}