using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FilterInfoPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text descriptionText;
    public TMP_Text effectivenessText;
    public TMP_Text filterMediaText;
    public TMP_Text filterCapacityText;
    public TMP_Text pHChangeRateText;
    public TMP_Text ammoniaChangeRateText;
    public TMP_Text nitriteChangeRateText;
    public TMP_Text nitrateChangeRateText;
    public TMP_Text oxygenChangeRateText;
    public TMP_Text priceText;

    public JSONLoader jsonLoader;
    public Button closeButton;

    private bool isActive;

    private void Start()
    {
        // Check if the closeButton is properly assigned in the Inspector
        if (closeButton != null)
        {
            // Add a listener to the button's click event
            closeButton.onClick.AddListener(ClosePanel);
        }
        else
        {
            Debug.LogError("closeButton is not assigned in the Inspector.");
        }

        // Assuming SetActive is a method to control the panel's visibility
        SetActive(false);
    }


    public void SetJSONLoader(JSONLoader loader)
    {
        jsonLoader = loader;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            closeButton.gameObject.SetActive(true);
            ClearFilterInfo();
        }
        else
        {
            closeButton.gameObject.SetActive(false);
        }
    }

    public void UpdateFilterInfo(Filter filter)
    {
        nameText.text = filter.displayName;
        typeText.text = "Type: " + filter.type;
        descriptionText.text = "Description: " + filter.description;
        effectivenessText.text = "Effectiveness: " + filter.effectiveness.ToString();
        filterMediaText.text = "Filter Media: " + filter.filterMedia;
        filterCapacityText.text = "Filter Capacity: " + filter.filterCapacity.ToString();
        pHChangeRateText.text = "pH Change Rate: " + filter.pHChangeRate.ToString("0.00");
        ammoniaChangeRateText.text = "Ammonia Change Rate: " + filter.ammoniaChangeRate.ToString("0.00");
        nitriteChangeRateText.text = "Nitrite Change Rate: " + filter.nitriteChangeRate.ToString("0.00");
        nitrateChangeRateText.text = "Nitrate Change Rate: " + filter.nitrateChangeRate.ToString("0.00");
        oxygenChangeRateText.text = "Oxygen Change Rate: " + filter.oxygenChangeRate.ToString("0.00");
        priceText.text = "$ " + filter.price.ToString("0.00");
    }

    private void ClearFilterInfo()
    {
        nameText.text = "";
        typeText.text = "";
        descriptionText.text = "";
        effectivenessText.text = "";
        filterMediaText.text = "";
        filterCapacityText.text = "";
        pHChangeRateText.text = "";
        ammoniaChangeRateText.text = "";
        nitriteChangeRateText.text = "";
        nitrateChangeRateText.text = "";
        oxygenChangeRateText.text = "";
        priceText.text = "";
    }

    private void ClosePanel()
    {
        SetActive(false);
    }
}
