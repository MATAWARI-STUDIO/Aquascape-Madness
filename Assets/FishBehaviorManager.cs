using UnityEngine;
using System.Collections.Generic;

public class FishBehaviorManager : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;
    [SerializeField] private float herbivoreAmmoniaEffect = 0.05f;
    [SerializeField] private float herbivoreNitrateEffect = 0.1f;
    [SerializeField] private float predatorAmmoniaEffect = 0.1f;
    [SerializeField] private float predatorNitrateEffect = 0.05f;

    private List<FishBehavior> fishBehaviors;

    private void Start()
    {
        RefreshFishList();
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
        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters not set!");
            return;
        }

        float totalAmmoniaEffect = 0.0f;
        float totalNitrateEffect = 0.0f;

        foreach (FishBehavior fishBehavior in fishBehaviors)
        {
            if (fishBehavior.fish.isHerbivorous)
            {
                totalAmmoniaEffect += herbivoreAmmoniaEffect;
                totalNitrateEffect += herbivoreNitrateEffect;
            }
            else if (fishBehavior.fish.predatorFoodAmount > 0)
            {
                totalAmmoniaEffect += predatorAmmoniaEffect;
                totalNitrateEffect += predatorNitrateEffect;
            }
        }

        waterQualityParameters.AdjustAmmoniaLevel(-totalAmmoniaEffect);
        waterQualityParameters.AdjustNitrateLevel(-totalNitrateEffect);
    }


    public void RefreshFishList()
    {
        fishBehaviors = new List<FishBehavior>(FindObjectsOfType<FishBehavior>());
    }
}
