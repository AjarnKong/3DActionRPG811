using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Builds the buy and sell lists and forwards clicks to the transaction service.
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelRoot;

    [SerializeField] private Transform entryContainer;
    [SerializeField] private ShopEntryUI entryPrefab;
    [SerializeField] private TransactionService transactions;
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private CurrencyManager currency;
    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    private readonly List<ShopEntryUI> _rows = new List<ShopEntryUI>();
    private ShopInventory _shop;
    private bool _isSellTab;
    private void Start()
    {
        panelRoot.SetActive(false);
    }
    public void Open(ShopInventory shop)
    {
        _shop = shop;
        _isSellTab = false;
        titleText.text = shop.shopName;
        messageText.text = string.Empty;
        panelRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Refresh();
    }
    public void Close()
    {
        panelRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Hooked to the two tab buttons in the Inspector
    public void ShowBuyTab()
    {
        _isSellTab = false;
        Refresh();
    }
    public void ShowSellTab()
    {
        _isSellTab = true;
        Refresh();
    }

    private void Refresh()
    {
        ClearRows();
        if (_isSellTab)
        {
            BuildSellList();
        }
        else
        {
            BuildBuyList();
        }
    }
    private void BuildBuyList()
    {
        foreach (ShopInventory.Entry entry in _shop.entries)
        {
            if (entry.item == null) continue;
            // Copied into a local variable so the click uses the right entry
            ShopInventory.Entry captured = entry;
            int price = _shop.GetPrice(captured);
            ShopEntryUI row = Instantiate(entryPrefab, entryContainer);
            row.Bind(captured.item, price, "Buy", currency.CanAfford(price),
            () => HandleBuy(captured.item, price));
            _rows.Add(row);
        }
    }
    private void BuildSellList()
    {
        foreach (InventorySlot slot in inventory.Slots)
        {
            if (slot.IsEmpty) continue;
            ItemData captured = slot.item;
            ShopEntryUI row = Instantiate(entryPrefab, entryContainer);
            row.Bind(captured, captured.sellPrice, "Sell", true,
            () => HandleSell(captured));
            _rows.Add(row);
        }
    }
    private void HandleBuy(ItemData item, int price)
    {
        transactions.TryBuy(item, price);
        messageText.text = transactions.LastMessage;
        Refresh();
    }

    private void HandleSell(ItemData item)
    {
        transactions.TrySell(item);
        messageText.text = transactions.LastMessage;
        Refresh();
    }
    private void ClearRows()
    {
        foreach (ShopEntryUI row in _rows)
        {
            Destroy(row.gameObject);
        }
        _rows.Clear();
    }
}