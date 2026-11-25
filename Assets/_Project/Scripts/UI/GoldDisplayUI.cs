using TMPro;
using UnityEngine;
/// <summary>
/// Keeps the gold label in sync with the currency manager.
/// </summary>
public class GoldDisplayUI : MonoBehaviour
{
    [SerializeField] private CurrencyManager currency;
    [SerializeField] private TextMeshProUGUI goldText;
    private void OnEnable()
    {
        currency.OnGoldChanged += UpdateLabel;
    }
    private void OnDisable()
    {
        currency.OnGoldChanged -= UpdateLabel;
    }
    private void UpdateLabel(int amount)
    {
        // N0 inserts thousand separators, so 12500 becomes 12,500
        goldText.text = amount.ToString("N0");
    }
}