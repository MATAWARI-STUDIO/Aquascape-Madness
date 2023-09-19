using UnityEngine;

public class WaterEcosystem : MonoBehaviour
{
    private float pH;
    private float ammoniaLevel;
    private float nitriteLevel;
    private float nitrateLevel;
    private float o2_production_mgphg;
    private float co2AbsorptionRate;
    private float carbonateHardness;
    private float generalHardness;

    public float lightEffectOnpH = 0.1f;
    public float lightEffectOnOxygen = 0.2f;
    public float lightEffectOnCO2Absorption = 0.1f;

    public float pHChangeOnEnter = -0.2f;
    public float ammoniaChangeOnEnter = 0.1f;
    public float nitriteChangeOnEnter = 0.05f;
    public float nitrateChangeOnEnter = 0.1f;
    public float oxygenChangeOnEnter = 1.0f;
    public float co2AbsorptionChangeOnEnter = -0.5f;
    public float carbonateHardnessChangeOnEnter = 1.0f;
    public float generalHardnessChangeOnEnter = 0.5f;

    public float pHChangeOnExit = 0.1f;
    public float ammoniaChangeOnExit = -0.05f;
    public float nitriteChangeOnExit = -0.02f;
    public float nitrateChangeOnExit = -0.05f;
    public float oxygenChangeOnExit = -0.5f;
    public float co2AbsorptionChangeOnExit = 0.2f;
    public float carbonateHardnessChangeOnExit = -0.5f;
    public float generalHardnessChangeOnExit = -0.2f;

    public WaterQualityManager waterQualityManager;

    private void Start()
    {
        pH = 7.0f;
        ammoniaLevel = 0.0f;
        nitriteLevel = 0.0f;
        nitrateLevel = 0.0f;
        o2_production_mgphg = 8.0f;
        co2AbsorptionRate = 0.1f;
        carbonateHardness = 5.0f;
        generalHardness = 10.0f;

        ApplyLightEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            pH += pHChangeOnEnter;
            ammoniaLevel += ammoniaChangeOnEnter;
            nitriteLevel += nitriteChangeOnEnter;
            nitrateLevel += nitrateChangeOnEnter;
            o2_production_mgphg += oxygenChangeOnEnter;
            co2AbsorptionRate += co2AbsorptionChangeOnEnter;
            carbonateHardness += carbonateHardnessChangeOnEnter;
            generalHardness += generalHardnessChangeOnEnter;
        }
        else if (other.CompareTag("Plant"))
        {
            pH += pHChangeOnEnter;
            ammoniaLevel -= ammoniaChangeOnEnter;
            nitriteLevel -= nitriteChangeOnEnter;
            nitrateLevel += nitrateChangeOnEnter;
            o2_production_mgphg += oxygenChangeOnEnter;
            co2AbsorptionRate += co2AbsorptionChangeOnEnter;
            carbonateHardness += carbonateHardnessChangeOnEnter;
            generalHardness += generalHardnessChangeOnEnter;
        }

        ApplyLightEffect();
        ClampWaterQualityParameters();

        waterQualityManager.UpdateWaterStatsUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            pH += pHChangeOnExit;
            ammoniaLevel += ammoniaChangeOnExit;
            nitriteLevel += nitriteChangeOnExit;
            nitrateLevel += nitrateChangeOnExit;
            o2_production_mgphg += oxygenChangeOnExit;
            co2AbsorptionRate += co2AbsorptionChangeOnExit;
            carbonateHardness += carbonateHardnessChangeOnExit;
            generalHardness += generalHardnessChangeOnExit;
        }
        else if (other.CompareTag("Plant"))
        {
            pH += pHChangeOnExit;
            ammoniaLevel -= ammoniaChangeOnExit;
            nitriteLevel -= nitriteChangeOnExit;
            nitrateLevel += nitrateChangeOnExit;
            o2_production_mgphg += oxygenChangeOnExit;
            co2AbsorptionRate += co2AbsorptionChangeOnExit;
            carbonateHardness += carbonateHardnessChangeOnExit;
            generalHardness += generalHardnessChangeOnExit;
        }

        ApplyLightEffect();
        ClampWaterQualityParameters();

        waterQualityManager.UpdateWaterStatsUI();
    }

    private void ApplyLightEffect()
    {
        pH += lightEffectOnpH;
        o2_production_mgphg += lightEffectOnOxygen;
        co2AbsorptionRate += lightEffectOnCO2Absorption;
        ClampWaterQualityParameters();
    }

    private void ClampWaterQualityParameters()
    {
        pH = Mathf.Clamp(pH, 6.0f, 9.0f);
        ammoniaLevel = Mathf.Clamp(ammoniaLevel, 0.0f, 2.0f);
        nitriteLevel = Mathf.Clamp(nitriteLevel, 0.0f, 2.0f);
        nitrateLevel = Mathf.Clamp(nitrateLevel, 0.0f, 2.0f);
        o2_production_mgphg = Mathf.Clamp(o2_production_mgphg, 0.0f, 12.0f);
        co2AbsorptionRate = Mathf.Clamp(co2AbsorptionRate, 0.0f, 2.0f);
        carbonateHardness = Mathf.Clamp(carbonateHardness, 0.0f, 20.0f);
        generalHardness = Mathf.Clamp(generalHardness, 0.0f, 30.0f);
    }
}
