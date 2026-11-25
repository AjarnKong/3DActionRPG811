using System;
using UnityEngine;
/// <summary>
/// The list of goods a single shop keeper offers.
/// </summary>
[CreateAssetMenu(fileName = "NewShop", menuName = "RPG/Shop Inventory")]
public class ShopInventory : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public ItemData item;
        // Leave at zero to use the buy price stored on the item itself
        [Min(0)] public int priceOverride;
    }
    public string shopName = "General Store";
    public Entry[] entries;
    public int GetPrice(Entry entry)
    {
        if (entry == null || entry.item == null)
        {
            return 0;
        }
        return entry.priceOverride > 0 ? entry.priceOverride : entry.item.buyPrice;
    }
}