using UnityEngine;
using TMPro;

public class AlgaeDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text healthText;
    public TMP_Text pHText;
    public TMP_Text nitrateToleranceText;
    public TMP_Text growthRateText;
    public TMP_Text oxygenProductionText;
    public TMP_Text interactionWithFishText;
    public TMP_Text interactionWithWaterText;

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

    public void UpdateAlgaeData(Algae algae)
    {
        nameText.text = algae.name;
        descriptionText.text = algae.description;
        healthText.text = "Health: " + algae.health.ToString();
        pHText.text = "pH Tolerance: " + algae.pH_tolerance[0] + " - " + algae.pH_tolerance[1];
        nitrateToleranceText.text = "Nitrate Tolerance (ppm): " + algae.nitrate_tolerance_ppm[0] + " - " + algae.nitrate_tolerance_ppm[1];
        growthRateText.text = "Growth Rate: " + algae.growth_rate_range[0] + " - " + algae.growth_rate_range[1];
        oxygenProductionText.text = "Oxygen Production Rate: " + algae.oxygen_production_rate.ToString();
        interactionWithFishText.text = "Interaction with Fish: pH - " + algae.interaction_with_fish.effectOnpH + ", Nitrate - " + algae.interaction_with_fish.effectOnNitrate + ", Oxygen - " + algae.interaction_with_fish.effectOnOxygenProduction;
        interactionWithWaterText.text = "Interaction with Water: pH - " + algae.interaction_with_water.effectOnpH + ", Nitrate - " + algae.interaction_with_water.effectOnNitrate + ", Oxygen - " + algae.interaction_with_water.effectOnOxygenProduction;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearAlgaeData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearAlgaeData()
    {
        nameText.text = "";
        descriptionText.text = "";
        healthText.text = "";
        pHText.text = "";
        nitrateToleranceText.text = "";
        growthRateText.text = "";
        oxygenProductionText.text = "";
        interactionWithFishText.text = "";
        interactionWithWaterText.text = "";
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
