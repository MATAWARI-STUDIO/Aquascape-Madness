using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    private CurrencyManager currencyManager;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
    }

    public void PurchasePlant(Plant plant)
    {
        decimal plantPrice = plant.price_usd;
        if (currencyManager.CanAfford(plantPrice))
        {
            currencyManager.SubtractUSD(plantPrice);
            Debug.Log("Purchase successful!");
        }
        else
        {
            Debug.Log("Insufficient funds!");
        }
    }
}
