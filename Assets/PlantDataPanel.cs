using UnityEngine;
using TMPro;

public class PlantDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text pHText;
    public TMP_Text ammoniaPPMText;
    public TMP_Text nitritePPMText;
    public TMP_Text nitratePPMText;
    public TMP_Text lightRequirementText;
    public TMP_Text lightIntensityText;
    public TMP_Text o2ProductionText;
    public TMP_Text co2NeedsPPMText;
    public TMP_Text priceText;
    public TMP_Text descriptionText;
    public TMP_Text temperatureRangeText;
    public TMP_Text carbonateHardnessText;
    public TMP_Text generalHardnessText;

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

    public void UpdatePlantData(Plant plant)
    {
        nameText.text = plant.name;
        typeText.text = "Type: " + plant.type;
        pHText.text = "pH: " + (plant.pH.Length >= 2 ? plant.pH[0].ToString() + " - " + plant.pH[1].ToString() : "N/A");
        ammoniaPPMText.text = "Ammonia ppm: " + (plant.ammonia_ppm.Length >= 2 ? plant.ammonia_ppm[0].ToString() + " - " + plant.ammonia_ppm[1].ToString() : "N/A");
        nitritePPMText.text = "Nitrite ppm: " + (plant.nitrite_ppm.Length >= 2 ? plant.nitrite_ppm[0].ToString() + " - " + plant.nitrite_ppm[1].ToString() : "N/A");
        nitratePPMText.text = "Nitrate ppm: " + (plant.nitrate_ppm.Length >= 2 ? plant.nitrate_ppm[0].ToString() + " - " + plant.nitrate_ppm[1].ToString() : "N/A");
        lightRequirementText.text = "Light Requirement: " + plant.light_requirement;
        lightIntensityText.text = "Light Intensity (lux): " + plant.light_intensity_lux.ToString();
        o2ProductionText.text = "O2 Production (mg/ph/g): " + (plant.o2_production_mgphg.Length >= 2 ? plant.o2_production_mgphg[0].ToString() + " - " + plant.o2_production_mgphg[1].ToString() : "N/A");
        co2NeedsPPMText.text = "CO2 Needs (ppm): " + (plant.co2_needs_ppm.Length >= 2 ? plant.co2_needs_ppm[0].ToString() + " - " + plant.co2_needs_ppm[1].ToString() : "N/A");
        priceText.text = "$ " + plant.price_usd.ToString();
        descriptionText.text = "Description: " + plant.description;
        temperatureRangeText.text = "Temperature Range (°C): " + (plant.temperature_range_celsius.Length >= 2 ? plant.temperature_range_celsius[0].ToString() + " - " + plant.temperature_range_celsius[1].ToString() : "N/A");
        carbonateHardnessText.text = "Carbonate Hardness (dKH): " + GetHardnessString(plant.carbonate_hardness);
        generalHardnessText.text = "General Hardness (dGH): " + GetHardnessString(plant.general_hardness);
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearPlantData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearPlantData()
    {
        nameText.text = "";
        typeText.text = "";
        pHText.text = "";
        ammoniaPPMText.text = "";
        nitritePPMText.text = "";
        nitratePPMText.text = "";
        lightRequirementText.text = "";
        lightIntensityText.text = "";
        o2ProductionText.text = "";
        co2NeedsPPMText.text = "";
        temperatureRangeText.text = "";
        priceText.text = "";
        descriptionText.text = "";
        carbonateHardnessText.text = "";
        generalHardnessText.text = "";
    }

    private string GetHardnessString(float[] hardnessValues)
    {
        if (hardnessValues != null && hardnessValues.Length >= 2)
        {
            return hardnessValues[0].ToString() + " - " + hardnessValues[1].ToString();
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
