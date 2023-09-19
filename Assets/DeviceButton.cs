using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DeviceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string deviceName;
    public FilterDataPanel deviceDataPanel;
    public TMP_Text playerMoneyText;

    private bool isPointerOver;
    private JSONLoader jsonLoader;
    private CurrencyManager currencyManager;
    private Filter currentDevice;

    private void Start()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        deviceDataPanel.SetActive(false);
    }

    private void Update()
    {
        if (isPointerOver && !deviceDataPanel.IsActive())
        {
            UpdateDeviceDataPanel();
            deviceDataPanel.SetActive(true);
        }
        else if (!isPointerOver && deviceDataPanel.IsActive())
        {
            deviceDataPanel.SetActive(false);
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

    private void UpdateDeviceDataPanel()
    {
        if (jsonLoader != null)
        {
            Filter deviceData = jsonLoader.GetFilterByName(deviceName);
            if (deviceData != null)
            {
                currentDevice = deviceData;
                deviceDataPanel.UpdateFilterData(deviceData);
            }
            else
            {
                Debug.LogError("Device data is null for deviceName: " + deviceName);
            }
        }
        else
        {
            Debug.LogError("JSONLoader is null in DeviceButton");
        }
    }

    public void OnDeviceButtonClick()
    {
        if (currentDevice != null && currencyManager.CanAfford((decimal)currentDevice.price))
        {
            currencyManager.SubtractUSD((decimal)currentDevice.price);
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
