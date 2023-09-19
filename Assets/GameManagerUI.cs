using UnityEngine;
using TMPro;

public class GameManagerUI : MonoBehaviour
{
    public TMP_Text playerMoneyText; // Reference to the TMP_Text representing player's money
    private CurrencyManager currencyManager;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        currencyManager.OnCurrencyUpdated += UpdatePlayerMoneyText; // Subscribe to the OnCurrencyUpdated event
        UpdatePlayerMoneyText(currencyManager.CurrentUSD); // Update the player money text with the initial value
    }

    private void UpdatePlayerMoneyText(decimal updatedValue)
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = "$ " + updatedValue.ToString();
        }
    }
}
