using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantInfoPanel : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text typeText;
    public Slider pHSlider;
    public Slider ammoniaSlider;
    public Slider nitriteSlider;
    public Slider nitrateSlider;
    public Slider o2ProductionSlider;
    public Slider co2AbsorptionSlider;
    public Slider lightIntensitySlider;
    public Slider temperatureSlider;
    public Slider growthRateSlider;
    public Slider nutrientReleaseRateSlider;
    public Slider oxygenConsumptionRateSlider;
    public Slider healthSlider;
    public Slider stressSlider;
    public GameObject closeButton;

    public Color healthyColor = Color.green;
    public Color stressedColor = Color.red;

    private Plant currentPlant;
    public PlantInfoPanel plantInfoPanel; // Reference to the PlantInfoPanel script

    public JSONLoader jsonLoader;

    public void UpdatePlantInfo(Plant plant)
    {
        Debug.Log("UpdatePlantInfo started.");

        if (plant == null)
        {
            Debug.LogError("Plant object is null.");
            return;
        }

        Debug.Log("Plant name: " + plant.name);
        Debug.Log("Plant type: " + plant.type);

        nameLabel.text = plant.name;
        typeText.text = "Type: " + plant.type;

        pHSlider.minValue = plant.pH[0];
        pHSlider.maxValue = plant.pH[1];
        pHSlider.value = (plant.pH[0] + plant.pH[1]) / 2f;

        ammoniaSlider.minValue = plant.ammonia_ppm[0];
        ammoniaSlider.maxValue = plant.ammonia_ppm[1];
        ammoniaSlider.value = (plant.ammonia_ppm[0] + plant.ammonia_ppm[1]) / 2f;

        nitriteSlider.minValue = plant.nitrite_ppm[0];
        nitriteSlider.maxValue = plant.nitrite_ppm[1];
        nitriteSlider.value = (plant.nitrite_ppm[0] + plant.nitrite_ppm[1]) / 2f;

        nitrateSlider.minValue = plant.nitrate_ppm[0];
        nitrateSlider.maxValue = plant.nitrate_ppm[1];
        nitrateSlider.value = (plant.nitrate_ppm[0] + plant.nitrate_ppm[1]) / 2f;

        o2ProductionSlider.minValue = plant.o2_production_mgphg[0];
        o2ProductionSlider.maxValue = plant.o2_production_mgphg[1];
        o2ProductionSlider.value = (plant.o2_production_mgphg[0] + plant.o2_production_mgphg[1]) / 2f;

        co2AbsorptionSlider.minValue = plant.co2_needs_ppm[0];
        co2AbsorptionSlider.maxValue = plant.co2_needs_ppm[1];
        co2AbsorptionSlider.value = (plant.co2_needs_ppm[0] + plant.co2_needs_ppm[1]) / 2f;

        lightIntensitySlider.value = plant.light_intensity_lux;

        temperatureSlider.minValue = plant.temperature_range_celsius[0];
        temperatureSlider.maxValue = plant.temperature_range_celsius[1];

        // Initialize jsonLoader if it's null
        if (jsonLoader == null)
        {
            jsonLoader = FindObjectOfType<JSONLoader>();
        }

        float currentTemperature = jsonLoader.GetCurrentTemperature();

        float adjustedGrowthRate = plant.CalculateAdjustedGrowthRate(currentTemperature);

        temperatureSlider.value = currentTemperature;
        growthRateSlider.value = adjustedGrowthRate;

        nutrientReleaseRateSlider.value = plant.nutrientReleaseRate;
        oxygenConsumptionRateSlider.value = plant.oxygenConsumptionRate;

        healthSlider.value = plant.health / 100f;
        healthSlider.fillRect.GetComponent<Image>().color = plant.health >= 70 ? healthyColor : stressedColor;

        stressSlider.value = plant.stress / 100f;
        stressSlider.fillRect.GetComponent<Image>().color = plant.stress <= 30 ? healthyColor : stressedColor;

        currentPlant = plant;

        Debug.Log("UpdatePlantInfo finished.");
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);

        if (!active)
        {
            ClearPlantData();
        }
    }

    public void ClearPlantData()
    {
        pHSlider.value = 0;
        ammoniaSlider.value = 0;
        nitriteSlider.value = 0;
        nitrateSlider.value = 0;
        o2ProductionSlider.value = 0;
        co2AbsorptionSlider.value = 0;
        lightIntensitySlider.value = 0;
        temperatureSlider.value = 0;
        growthRateSlider.value = 0;
        nutrientReleaseRateSlider.value = 0;
        oxygenConsumptionRateSlider.value = 0;
        healthSlider.value = 0;
        stressSlider.value = 0;

        nameLabel.text = "";
        typeText.text = "";
    }

    public void OnCloseButtonClicked()
    {
        plantInfoPanel.ClosePanel();
    }
}
