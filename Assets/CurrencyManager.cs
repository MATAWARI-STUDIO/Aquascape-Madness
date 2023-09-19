using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private decimal startingUSD = 10000m; // Starting money for the player

    private decimal currentUSD;

    public decimal StartingUSD
    {
        get { return startingUSD; }
        set { startingUSD = value; }
    }

    public decimal CurrentUSD
    {
        get { return currentUSD; }
        set
        {
            currentUSD = value;
            OnCurrencyUpdated?.Invoke(currentUSD);
        }
    }

    public static CurrencyManager Instance { get; private set; }

    public event Action<decimal> OnCurrencyUpdated;

    private void Awake()
    {
        // Singleton pattern to ensure there is only one instance of the CurrencyManager.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentUSD = startingUSD;
        OnCurrencyUpdated?.Invoke(currentUSD); // Notify listeners about the initial currency value
    }

    public bool CanAfford(decimal amount)
    {
        return currentUSD >= amount;
    }

    public void AddUSD(decimal amount)
    {
        currentUSD += amount;
        OnCurrencyUpdated?.Invoke(currentUSD); // Notify listeners about the updated currency value
    }

    public bool SubtractUSD(decimal amount)
    {
        if (currentUSD >= amount)
        {
            currentUSD -= amount;
            OnCurrencyUpdated?.Invoke(currentUSD); // Notify listeners about the updated currency value
            return true;
        }
        return false; // Return false if the subtraction cannot be performed
    }

    // Add any other currency-related functionality as needed.
}
