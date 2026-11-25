using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// One row in the shop list. Works for both the buy and the sell tab.
/// </summary>
public class ShopEntryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionLabel;
    public void Bind(ItemData item, int price, string label,
    bool interactable, Action onClicked)
    {
        iconImage.sprite = item.icon;
        // Applies the same tint used in the inventory grid
        //iconImage.color = item.iconTint;
        nameText.text = item.displayName;
        priceText.text = price.ToString("N0");
        actionLabel.text = label;
        actionButton.interactable = interactable;
        // Clearing old listeners first prevents rows from firing twice
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => onClicked());
    }
}