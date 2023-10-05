using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private float simulationTime;
    private float timeStep;
    private bool isSimulationRunning;
    private float currentTimeOfLightCycle;

    public WaterQualityParameters waterQualityParameters;
    public FishBehavior[] fishBehaviors;
    public PlantBehavior[] plantBehaviors;
    public AlgaeBehavior[] algaeBehaviors;
    public BacteriaBehavior[] bacteriaBehaviors;
    public LightIntensityManager lightIntensityManager;

    void Start()
    {
        simulationTime = 0.0f;
        timeStep = 0.1f;
        isSimulationRunning = false;
        currentTimeOfLightCycle = 0.0f;
    }

    void Update()
    {
        if (isSimulationRunning)
        {
            simulationTime += Time.deltaTime;
            currentTimeOfLightCycle += Time.deltaTime / 86400.0f;

            waterQualityParameters.UpdateWaterParameters(); // Use the correct method to update water parameters
            waterQualityParameters.UpdateNutrientLevels(Time.deltaTime);

            foreach (var fishBehavior in fishBehaviors)
            {
                float pHValue = waterQualityParameters.GetpH();
                float ammoniaValue = waterQualityParameters.GetAmmoniaLevel();
                float nitriteValue = waterQualityParameters.GetNitriteLevel();
                float nitrateValue = waterQualityParameters.GetNitrateLevel();
                float o2ProductionRate = waterQualityParameters.GetCurrentOxygen();
                float currentTemperature = waterQualityParameters.GetTemperature();

                // Assuming each fish has its own FishData object, retrieve it here.
                FishData fishData = fishBehavior.fishData;

                // Pass the arguments correctly to ApplyWaterEffects for each fish
                fishBehavior.ApplyWaterEffects(pHValue, ammoniaValue, nitriteValue, nitrateValue, o2ProductionRate, currentTemperature);

            }

            foreach (var plantBehavior in plantBehaviors)
            {
                float consumedLight = CalculateConsumedLight(plantBehavior);
                float consumedNutrient = CalculateConsumedNutrient(plantBehavior);
                plantBehavior.UpdatePlantBehavior(consumedLight, consumedNutrient);
                waterQualityParameters.AdjustNitrateLevel(-0.1f); // Plants consume nitrates directly
            }

            foreach (var algaeBehavior in algaeBehaviors)
            {
                algaeBehavior.UpdateAlgae();
            }

            foreach (var bacteriaBehavior in bacteriaBehaviors)
            {
                bacteriaBehavior.UpdateBacteria();
            }

            HandleGameEvents();
        }
    }


    public void StartSimulation()
    {
        isSimulationRunning = true;
    }

    public void PauseSimulation()
    {
        isSimulationRunning = false;
    }

    private float CalculateConsumedLight(PlantBehavior plantBehavior)
    {
        // Using the light consumption rate from the LightIntensityManager's current light intensity.
        return lightIntensityManager.currentLightIntensity * Time.deltaTime;
    }

    private float CalculateConsumedNutrient(PlantBehavior plantBehavior)
    {
        return plantBehavior.nutrientConsumptionRate * Time.deltaTime; // Assuming plants have a nutrientConsumptionRate property.
    }

    private void HandleGameEvents()
    {
        // Placeholder for any game events you might want to handle.
    }
}
