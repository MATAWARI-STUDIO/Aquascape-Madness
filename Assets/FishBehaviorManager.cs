using UnityEngine;
using System.Collections.Generic;

public class FishBehaviorManager : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;
    private List<FishBehavior> fishBehaviors;

    private void Start()
    {
        fishBehaviors = new List<FishBehavior>(FindObjectsOfType<FishBehavior>());
    }

    private void Update()
    {
        SimulateFishBehaviors();
        AdjustWaterQuality();
    }

    private void SimulateFishBehaviors()
    {
        foreach (FishBehavior fishBehavior in fishBehaviors)
        {
            fishBehavior.Grow();
            fishBehavior.Eat();
        }
    }

    private void AdjustWaterQuality()
    {
        float totalAmmoniaEffect = 0.0f;
        float totalNitrateEffect = 0.0f;

        foreach (FishBehavior fishBehavior in fishBehaviors)
        {
            if (fishBehavior.fish.isHerbivorous)
            {
                totalAmmoniaEffect += 0.05f; // Example value, adjust as needed
                totalNitrateEffect += 0.1f; // Example value, adjust as needed
            }
            else if (fishBehavior.fish.predatorFoodAmount > 0)
            {
                totalAmmoniaEffect += 0.1f; // Example value, adjust as needed
                totalNitrateEffect += 0.05f; // Example value, adjust as needed
            }
        }

        waterQualityParameters.AdjustAmmoniaLevel(-totalAmmoniaEffect);
        waterQualityParameters.AdjustNitrateLevel(-totalNitrateEffect);
    }
}
