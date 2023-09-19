using UnityEngine;

public class WaterQualityParameters : MonoBehaviour
{
    [Header("Water Quality Thresholds")]
    public float maxAmmoniaLevel = 100.0f;
    public float minOxygenLevel = 0.0f;
    public float maxOxygenLevel = 10.0f;
    public float maxNitrateLevel = 1.0f;
    public float minNitrateLevel = 0.0f;
    public float maxNitriteLevel = 1.0f;
    public float minNitriteLevel = 0.0f;
    public float minPhosphorusLevel = 0.0f;
    public float maxPhosphorusLevel = 1.0f;
    public float maxPotassiumLevel = 1.0f;
    public float minPotassiumLevel = 1.0f;
    public float maxCalciumLevel = 1.0f;
    public float minCalciumLevel = 1.0f;
    public float bacteriaPopulation = 0.0f;

    [Header("Water Parameters")]
    [SerializeField] private float nitrate = 0.0f;
    [SerializeField] private float potassium = 0.0f;
    [SerializeField] private float phosphorus = 0.0f;
    [SerializeField] private float calcium = 0.0f;
    [SerializeField] private float temperature = 25.0f;
    [SerializeField] private float pH = 7.0f;
    [SerializeField] private float wasteLevel = 100.0f;
    [SerializeField] private float nutrientLevel = 0.0f;
    [SerializeField] private float algaePopulation = 0.0f;
    [SerializeField] private float ammoniaLevel = 0.0f;
    [SerializeField] private float oxygenLevel = 100.0f;
    [SerializeField] private float nitrite = 0.0f;
    [SerializeField] private float toxinLevel = 0.0f;
    [SerializeField] private WaterBody waterBody;

