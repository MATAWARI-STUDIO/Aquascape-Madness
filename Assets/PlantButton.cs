using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlantButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string plantName;
    public PlantDataPanel plantDataPanel;
    public TMP_Text playerMoneyText;

    private bool isPointerOver;
    private JSONLoader jsonLoader;
    private CurrencyManager currencyManager;
    private Plant currentPlant;

    private void Start()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        plantDataPanel.SetActive(false);
    }

    private void Update()
    {
        if (isPointerOver && !plantDataPanel.IsActive())
        {
            UpdatePlantDataPanel();
            plantDataPanel.SetActive(true);
        }
        else if (!isPointerOver && plantDataPanel.IsActive())
        {
            plantDataPanel.SetActive(false);
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

    private void UpdatePlantDataPanel()
    {
        if (jsonLoader != null)
        {
            Plant plantData = jsonLoader.GetPlantDataByName(plantName);
            if (plantData != null)
            {
                currentPlant = plantData;
                plantDataPanel.UpdatePlantData(plantData);
            }
            else
            {
                Debug.LogError("Plant data is null for plantName: " + plantName);
            }
        }
        else
        {
            Debug.LogError("JSONLoader is null in PlantButton");
        }
    }

    public void OnPlantButtonClick()
    {
        Debug.Log("Plant button clicked!");
        if (currentPlant != null && currencyManager.CanAfford(currentPlant.price_usd))
        {
            currencyManager.SubtractUSD(currentPlant.price_usd);
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
