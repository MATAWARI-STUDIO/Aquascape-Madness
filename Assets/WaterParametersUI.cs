using UnityEngine;
using TMPro;

public class WaterParametersUI : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;

    public TMP_Text nitrateText;
    public TMP_Text potassiumText;
    public TMP_Text phosphorusText;
    public TMP_Text temperatureText;
    public TMP_Text pHText;
    public TMP_Text wasteLevelText;
    public TMP_Text nutrientLevelText;
    public TMP_Text algaePopulationText;
    public TMP_Text ammoniaLevelText;
    public TMP_Text oxygenLevelText;
    public TMP_Text nitriteText;
    public TMP_Text toxinLevelText;

    private void Update()
    {
        // Check if the waterQualityParameters reference is set.
        if (waterQualityParameters != null)
        {
            // Update the TextMeshPro Text components with the corresponding parameter values, rounding to two decimal places.
            nitrateText.text = $"Nitrate: {waterQualityParameters.GetCurrentNitrate():F2}";
            potassiumText.text = $"Potassium: {waterQualityParameters.GetCurrentPotassium():F2}";
            phosphorusText.text = $"Phosphorus: {waterQualityParameters.GetCurrentPhosphorus():F2}";
            temperatureText.text = $"Temperature: {waterQualityParameters.GetTemperature():F2}";
            pHText.text = $"pH: {waterQualityParameters.GetpH():F2}";
            wasteLevelText.text = $"Waste Level: {waterQualityParameters.GetWasteLevel():F2}";
            nutrientLevelText.text = $"Nutrient Level: {waterQualityParameters.GetNutrientLevel():F2}";
            algaePopulationText.text = $"Algae Population: {waterQualityParameters.GetAlgaePopulation():F2}";
            ammoniaLevelText.text = $"Ammonia Level: {waterQualityParameters.GetAmmoniaLevel():F2}";
            oxygenLevelText.text = $"Oxygen Level: {waterQualityParameters.GetOxygenLevel():F2}";
            nitriteText.text = $"Nitrite: {waterQualityParameters.GetNitriteLevel():F2}";
            toxinLevelText.text = $"Toxin Level: {waterQualityParameters.GetToxinLevel():F2}";
        }
        else
        {
            Debug.LogError("WaterQualityParameters reference is not set. Please assign it in the Inspector.");
        }
    }
}
