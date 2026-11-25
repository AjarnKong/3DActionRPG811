using System;
using UnityEngine;
/// <summary>
/// Holds the player gold. Other systems must go through the methods,
/// they can never write to the value directly.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int startingGold = 50;
    public int Gold { get; private set; }
    public event Action<int> OnGoldChanged;
    private void Awake()
    {
        Gold = startingGold;
    }
    private void Start()
    {
        // Announced in Start so listeners that subscribe in OnEnable
        // have already registered and will receive this first update
        OnGoldChanged?.Invoke(Gold);
    }
    public void Add(int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }
    // Checks without changing anything, used to grey out buttons
    public bool CanAfford(int amount)
    {
        return Gold >= amount;
    }
    /// <summary>
    /// Spends gold only when the full amount is available.
    /// Returns false and leaves the balance untouched otherwise.
    /// </summary>
    public bool TrySpend(int amount)
    {
        if (amount <= 0 || Gold < amount)
        {
            return false;
        }
        Gold -= amount;
        OnGoldChanged?.Invoke(Gold);
        return true;
    }
}