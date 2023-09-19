using UnityEngine;
using TMPro;

public class FishDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text pHText;
    public TMP_Text ammoniaToleranceText;
    public TMP_Text nitriteToleranceText;
    public TMP_Text nitrateToleranceText;
    public TMP_Text o2ProductionText;
    public TMP_Text co2ProductionText;
    public TMP_Text carbonateHardnessText;
    public TMP_Text generalHardnessText;
    public TMP_Text temperatureRangeText;
    public TMP_Text fishInteractionsText;
    public TMP_Text priceText;
    public TMP_Text descriptionText;

    private bool isActive;
    private bool isMouseOver;

    private void Start()
    {
        SetActive(false);
    }

    private void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            SetActive(!isActive);
        }
    }

    public void UpdateFishData(Fish fish)
    {
        nameText.text = fish.name;
        typeText.text = "Type: " + fish.type;
        pHText.text = "pH Tolerance: " + GetToleranceString(fish.pH_tolerance);
        ammoniaToleranceText.text = "Ammonia Tolerance (ppm): " + GetToleranceString(fish.ammonia_tolerance_ppm);
        nitriteToleranceText.text = "Nitrite Tolerance (ppm): " + GetToleranceString(fish.nitrite_tolerance_ppm);
        nitrateToleranceText.text = "Nitrate Tolerance (ppm): " + GetToleranceString(fish.nitrate_tolerance_ppm);
        o2ProductionText.text = "O2 Production (mg/ph/g): " + GetToleranceString(fish.o2_production_mgphg);
        co2ProductionText.text = "CO2 Production (ppm): " + GetToleranceString(fish.co2_production_ppm);
        carbonateHardnessText.text = "Carbonate Hardness Tolerance (dKH): " + GetToleranceString(fish.carbonate_hardness_Tolerance);
        generalHardnessText.text = "General Hardness Tolerance (dGH): " + GetToleranceString(fish.general_hardness_Tolerance);
        temperatureRangeText.text = "Temperature Range (°C): " + GetToleranceString(fish.temperature_range_celsius);
        fishInteractionsText.text = "Fish Interactions: \n"
            + "Effect on pH: " + fish.interaction_with_plant.effectOnpH.ToString("F2") + "\n"
            + "Effect on Ammonia: " + fish.interaction_with_plant.effectOnAmmonia.ToString("F2") + "\n"
            + "Effect on Nitrite: " + fish.interaction_with_plant.effectOnNitrite.ToString("F2") + "\n"
            + "Effect on Nitrate: " + fish.interaction_with_plant.effectOnNitrate.ToString("F2") + "\n"
            + "Effect on CO2 Production: " + fish.interaction_with_water.effectOnCO2Production.ToString("F2");
        priceText.text = "$ " + fish.price_usd.ToString();
        descriptionText.text = "Description: " + fish.description;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearFishData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearFishData()
    {
        nameText.text = "";
        typeText.text = "";
        pHText.text = "";
        ammoniaToleranceText.text = "";
        nitriteToleranceText.text = "";
        nitrateToleranceText.text = "";
        o2ProductionText.text = "";
        co2ProductionText.text = "";
        carbonateHardnessText.text = "";
        generalHardnessText.text = "";
        temperatureRangeText.text = "";
        fishInteractionsText.text = "";
        priceText.text = "";
        descriptionText.text = "";
    }

    private string GetToleranceString(float[] toleranceValues)
    {
        if (toleranceValues != null && toleranceValues.Length >= 2)
        {
            return toleranceValues[0].ToString() + " - " + toleranceValues[1].ToString();
        }
        else
        {
            return "N/A";
        }
    }

    public void OnPointerEnter()
    {
        isMouseOver = true;
    }

    public void OnPointerExit()
    {
        isMouseOver = false;
    }
}
