using UnityEngine;
using TMPro;

public class FilterDataPanel : MonoBehaviour
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

    private bool isActive;

    private void Start()
    {
        SetActive(false);
    }

    public void UpdateFilterData(Filter filter)
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

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearFilterData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearFilterData()
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
}
