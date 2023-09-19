using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class FishButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string fishName;
    public FishDataPanel fishDataPanel;
    public TMP_Text playerMoneyText;

    private bool isPointerOver;
    private JSONLoader jsonLoader;
    private CurrencyManager currencyManager;
    private Fish currentFish;

    private void Start()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        fishDataPanel.SetActive(false);
    }

    private void Update()
    {
        if (isPointerOver && !fishDataPanel.IsActive())
        {
            UpdateFishDataPanel();
            fishDataPanel.SetActive(true);
        }
        else if (!isPointerOver && fishDataPanel.IsActive())
        {
            fishDataPanel.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }

    private void UpdateFishDataPanel()
    {
        if (jsonLoader != null)
        {
            Fish fishData = jsonLoader.GetFishDataByName(fishName);
            if (fishData != null)
            {
                currentFish = fishData;
                fishDataPanel.UpdateFishData(fishData);
            }
            else
            {
                Debug.LogError("Fish data is null for fishName: " + fishName);
            }
        }
        else
        {
            Debug.LogError("JSONLoader is null in FishButton");
        }
    }


    public void OnFishButtonClick()
    {
        Debug.Log("Fish button clicked!");
        if (currentFish != null && currencyManager.CanAfford(currentFish.price_usd))
        {
            currencyManager.SubtractUSD(currentFish.price_usd);
            UpdatePlayerMoneyText();
            SoundManager.Instance.PlayKachingSound();
            Debug.Log("Purchase successful!");
        }
        else
        {
            SoundManager.Instance.PlayInsufficientFundsSound();
            Debug.Log("Insufficient funds!");
        }
    }

    private void UpdatePlayerMoneyText()
    {
        if (playerMoneyText != null && currencyManager != null)
        {
            playerMoneyText.text = "$ " + currencyManager.CurrentUSD.ToString("F2");
        }
    }
}
