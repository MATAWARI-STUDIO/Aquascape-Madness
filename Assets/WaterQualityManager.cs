using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WaterQualityManager : MonoBehaviour
{
    public float pH; // Change to public
    public float ammoniaLevel; // Change to public
    public float nitriteLevel; // Change to public
    public float nitrateLevel; // Change to public
    public float o2_production_mgphg; // Change to public
    private float co2AbsorptionRate;
    private int waterTemperature = 25;
    private float carbonateHardness;
    private float generalHardness;
    private float nitrogenBalance;
    private float organicNitrogenDecompositionRate = 0.005f; // Adjust the value as needed

    private float totalPlantBiomass;
    public float eutrophicationThreshold = 100.0f;

    [SerializeField] private WaterQualityManager waterQualityManager; // Assign in the Inspector

    private const float MIN_TEMPERATURE = 14f;
    private const float MAX_TEMPERATURE = 34f;

    public float lightEffectOnpH = 0.1f;
    public float lightEffectOnOxygen = 0.2f;
    public float lightEffectOnCO2Absorption = 0.1f;

    public float pHChangeRate = 0.01f;
    public float ammoniaChangeRate = 0.02f;
    public float nitriteChangeRate = 0.01f;
    public float nitrateChangeRate = 0.02f;
    public float oxygenChangeRate = 0.1f;

    public float plantpHEffect = 0.1f;
    public float plantAmmoniaEffect = -0.1f;
    public float plantNitriteEffect = -0.1f;
    public float plantNitrateEffect = -0.1f;
    public float plantOxygenEffect = 0.2f;

    public float[] carbonateHardness_Tolerance = { 4f, 10f };
    public float[] generalHardness_Tolerance = { 4f, 15f };
    public float denitrificationFactor = 0.005f; // You can adjust this value
    public Light[] lightSettings; // Define the array to hold your light settings

    private bool referencesValid = true;

    public Button playButton;
    public Button pauseButton;
    public Button fastForwardButton1x;
    public Button fastForwardButton2x;


    public static WaterQualityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Make sure there's only one instance
        }
    }
    public LightData GetLightData()
    {
        // Implement the logic to retrieve and return light data
        LightData lightData = new LightData(); // Replace with actual implementation
        return lightData;
    }

    public float LightIntensityMultiplier
    {
        get { return lightIntensityMultiplier; }
        set { lightIntensityMultiplier = value; }
    }

    public TextMeshProUGUI speedText;


    private float simulationSpeed = 1.0f;

    private int year;
    private int week;
    private int day;
    private float lightIntensityMultiplier;

    public float TotalPlantBiomass
    {
        get { return totalPlantBiomass; }
        set { totalPlantBiomass = value; }
    }

    public List<Fish> FishInstances
    {
        get { return fishInstances; }
        set { fishInstances = value; }
    }

    public Light[] GetLightSettings()
    {
        return lightSettings; // Return the array of LightSettings that you have
    }

    public float GetCO2AbsorptionRate()
    {
        // Add your implementation here to calculate and return the CO2 absorption rate
        // For example:
        return co2AbsorptionRate;
    }

    public TextMeshProUGUI pHTextUI;
    public TextMeshProUGUI ammoniaTextUI;
    public TextMeshProUGUI nitriteTextUI;
    public TextMeshProUGUI nitrateTextUI;
    public TextMeshProUGUI oxygenTextUI;
    public TextMeshProUGUI co2AbsorptionTextUI;
    public TextMeshProUGUI waterTemperatureText;
    public TextMeshProUGUI carbonateHardnessTextUI;
    public TextMeshProUGUI generalHardnessTextUI;

    public TimeController timeController;

    private Dictionary<Plant, PlantEffect> plantEffects = new Dictionary<Plant, PlantEffect>();
    private Dictionary<Fish, FishEffect> fishEffects = new Dictionary<Fish, FishEffect>();
    private Slider temperatureSlider;
    private float fishpHEffect = 0.1f;
    private float fishAmmoniaEffect = 0.2f;
    private float fishNitriteEffect = 0.1f;
    private float fishNitrateEffect = 0.2f;
    private List<Fish> fishInstances = new List<Fish>();

    private float GetLightIntensityValue()
    {
        // Replace this with your actual logic to obtain the light intensity value
        // For example, you might access a Light component in your scene and retrieve its intensity.
        // Ensure that the returned value is between 0 and 1.
        Light mainLight = FindObjectOfType<Light>(); // Replace with your light source
        if (mainLight != null)
        {
            float lightIntensity = mainLight.intensity / 8.0f; // Adjust the divisor as needed
            lightIntensity = Mathf.Clamp01(lightIntensity); // Ensure the value is between 0 and 1
            return lightIntensity;
        }
        else
        {
            Debug.LogWarning("Light component not found. Defaulting light intensity to 0.5.");
            return 0.5f; // Default value if light source is not found
        }
    }


    private void Start()
    {
        pH = 7.0f;
        ammoniaLevel = 0.0f;
        nitriteLevel = 0.0f;
        nitrateLevel = 0.0f;
        o2_production_mgphg = 8.0f;
        co2AbsorptionRate = 0.1f;

        if (timeController == null)
        {
            timeController = FindObjectOfType<TimeController>();
            if (timeController == null)
            {
                Debug.LogError("TimeController not found in the scene. Make sure it exists.");
            }
        }

        // Find the Slider component in the scene
        temperatureSlider = GameObject.FindObjectOfType<Slider>();

        if (temperatureSlider == null)
        {
            Debug.LogError("Slider component not found in the scene. Make sure it exists.");
        }

        temperatureSlider.minValue = MIN_TEMPERATURE;
        temperatureSlider.maxValue = MAX_TEMPERATURE;
        temperatureSlider.value = waterTemperature;

        temperatureSlider.onValueChanged.AddListener(OnTemperatureSliderValueChanged);

        playButton.onClick.AddListener(PlaySimulation);
        pauseButton.onClick.AddListener(PauseSimulation);
        fastForwardButton1x.onClick.AddListener(FastForward1xSimulation);
        fastForwardButton2x.onClick.AddListener(FastForward2xSimulation);

    }

    private void Update()
    {
        if (!referencesValid)
            return;

        float artificialLightIntensity = GetArtificialLightIntensity();
        UpdateWaterQualityWithArtificialLight(artificialLightIntensity);

        {
            CollectWaterQualityData();
        }


        void CollectWaterQualityData()
        {
            float pHValue = GetPHValue();
            float ammoniaValue = GetAmmoniaValue();
            float oxygenLevel = GetOxygenLevel();

            // TimeManager is used for timestamp calculation, so we still need to check its presence
            TimeManager timeManager = FindObjectOfType<TimeManager>();

            if (timeManager != null)
            {
                float timestamp = (year * 365) + (week * 7) + day + timeManager.CurrentTime / 86400f;
            }
            else
            {
                Debug.LogError("TimeManager instance not found.");
            }
        }


        void UpdateWaterQualityWithArtificialLight(float artificialLightIntensity)
        {
            Debug.Log("Updating simulation...");

            // Check if the game is not paused or fast forwarded
            if (!TimeController.IsGamePausedOrFastForwarded())
            {
                // Calculate the light intensity factor
                float lightIntensityFactor = CalculateLightIntensityFactor();

                // Simulate various environmental changes
                SimulateAmmonification();
                SimulateNitriteChange(nitriteChangeRate);
                SimulateNitrateChange(nitrateChangeRate);
                SimulateOxygenChange(oxygenChangeRate);

                // Initialize the JSONLoader object outside the if block
                JSONLoader jsonLoader = FindObjectOfType<JSONLoader>();

                // Simulate plant nutrient uptake with the adjusted light intensity factor
                if (jsonLoader != null)
                {
                    Plant[] plants = jsonLoader.plantData.plants;

                    foreach (Plant plant in plants)
                    {
                        SimulatePlantNutrientUptake(plant, lightIntensityFactor);
                    }
                }
                else
                {
                    Debug.LogError("JSONLoader component not found on this GameObject.");
                }

                // Calculate and update water quality parameters
                CalculateWaterHardness();
                ClampWaterQualityParameters();
                UpdateWaterStatsUI();

                // Calculate the light intensity factor again
                float calculatedLightIntensityFactor = CalculateLightIntensityFactor();

                // Simulate plant nutrient uptake with the calculated light intensity factor
                if (jsonLoader != null)
                {
                    Plant[] plants = jsonLoader.plantData.plants;

                    foreach (Plant plant in plants)
                    {
                        SimulatePlantNutrientUptake(plant, calculatedLightIntensityFactor);
                    }
                }

                // Apply fish effects with the calculated light intensity factor
                foreach (Fish fish in fishInstances)
                {
                    ApplyFishEffect(fish, calculatedLightIntensityFactor);
                }
            }
        }


    }

    private float GetArtificialLightIntensity()
    {
        // Replace this with your logic to calculate artificial light intensity
        // For example:
        return Mathf.Sin(Time.time) * 0.5f + 0.5f;
    }

    public float CalculateLightIntensityFactor()
    {
        // Calculate the light intensity value (between 0 and 1) using your method
        float lightIntensity = GetLightIntensityValue(); // Replace with your actual method

        // Adjust the light intensity to a factor between 0.5 and 1.0
        float lightIntensityFactor = Mathf.Lerp(0.5f, 1.0f, lightIntensity);

        return lightIntensityFactor;
    }

    public float CalculateIntensityEffect()
    {
        // Water quality value between 0 and 1 (0 being poor, 1 being excellent)
        float waterQualityValue = GetWaterQualityValue(); // Replace with your method to get water quality value

        // Define the range of effect (e.g., how much light intensity changes based on water quality)
        float minEffect = 0.5f; // Minimum effect when water quality is poor
        float maxEffect = 1.5f; // Maximum effect when water quality is excellent

        // Calculate the effect on light intensity based on water quality
        float waterQualityEffect = Mathf.Lerp(minEffect, maxEffect, waterQualityValue);

        return waterQualityEffect;
    }
    private float GetWaterQualityValue()
    {
        // Replace these variables with appropriate initial values
        float initialWaterQuality = 0.7f; // Initial water quality value (between 0 and 1)
        float waterQualityChangeRate = 0.01f; // Rate of change per frame

        // Simulate water quality changes over time (e.g., in the Update method)
        // Make sure this logic is appropriately integrated into your game's simulation loop
        initialWaterQuality += waterQualityChangeRate;

        // Ensure that water quality value stays within the valid range of 0 to 1
        initialWaterQuality = Mathf.Clamp01(initialWaterQuality);

        return initialWaterQuality;
    }

    public void UpdateWaterQualityWithArtificialLight(float artificialLightIntensity)
    {
        // Implement the logic to update water quality using artificial light intensity
        // For example:
        pH += artificialLightIntensity * lightEffectOnpH;
        co2AbsorptionRate += artificialLightIntensity * lightEffectOnCO2Absorption;
        // Adjust pH based on artificial light intensity
        pH += artificialLightIntensity * lightEffectOnpH;

        // Adjust CO2 absorption rate based on artificial light intensity
        co2AbsorptionRate += artificialLightIntensity * lightEffectOnCO2Absorption;
    }

    public void UpdateWaterQuality(float artificialLightIntensity, WaterQualityManager waterQualityManager, Lights lights)
    {
        if (waterQualityManager != null)
        {
            waterQualityManager.UpdateWaterQualityWithArtificialLight(artificialLightIntensity);

            if (lights != null)
            {
                lights.intensity = artificialLightIntensity; // Update light intensity
            }
        }
    }

    public void SimulateFilterEffect(float filterEfficiency)
    {
        float ammoniaReduction = ammoniaLevel * filterEfficiency;
        float nitriteReduction = nitriteLevel * filterEfficiency;
        float nitrateReduction = nitrateLevel * filterEfficiency;

        ammoniaLevel -= ammoniaReduction;
        nitriteLevel -= nitriteReduction;
        nitrateLevel -= nitrateReduction;

        // Clamp values to valid ranges
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
        nitriteLevel = Mathf.Clamp(nitriteLevel, 0.0f, 2.0f);
        nitrateLevel = Mathf.Clamp(nitrateLevel, 0.0f, 2.0f);

        // Update the UI to reflect the changes in water quality
        UpdateWaterStatsUI();

        // Debug log messages for troubleshooting
        Debug.Log($"Ammonia Level Before: {ammoniaLevel + ammoniaReduction}, After: {ammoniaLevel}");
        Debug.Log($"Nitrite Level Before: {nitriteLevel + nitriteReduction}, After: {nitriteLevel}");
        Debug.Log($"Nitrate Level Before: {nitrateLevel + nitrateReduction}, After: {nitrateLevel}");
        Debug.Log($"Filter Efficiency: {filterEfficiency}");

        float totalReduction = ammoniaReduction + nitriteReduction + nitrateReduction;
        Debug.Log($"Total Reduction: {totalReduction}");

        // Debug log message for filter effect
        Debug.Log($"Filter effect applied: Ammonia reduced by {ammoniaReduction}, Nitrite reduced by {nitriteReduction}, Nitrate reduced by {nitrateReduction}");
    }

    public void UpdateWaterQuality(float newpH, float newAmmonia, float newNitrite, float newNitrate, float newOxygen)
    {
        Debug.Log("Updating water quality...");
        pH = newpH;
        ammoniaLevel = newAmmonia;
        nitriteLevel = newNitrite;
        nitrateLevel = newNitrate;
        o2_production_mgphg = newOxygen;

        // Ensure values stay within valid ranges if needed

        // Update the UI to reflect the changes in water quality
        UpdateWaterStatsUI();
        Debug.Log("Water quality updated.");
    }


    private void SimulateAmmonification()
    {
        float organicNitrogen = CalculateOrganicNitrogen();

        // Calculate the ammonium produced based on the organic nitrogen and decomposition rate
        float ammoniumProduced = organicNitrogen * organicNitrogenDecompositionRate;

        // Update the ammonia level and clamp it
        ammoniaLevel += ammoniumProduced;
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
    }

    public void SimulatepHChange(float pHChangeRate)
    {
        pH -= pHChangeRate * Time.deltaTime; // Decrease pH over time
        pH = Mathf.Clamp(pH, 6.0f, 9.0f);
    }


    public void SimulateAmmoniaChange(float reductionAmount)
    {
        ammoniaLevel -= reductionAmount; // Reduce ammonia by the specified amount
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
    }

    public void SimulateNitriteChange(float nitriteChangeRate)
    {
        nitriteLevel += ammoniaLevel * nitriteChangeRate * Time.deltaTime;
        nitriteLevel = Mathf.Clamp(nitriteLevel, 0.0f, 2.0f);
    }

    public void SimulateNitrateChange(float nitrateChangeRate)
    {
        nitrateLevel += nitriteLevel * nitrateChangeRate * Time.deltaTime;
        nitrateLevel = Mathf.Clamp(nitrateLevel, 0.0f, 2.0f);
    }

    public void SimulateOxygenChange(float oxygenChangeRate)
    {
        o2_production_mgphg -= Random.Range(0.0f, oxygenChangeRate) * Time.deltaTime;
        o2_production_mgphg = Mathf.Clamp(o2_production_mgphg, 0.0f, 12.0f);
    }

    private void CalculateWaterHardness()
    {
        carbonateHardness = Mathf.Clamp(10.0f - pH, 4.0f, 10.0f);
        generalHardness = Mathf.Clamp(pH + 5.0f, 4.0f, 15.0f);
    }

    private void ClampWaterQualityParameters()
    {
        pH = Mathf.Clamp(pH, 6.0f, 9.0f);
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
        nitriteLevel = Mathf.Clamp(nitriteLevel, 0.0f, 2.0f);
        nitrateLevel = Mathf.Clamp(nitrateLevel, 0.0f, 2.0f);
        o2_production_mgphg = Mathf.Clamp(o2_production_mgphg, 0.0f, 12.0f);
        co2AbsorptionRate = Mathf.Clamp(co2AbsorptionRate, 0.0f, 2.0f);
        carbonateHardness = Mathf.Clamp(carbonateHardness, carbonateHardness_Tolerance[0], carbonateHardness_Tolerance[1]);
        generalHardness = Mathf.Clamp(generalHardness, generalHardness_Tolerance[0], generalHardness_Tolerance[1]);
    }

    private void OnTemperatureSliderValueChanged(float value)
    {
        waterTemperature = Mathf.RoundToInt(value);
    }

    public void UpdateWaterStatsUI()
    {
        float displayTimeScale = TimeController.IsGameFastForwarded() ? TimeController.Instance.fastForwardMultiplier : 1f;

        pHTextUI.text = "pH: " + (pH * displayTimeScale).ToString("F1");
        ammoniaTextUI.text = "Ammonia ppm: " + (ammoniaLevel * displayTimeScale).ToString("F2");
        nitriteTextUI.text = "Nitrite ppm: " + (nitriteLevel * displayTimeScale).ToString("F2");
        nitrateTextUI.text = "Nitrate ppm: " + (nitrateLevel * displayTimeScale).ToString("F2");
        oxygenTextUI.text = "Dissolved Oxygen: " + (o2_production_mgphg * displayTimeScale).ToString("F1") + " mg/L";
        co2AbsorptionTextUI.text = "CO2 Absorption Rate: " + (co2AbsorptionRate * displayTimeScale).ToString("F2") + " ppm/h";
        waterTemperatureText.text = "Water Temperature: " + waterTemperature + " °C";
        carbonateHardnessTextUI.text = "Carbonate Hardness: " + carbonateHardness.ToString("F1");
        generalHardnessTextUI.text = "General Hardness: " + generalHardness.ToString("F1");
    }

    private void SimulatePlantNutrientUptake(Plant plant, float lightIntensityFactor)
    {
        float nutrientUptakeRate = plant.nutrientUptakeRate * lightIntensityFactor; // Adjust nutrient uptake based on light

        float ammoniaUptake = nutrientUptakeRate * ammoniaLevel;
        float nitriteUptake = nutrientUptakeRate * nitriteLevel;
        float nitrateUptake = nutrientUptakeRate * nitrateLevel;

        ammoniaLevel -= ammoniaUptake;
        nitriteLevel -= nitriteUptake;
        nitrateLevel -= nitrateUptake;

        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
        nitriteLevel = Mathf.Clamp(nitriteLevel, 0.0f, 2.0f);
        nitrateLevel = Mathf.Clamp(nitrateLevel, 0.0f, 2.0f);
    }


    private void UpdateSpeedText()
    {
        string speedTextValue = "";

        if (simulationSpeed == 0)
        {
            speedTextValue = "Paused Speed";
        }
        else if (simulationSpeed == 1)
        {
            speedTextValue = "Normal Speed";
        }
        else if (simulationSpeed == 2)
        {
            speedTextValue = "2x Speed";
        }
        else if (simulationSpeed == 4)
        {
            speedTextValue = "4x Speed";
        }

        speedText.text = speedTextValue;
    }

    private void PlaySimulation()
    {
        if (simulationSpeed == 0)
        {
            SetSimulationSpeed(1.0f);
        }
        else if (simulationSpeed > 1)
        {
            SetSimulationSpeed(1.0f);
            day++; // Increment day to account for time passed during fast forward
        }
    }

    private void PauseSimulation()
    {
        SetSimulationSpeed(0.0f); // Pause simulation
    }

    private void FastForward1xSimulation()
    {
        SetSimulationSpeed(2.0f); // Fast forward 1x
    }

    private void FastForward2xSimulation()
    {
        SetSimulationSpeed(4.0f); // Fast forward 2x
    }

    private void SetSimulationSpeed(float speed)
    {
        simulationSpeed = speed;
        Time.timeScale = speed;
        UpdateSpeedText();
    }

    public void ApplyPlantEffect(Plant plant, float lightIntensityFactor, float currentTemperature)
    {
        // Check if the provided plant and its data are valid
        if (plant != null && plant.pH.Length > 0 && plant.ammonia_ppm.Length > 0 && plant.nitrite_ppm.Length > 0 && plant.nitrate_ppm.Length > 0)
        {
            // Extract plant data for calculations
            float plantpH = plant.pH[0];
            float plantAmmonia = plant.ammonia_ppm[0];
            float plantNitrite = plant.nitrite_ppm[0];
            float plantNitrate = plant.nitrate_ppm[0];
            float plantOxygen = plant.o2_production_mgphg[0];

            // Update water quality based on plant effects
            pH += plantpHEffect * plantpH;
            ammoniaLevel += plantAmmoniaEffect * plantAmmonia;
            nitriteLevel += plantNitriteEffect * plantNitrite;
            nitrateLevel += plantNitrateEffect * plantNitrate;
            o2_production_mgphg += plantOxygen;

            // Create a plant effect object to store the calculated effects
            PlantEffect effect = new PlantEffect()
            {
                plantpHEffect = plantpHEffect * plantpH,
                plantAmmoniaEffect = plantAmmoniaEffect * plantAmmonia,
                plantNitriteEffect = plantNitriteEffect * plantNitrite,
                plantNitrateEffect = plantNitrateEffect * plantNitrate,
                plantOxygenEffect = plantOxygen
            };

            // Check if the plant is already in the dictionary before adding the effect
            if (!plantEffects.ContainsKey(plant))
            {
                plantEffects.Add(plant, effect);
            }
        }
        else
        {
            // Log a warning for invalid plant data
            Debug.LogWarning("Invalid plant data for effect application.");
        }
    }


    public void RemovePlantEffect(Plant plant)
    {
        if (plant != null && plantEffects.ContainsKey(plant))
        {
            PlantEffect effect = plantEffects[plant];

            // Revert the changes made by the plant
            pH -= effect.plantpHEffect;
            ammoniaLevel -= effect.plantAmmoniaEffect;
            nitriteLevel -= effect.plantNitriteEffect;
            nitrateLevel -= effect.plantNitrateEffect;
            o2_production_mgphg -= effect.plantOxygenEffect;

            // Remove the plant effect from the dictionary
            plantEffects.Remove(plant);

            Debug.Log("Plant effect on water reverted: pH=" + pH + " Ammonia=" + ammoniaLevel + " Nitrite=" + nitriteLevel + " Nitrate=" + nitrateLevel + " Oxygen=" + o2_production_mgphg);
        }
        else
        {
            Debug.LogWarning("Invalid plant data");
        }
    }

    public void ApplyFishEffect(Fish fish, float lightIntensityFactor)
    {
        if (fish != null && !fishEffects.ContainsKey(fish))
        {
            float fishpH = fish.pH_tolerance[0];
            float fishAmmonia = fish.ammonia_tolerance_ppm[0];
            float fishNitrite = fish.nitrite_tolerance_ppm[0];
            float fishNitrate = fish.nitrate_tolerance_ppm[0];
            float fishCO2Absorption = fish.interaction_with_water.effectOnCO2Production;

            // Adjust effects based on light intensity
            fishpH *= lightIntensityFactor;
            fishAmmonia *= lightIntensityFactor;
            fishNitrite *= lightIntensityFactor;
            fishNitrate *= lightIntensityFactor;

            pH += fishpHEffect * fishpH;
            ammoniaLevel += fishAmmoniaEffect * fishAmmonia;
            nitriteLevel += fishNitriteEffect * fishNitrite;
            nitrateLevel += fishNitrateEffect * fishNitrate;
            co2AbsorptionRate += fishCO2Absorption;

            // Store the fish effect in the dictionary
            FishEffect effect = new FishEffect()
            {
                fishpHEffect = fishpHEffect * fishpH,
                fishAmmoniaEffect = fishAmmoniaEffect * fishAmmonia,
                fishNitriteEffect = fishNitriteEffect * fishNitrite,
                fishNitrateEffect = fishNitrateEffect * fishNitrate,
                fishCO2AbsorptionEffect = fishCO2Absorption
            };

            // Update pH, ammonia, nitrite, nitrate, and CO2 absorption rate based on light intensity
            pH += fishpHEffect * fishpH * lightIntensityFactor;
            ammoniaLevel += fishAmmoniaEffect * fishAmmonia * lightIntensityFactor;
            nitriteLevel += fishNitriteEffect * fishNitrite * lightIntensityFactor;
            nitrateLevel += fishNitrateEffect * fishNitrate * lightIntensityFactor;
            co2AbsorptionRate += fishCO2Absorption * lightIntensityFactor;

            fishEffects.Add(fish, effect);

            Debug.Log("Fish effect on water: pH=" + pH + " Ammonia=" + ammoniaLevel + " Nitrite=" + nitriteLevel + " Nitrate=" + nitrateLevel + " CO2 Absorption=" + co2AbsorptionRate);
        }
        else
        {
            Debug.LogWarning("Invalid fish data");
        }
    }


    public void RemoveFishEffect(Fish fish, float lightIntensityFactor)
    {
        if (fish != null && fishEffects.ContainsKey(fish))
        {
            FishEffect effect = fishEffects[fish];

            // Revert the changes made by the fish
            pH -= effect.fishpHEffect;
            ammoniaLevel -= effect.fishAmmoniaEffect;
            nitriteLevel -= effect.fishNitriteEffect;
            nitrateLevel -= effect.fishNitrateEffect;
            co2AbsorptionRate -= effect.fishCO2AbsorptionEffect;

            // Remove the fish effect from the dictionary
            fishEffects.Remove(fish);

            Debug.Log("Fish effect on water reverted: pH=" + pH + " Ammonia=" + ammoniaLevel + " Nitrite=" + nitriteLevel + " Nitrate=" + nitrateLevel + " CO2 Absorption=" + co2AbsorptionRate);
        }
        else
        {
            Debug.LogWarning("Invalid fish data");
        }
    }

    public float GetAmmoniaLevel()
    {
        return ammoniaLevel;
    }

    public float GetNitrateLevel()
    {
        return nitrateLevel;
    }

    public float GetNitriteLevel()
    {
        return nitriteLevel;
    }

    public float GetOxygenProduction()
    {
        return o2_production_mgphg;
    }

    public float GetpH()
    {
        return pH;
    }

    public float GetPHValue()
    {
        // Simulating pH value between 6.5 and 8.5
        float calculatedPHValue = Random.Range(6.5f, 8.5f);
        return calculatedPHValue;
    }

    public float GetAmmoniaValue()
    {
        // Simulating ammonia value between 0.1 and 2.0
        float calculatedAmmoniaValue = Random.Range(0.1f, 2.0f);
        return calculatedAmmoniaValue;
    }

    public float GetOxygenLevel()
    {
        // Simulating oxygen level between 5.0 and 10.0
        float calculatedOxygenLevel = Random.Range(5.0f, 10.0f);
        return calculatedOxygenLevel;
    }


    private float CalculateOrganicNitrogen()
    {
        // Implement your logic to calculate organic nitrogen
        // For example:
        float totalOrganicMatter = 1.0f;
        float organicNitrogen = totalOrganicMatter;
        return organicNitrogen;
    }

}


// Helper class to store the plant's effect on water
public class PlantEffect
{
    public float plantpHEffect;
    public float plantAmmoniaEffect;
    public float plantNitriteEffect;
    public float plantNitrateEffect;
    public float plantOxygenEffect;
}

// Helper class to store the fish's effect on water
public class FishEffect
{
    public float fishpHEffect;
    public float fishAmmoniaEffect;
    public float fishNitriteEffect;
    public float fishNitrateEffect;
    public float fishCO2AbsorptionEffect;
}