    private const float UPDATE_INTERVAL = 1f;
    private float timeSinceLastUpdate = 0f;

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= UPDATE_INTERVAL)
        {
            UpdateWaterParameters();
            UpdateNutrientLevels();
            UpdateParameters();
            timeSinceLastUpdate = 0f;
        }
    }

    public void UpdateParameters()
    {
        SimulateCalciumChange();
        SimulateToxinChange();
        SimulateWasteChange();
        SimulatepHChange();
    }

    private void SimulateCalciumChange()
    {
        float calciumChange = Random.Range(-0.1f, 0.1f) * Time.timeScale;
        calcium += calciumChange;
        calcium = Mathf.Clamp(calcium, minCalciumLevel, maxCalciumLevel);
    }

    private void SimulateToxinChange()
    {
        float toxinIncreaseRate = 0.02f * Time.timeScale;
        toxinLevel += toxinIncreaseRate;
        toxinLevel = Mathf.Clamp(toxinLevel, 0, 100);
    }

    private void SimulateWasteChange()
    {
        float wasteDecreaseRate = 0.01f * Time.timeScale;
        wasteLevel -= wasteDecreaseRate;
        wasteLevel = Mathf.Max(0, wasteLevel);
    }

    private void SimulatepHChange()
    {
        float pHChange = Random.Range(-0.05f, 0.05f) * Time.timeScale;
        pH += pHChange;
        pH = Mathf.Clamp(pH, 0, 14);
    }

    public void UpdateWaterParameters()
    {
        SimulateTemperatureChange();
        SimulatePHChange();
        SimulateAmmoniaChange();
        SimulateOxygenChange();
        NitrateNaturalDecay();
    }

    private void NitrateNaturalDecay()
    {
        nitrate -= 0.01f * nitrate * Time.deltaTime;
        nitrate = Mathf.Clamp(nitrate, 0, maxNitrateLevel);
    }

    public void UpdateNutrientLevels()
    {
        SimulateNutrientUptake();
        SimulateNutrientRelease();
    }

    private void SimulateTemperatureChange()
    {
        temperature += Random.Range(-0.5f, 0.5f) * Time.timeScale;
        AdjustNutrientLevelsBasedOnTemperature();
    }

    private void SimulatePHChange()
    {
        pH += Random.Range(-0.05f, 0.05f) * Time.timeScale;
        AdjustNutrientLevelsBasedOnPH();
    }

    private void SimulateAmmoniaChange()
    {
        ammoniaLevel += Random.Range(-0.1f, 0.1f) * Time.timeScale;
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, maxAmmoniaLevel);
    }

    private void SimulateOxygenChange()
    {
        oxygenLevel += Random.Range(-0.1f, 0.1f) * Time.timeScale;
        oxygenLevel = Mathf.Clamp(oxygenLevel, minOxygenLevel, maxOxygenLevel);
    }

    private void SimulateNutrientUptake()
    {
        AdjustNutrientLevels(-waterBody.NutrientUptakeRate * Time.timeScale);
    }

    private void SimulateNutrientRelease()
    {
        AdjustNutrientLevels(0.02f * Time.timeScale);
    }

    private void AdjustNutrientLevelsBasedOnTemperature()
    {
        float temperatureFactor = temperature > 28.0f ? 0.05f : -0.05f;
        nitrate += temperatureFactor * Time.timeScale;
        potassium += temperatureFactor * Time.timeScale;
        phosphorus += temperatureFactor * Time.timeScale;

        ClampNutrientLevels();
    }

    private void AdjustNutrientLevelsBasedOnPH()
    {
        float pHFactor = pH < 6.5f ? 0.05f : -0.05f;
        nitrate += pHFactor * Time.timeScale;
        potassium += pHFactor * Time.timeScale;
        phosphorus += pHFactor * Time.timeScale;

        ClampNutrientLevels();
    }

    private void ClampNutrientLevels()
    {
        nitrate = Mathf.Clamp(nitrate, 0, maxNitrateLevel);
        potassium = Mathf.Clamp(potassium, 0, 100);
        phosphorus = Mathf.Clamp(phosphorus, 0, maxPhosphorusLevel);
    }

    public void BacterialConversion()
    {
        float conversionRate = 0.1f;
        ConvertWasteToNutrients(conversionRate);
    }

    private void ConvertWasteToNutrients(float conversionRate)
    {
        float convertedWaste = wasteLevel * conversionRate * Time.timeScale;
        wasteLevel -= convertedWaste;
        nutrientLevel += convertedWaste;
        ammoniaLevel += convertedWaste;
        bacteriaPopulation += convertedWaste;
    }

    public float MaxBacteriaPopulation
    {
        get
        {
            if (waterBody != null)
            {
                return (waterBody.Width * waterBody.Length * waterBody.Depth) * 10f;
            }
            else
            {
                Debug.LogError("WaterBody is not assigned! Ensure the WaterBody component is attached to the GameObject.");
                return 10000.0f;
            }
        }
    }

    public void UpdateBacteriaPopulation()
    {
        float growthRate = 0.05f;
        bacteriaPopulation += bacteriaPopulation * growthRate * nutrientLevel * Time.timeScale;
    }

    public void AdjustAlgaePopulation(float amount)
    {
        algaePopulation += amount;
    }

    public float GetCO2AbsorptionRate()
    {
        return CalculateCO2AbsorptionRate();
    }

    private float CalculateCO2AbsorptionRate()
    {
        float baseAbsorptionRate = 1.0f;
        float algaeFactor = Mathf.Clamp01(algaePopulation / 1000.0f);
        float temperatureFactor = Mathf.Clamp01(1.0f - (temperature - 25.0f) * 0.02f);
        float pHFactor = pH < 7.0f ? pH / 7.0f : 1.0f;
        float wasteFactor = Mathf.Clamp01(1.0f - wasteLevel * 0.01f);
        float absorptionRate = baseAbsorptionRate * algaeFactor * temperatureFactor * pHFactor * wasteFactor;
        float randomFactor = Random.Range(0.8f, 1.2f);
        return absorptionRate * randomFactor;
    }

    public void AdjustNutrientLevels(float amount)
    {
        float ammoniaReleasePercentage = 0.5f;
        float nitrateReleasePercentage = 0.3f;
        float phosphateReleasePercentage = 0.2f;

        float ammoniaReleased = amount * ammoniaReleasePercentage;
        float nitrateReleased = amount * nitrateReleasePercentage;
        float phosphateReleased = amount * phosphateReleasePercentage;

        ammoniaLevel += ammoniaReleased;
        nitrate += nitrateReleased;
        phosphorus += phosphateReleased;

        ClampNutrientLevels();
    }
    public void AdjustWaterQualityBasedOnSubstrate(Substrate substrate)
    {
        pH += substrate.pH_effect;
        ammoniaLevel += substrate.interactions.water.effectOnAmmonia;
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0, maxAmmoniaLevel);

        // You can add other adjustments based on substrate here
    }

    public void AdjustNutrientLevelsBasedOnSubstrate(Substrate substrate)
    {
        nitrate += substrate.interactions.water.effectOnNitrate;
        nitrate = Mathf.Clamp(nitrate, 0, maxNitrateLevel);

        // You can add other adjustments based on substrate here
    }

    public void AdjustpHLevel(float amount)
    {
        pH += amount;
        AdjustAmmoniaLevelBasedOnpH(amount);
    }

    private void AdjustAmmoniaLevelBasedOnpH(float amount)
    {
        float ammoniaAdjustment = amount > 0 ? 0.05f : -0.03f;
        ammoniaLevel += ammoniaAdjustment * amount;
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0, maxAmmoniaLevel);
    }

    public void AdjustNitriteLevel(float changeAmount)
    {
        nitrite += changeAmount;
        nitrite = Mathf.Clamp(nitrite, minNitriteLevel, maxNitriteLevel);
    }


    public void AdjustNitrateLevel(float amount)
    {
        nitrate += amount;
        AdjustAlgaePopulationBasedOnNitrate(amount);
    }

    private void AdjustAlgaePopulationBasedOnNitrate(float amount)
    {
        float algaeAdjustment = amount > 0 ? 0.1f : 0.05f;
        algaePopulation += algaeAdjustment * amount;
        algaePopulation = Mathf.Clamp(algaePopulation, 0, 10000.0f);
        if (algaePopulation > 5000.0f)
        {
            oxygenLevel -= 0.05f * (algaePopulation - 5000.0f);
            oxygenLevel = Mathf.Clamp(oxygenLevel, minOxygenLevel, maxOxygenLevel);
        }
    }

    public void AdjustAmmoniaLevel(float amount)
    {
        ammoniaLevel += amount;
        AdjustOxygenLevelBasedOnAmmonia(amount);
    }

    private void AdjustOxygenLevelBasedOnAmmonia(float amount)
    {
        float oxygenAdjustment = amount > 0 ? -0.1f : 0.05f;
        oxygenLevel += oxygenAdjustment * amount;
        oxygenLevel = Mathf.Clamp(oxygenLevel, minOxygenLevel, maxOxygenLevel);
    }

    public void AdjustOxygenLevel(float amount)
    {
        oxygenLevel += amount;
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0.0f, maxOxygenLevel);
    }

    public void AdjustToxinLevel(float amount)
    {
        toxinLevel += amount;
    }

    public float GetOxygenLevel() => oxygenLevel;
    public float GetToxinLevel() => toxinLevel;

    public float GetBacteriaPopulation()
    {
        return bacteriaPopulation;
    }

    public void SetBacteriaPopulation(float value)
    {
        bacteriaPopulation = value;
    }

    public void AdjustBacteriaPopulation(float amount)
    {
        bacteriaPopulation += amount;
        ConvertAmmoniaAndNitriteToNitrate(amount);
    }

    private void ConvertAmmoniaAndNitriteToNitrate(float amount)
    {
        float conversionRate = 0.1f;
        float convertedAmmonia = amount * conversionRate;
        ammoniaLevel -= convertedAmmonia;
        nitrite += convertedAmmonia;

        float convertedNitrite = amount * conversionRate;
        nitrite -= convertedNitrite;
        nitrate += convertedNitrite * 2f;

        ClampNutrientLevels();
    }

    public void AdjustWasteLevel(float amount)
    {
        wasteLevel += amount;
        ammoniaLevel += 0.2f * amount;
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0, maxAmmoniaLevel);
    }

    public float GetNutrientLevel()
    {
        return Mathf.Clamp(nutrientLevel, 0, 100);
    }

    public float GetPhosphorusLevel()
    {
        return Mathf.Clamp(phosphorus, 0, maxPhosphorusLevel);
    }

    public float GetWasteLevel()
    {
        return Mathf.Max(0, wasteLevel);
    }

    public float GetpH()
    {
        return Mathf.Clamp(pH, 0, 14);
    }

    public float GetAmmoniaLevel()
    {
        return Mathf.Clamp(ammoniaLevel, 0, maxAmmoniaLevel);
    }

    public float GetNitriteLevel()
    {
        return Mathf.Clamp(nitrite, 0, maxNitriteLevel);
    }

    public float GetNitrateLevel()
    {
        return Mathf.Clamp(nitrate, 0, maxNitrateLevel);
    }

    public float GetTemperature()
    {
        return Mathf.Clamp(temperature, -5, 100);
    }

    public float GetAlgaePopulation()
    {
        return algaePopulation;
    }

    public void SetTemperature(float value)
    {
        temperature = Mathf.Clamp(value, -5, 100);
    }

    public void ApplyWaterChange(float changeFactor)
    {
        AdjustNutrientLevels(-changeFactor);
    }

    public float GetOxygenProduction()
    {
        return CalculateOxygenProduction();
    }

    private float CalculateOxygenProduction()
    {
        float algaeOxygenProduction = algaePopulation * 0.05f;
        float bacteriaOxygenConsumption = bacteriaPopulation * 0.02f;
        return algaeOxygenProduction - bacteriaOxygenConsumption;
    }

    public float BacteriaPopulation { get; private set; }

    public float AlgaePopulation
    {
        get { return algaePopulation; }
    }

    public float GetCurrentNitrate()
    {
        return CalculateCurrentNitrate();
    }

    private float CalculateCurrentNitrate()
    {
        float bacterialConversionRate = bacteriaPopulation / MaxBacteriaPopulation;
        nitrate += (ammoniaLevel + nitrite) * bacterialConversionRate;

        float plantUptakeRate = algaePopulation / 2500.0f;
        nitrate -= nitrate * plantUptakeRate;

        return Mathf.Clamp(nitrate, 0, maxNitrateLevel);
    }

    public float GetCurrentPotassium()
    {
        return CalculateCurrentPotassium();
    }

    private float CalculateCurrentPotassium()
    {
        float runoffFactor = temperature > 28.0f ? 0.02f : 0.01f;
        potassium += runoffFactor;

        float plantUptakeRate = algaePopulation / 10000.0f;
        potassium -= potassium * plantUptakeRate;

        return Mathf.Clamp(potassium, 0, 100);
    }

    public float GetCurrentPhosphorus()
    {
        return CalculateCurrentPhosphorus();
    }

    private float CalculateCurrentPhosphorus()
    {
        float runoffFactor = temperature > 28.0f ? 0.03f : 0.01f;
        phosphorus += runoffFactor;

        float plantUptakeRate = algaePopulation / 10000.0f;
        phosphorus -= phosphorus * plantUptakeRate;

        return Mathf.Clamp(phosphorus, 0, maxPhosphorusLevel);
    }

    public float GetForecastedNitrate(float waterChangeAmount)
    {
        return nitrate - (nitrate * waterChangeAmount);
    }

    public float GetForecastedPotassium(float waterChangeAmount)
    {
        return potassium - (potassium * waterChangeAmount);
    }

    public float GetForecastedPhosphorus(float waterChangeAmount)
    {
        return phosphorus - (phosphorus * waterChangeAmount);
    }

    public float GetCurrentpH()
    {
        return pH;
    }

    public float GetForecastedpH(float waterChangeAmount)
    {
        return pH;
    }

    public float GetCurrentAmmonia()
    {
        return ammoniaLevel;
    }

    public float GetForecastedAmmonia(float waterChangeAmount)
    {
        return ammoniaLevel - (ammoniaLevel * waterChangeAmount);
    }

    public float GetCurrentOxygen()
    {
        return oxygenLevel;
    }

    public float GetForecastedOxygen(float waterChangeAmount)
    {
        return oxygenLevel;
    }

    public float GetCurrentNitrite()
    {
        return nitrite;
    }

    public float GetForecastedNitrite(float waterChangeAmount)
    {
        return nitrite - (nitrite * waterChangeAmount);
    }

    public void ReduceNutrientLevels(float amount, float deltaTime)
    {
        // Calculate nutrient decay rates based on environmental factors (temperature, pH, etc.)
        float temperatureFactor = CalculateTemperatureFactor();
        float pHFactor = CalculatepHFactor();

        // Calculate the decay rate for nitrate (you can similarly calculate for other nutrients)
        float nitrateDecayRate = CalculateDecayRate(temperatureFactor, pHFactor);

        // Reduce nitrate level based on the decay rate
        nitrate -= amount * deltaTime * nitrateDecayRate;
        nitrate = Mathf.Clamp(nitrate, 0, maxNitrateLevel);

        // Similarly, calculate decay rates and update levels for nitrite, phosphorus, potassium, etc.
        float nitriteDecayRate = CalculateDecayRate(temperatureFactor, pHFactor);
        nitrite -= amount * deltaTime * nitriteDecayRate;
        nitrite = Mathf.Clamp(nitrite, 0, maxNitriteLevel);

        float phosphorusDecayRate = CalculateDecayRate(temperatureFactor, pHFactor);
        phosphorus -= amount * deltaTime * phosphorusDecayRate;
        phosphorus = Mathf.Clamp(phosphorus, 0, maxPhosphorusLevel);

        float potassiumDecayRate = CalculateDecayRate(temperatureFactor, pHFactor);
        potassium -= amount * deltaTime * potassiumDecayRate;
        potassium = Mathf.Clamp(potassium, 0, maxPotassiumLevel);

        // You can continue to add more nutrients and update their levels here
        // For example, if you have calcium as well:

        float calciumDecayRate = CalculateDecayRate(temperatureFactor, pHFactor);
        calcium -= amount * deltaTime * calciumDecayRate;
        calcium = Mathf.Clamp(calcium, 0, maxCalciumLevel);

        // You can continue to add more nutrients and update their levels as needed
    }

    private float CalculateTemperatureFactor()
    {
        // Simulated temperature range
        float minTemperature = 0.0f;
        float maxTemperature = 100.0f;

        // Calculate the temperature factor based on the current temperature
        float temperatureFactor = Mathf.Clamp01((temperature - minTemperature) / (maxTemperature - minTemperature));

        return temperatureFactor;
    }

    private float CalculatepHFactor()
    {
        // Simulated pH range
        float minpH = 0.0f;
        float maxpH = 14.0f;

        // Calculate the pH factor based on the current pH level
        float pHFactor = Mathf.Clamp01((pH - minpH) / (maxpH - minpH));

        return pHFactor;
    }

    private float CalculateDecayRate(float temperatureFactor, float pHFactor)
    {
        // Define the base decay rate
        float baseDecayRate = 0.01f;

        // Adjust the decay rate based on temperature and pH factors
        float adjustedDecayRate = baseDecayRate * temperatureFactor * pHFactor;

        // Ensure the adjusted decay rate is within a reasonable range
        adjustedDecayRate = Mathf.Clamp(adjustedDecayRate, 0.001f, 1.0f);

        return adjustedDecayRate;
    }

}
