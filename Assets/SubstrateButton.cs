using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SubstrateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string substrateName;
    public SubstrateDataPanel substrateDataPanel;
    public TMP_Text playerMoneyText;
    public GameObject shopPanel;
    public GameObject dataObject;
    public Camera dataCamera;
    public GameObject categoryPanel;

    private JSONLoader jsonLoader;
    private CurrencyManager currencyManager;
    private Substrate currentSubstrate;

    private void Start()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        substrateDataPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayHoverSound();
        UpdateSubstrateDataPanel();
        dataObject.SetActive(true);
        dataCamera.enabled = true;
        substrateDataPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dataObject.SetActive(false);
        dataCamera.enabled = false;
        substrateDataPanel.SetActive(false);
    }

    private void UpdateSubstrateDataPanel()
    {
        Substrate substrateData = jsonLoader.GetSubstrateDataByName(substrateName);
        if (substrateData != null)
        {
            currentSubstrate = substrateData;
            substrateDataPanel.UpdateSubstrateData(substrateData);
        }
    }

    public void OnCategoryPanelClicked()
    {
        substrateDataPanel.SetActive(true);
    }

    public void OnSubstrateButtonClick()
    {
        if (currentSubstrate != null && currencyManager.CanAfford((decimal)currentSubstrate.price_usd))
        {
            currencyManager.SubtractUSD((decimal)currentSubstrate.price_usd);
            UpdatePlayerMoneyText();
            SoundManager.Instance.PlayKachingSound();
        }
        else
        {
            SoundManager.Instance.PlayInsufficientFundsSound();
        }

        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    private void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = "$ " + currencyManager.CurrentUSD.ToString("F2");
    }
}
