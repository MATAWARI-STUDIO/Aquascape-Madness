using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using System.Collections.Generic;

public class EcosystemManager : MonoBehaviour
{
    public float timeStep = 1.0f;
    public float maxBacteriaPopulationThreshold = 5000.0f;

    public WaterBody waterBody;
    public WaterQualityParameters waterQuality;
    public PlantBehaviorManager plantBehavior;
    public List<Substrate> substrates;
    public PredatorPreyManager predatorPreyManager;
    public LightIntensityManager lightIntensityManager;
    public TemperatureManager temperatureManager;
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public AudioSource warningAudioSource;
    public AudioClip warningSound;
    public Button closeButton;
    public JSONLoader jsonLoader;

    public float maxBacteriaDensityPerArea = 1000.0f;

    private float warningCooldown = 60.0f; // Increased cooldown to 60 seconds
    private float lastWarningTime = -60.0f;

    private bool algaeWarningTriggered = false;
    private bool bacteriaWarningTriggered = false;

    private void Start()
    {
        InitializeComponents();
        warningPanel.SetActive(false);
        closeButton.onClick.AddListener(OnCloseButtonClick);

        temperatureManager.SetTemperature(25.0f);
        waterQuality.SetBacteriaPopulation(100.0f);
        waterQuality.SetAlgaePopulation(100.0f);
    }

    private void Update()
    {
        SimulateEcosystem();
        SimulateBacterialEvents();
    }

    private void TriggerEvent(string message)
    {
        if (Time.time - lastWarningTime < warningCooldown)
            return;

        lastWarningTime = Time.time;

        warningText.text = message;
        warningPanel.SetActive(true);
        StartCoroutine(DeactivateWarningPanelAfterDelay(5.0f));

        warningAudioSource.clip = warningSound;
        warningAudioSource.Play();
    }

    private IEnumerator DeactivateWarningPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningPanel.SetActive(false);
    }

    private void InitializeComponents()
    {
        waterBody = FindObjectOfType<WaterBody>();
        waterQuality = GetComponent<WaterQualityParameters>();
        plantBehavior = FindObjectOfType<PlantBehaviorManager>();
        predatorPreyManager = FindObjectOfType<PredatorPreyManager>();
        lightIntensityManager = FindObjectOfType<LightIntensityManager>();
        temperatureManager = FindObjectOfType<TemperatureManager>();
        jsonLoader = FindObjectOfType<JSONLoader>();

        substrates = new List<Substrate>(jsonLoader.substrateData.substrates);
    }

    private void SimulateEcosystem()
    {
        UpdateWaterQuality();
        SimulateNutrientCycling();
        SimulatePlantBehavior();
        SimulatePredatorPreyInteractions();
        SimulateLightIntensity();
        SimulateTemperatureEffects();
        CheckGameEvents();
    }

    private void UpdateWaterQuality()
    {
        foreach (var substrate in substrates)
        {
            waterQuality.AdjustWaterQualityBasedOnSubstrate(substrate);
        }
        waterQuality.UpdateParameters(0.5f);
    }

    private void SimulateNutrientCycling()
    {
        foreach (var substrate in substrates)
        {
            waterQuality.AdjustNutrientLevelsBasedOnSubstrate(substrate);
        }
        waterQuality.UpdateNutrientLevels(0.5f);
    }

    private void SimulatePlantBehavior()
    {
        float currentTemperatureValue = GetCurrentTemperature();
        foreach (var substrate in substrates)
        {
            if (substrate.plant != null)
            {
                plantBehavior.SimulatePlantGrowth(substrate.plant);
                plantBehavior.SimulateNutrientUptake(substrate.plant);
                plantBehavior.SimulateLightSensitivity(substrate.plant);
            }
        }
    }

    public float GetCurrentTemperature()
    {
        return temperatureManager.GetTemperature();
    }

    private void SimulatePredatorPreyInteractions()
    {
        predatorPreyManager.SimulateInteractions();
    }

    private void SimulateLightIntensity()
    {
        foreach (Light light in lightIntensityManager.lightGameObjects)
        {
            lightIntensityManager.SimulateLightIntensity(light);
        }
    }

    private void SimulateTemperatureEffects()
    {
        temperatureManager.SimulateTemperatureEffects();
    }

    private void CheckGameEvents()
    {
        float algaePopulation = waterQuality.AlgaePopulation;
        if (algaePopulation >= 1000 && !algaeWarningTriggered)
        {
            TriggerEvent("Warning: Algae overgrowth detected! Consider reducing nutrient levels to control algae growth.");
            algaeWarningTriggered = true;
        }
        else if (algaePopulation < 1000)
        {
            algaeWarningTriggered = false;
        }

        float bacteriaPopulation = waterQuality.BacteriaPopulation;
        if (bacteriaPopulation >= maxBacteriaPopulationThreshold && !bacteriaWarningTriggered)
        {
            TriggerEvent("Warning: High bacterial population affecting other living things! Consider reducing waste levels to control bacterial growth.");
            bacteriaWarningTriggered = true;
        }
        else if (bacteriaPopulation < maxBacteriaPopulationThreshold)
        {
            bacteriaWarningTriggered = false;
        }

        float ammoniaLevel = waterQuality.GetAmmoniaLevel();
        if (ammoniaLevel >= waterQuality.maxAmmoniaLevel * 0.9f)
        {
            TriggerEvent("Warning: Ammonia pollution detected! Ensure proper filtration and consider water changes.");
        }

        float nitrateLevel = waterQuality.GetNitrateLevel();
        if (nitrateLevel >= waterQuality.maxNitrateLevel * 0.9f)
        {
            TriggerEvent("Warning: Severe Nitrate pollution detected! Consider water changes and reducing feedings.");
        }

        float nitriteLevel = waterQuality.GetNitriteLevel();
        if (nitriteLevel >= waterQuality.maxNitriteLevel * 0.8f)
        {
            TriggerEvent("Warning: Nitrite pollution detected! Check the nitrogen cycle and consider adding beneficial bacteria.");
        }

        float pHLevel = waterQuality.GetpH();
        if (pHLevel <= 6.0f || pHLevel >= 8.0f)
        {
            TriggerEvent("Warning: pH level is out of the optimal range! Consider using pH adjusters or natural methods like driftwood or crushed coral.");
        }
    }

    private void SimulateBacterialEvents()
    {
        float currentPopulation = Mathf.Clamp(waterQuality.BacteriaPopulation, 0f, maxBacteriaPopulationThreshold);
        waterQuality.bacteriaPopulation = currentPopulation;
    }

    private void OnCloseButtonClick()
    {
        warningPanel.SetActive(false);
    }
}
