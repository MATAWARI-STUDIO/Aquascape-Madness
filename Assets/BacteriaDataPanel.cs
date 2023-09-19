using UnityEngine;
using TMPro;

public class BacteriaDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text healthText;
    public TMP_Text pHText;
    public TMP_Text ammoniaToleranceText;
    public TMP_Text growthRateText;
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

    public void UpdateBacteriaData(Bacteria bacteria)
    {
        nameText.text = bacteria.name;
        descriptionText.text = bacteria.description;
        healthText.text = "Health: " + bacteria.health.ToString();
        pHText.text = "pH Tolerance: " + bacteria.pH_tolerance[0] + " - " + bacteria.pH_tolerance[1];
        ammoniaToleranceText.text = "Ammonia Tolerance (ppm): " + bacteria.ammonia_tolerance_ppm[0] + " - " + bacteria.ammonia_tolerance_ppm[1];
        growthRateText.text = "Growth Rate: " + bacteria.growth_rate_range[0] + " - " + bacteria.growth_rate_range[1];
        interactionWithFishText.text = "Interaction with Fish: pH - " + bacteria.interaction_with_fish.effectOnpH + ", Ammonia - " + bacteria.interaction_with_fish.effectOnAmmonia;
        interactionWithWaterText.text = "Interaction with Water: pH - " + bacteria.interaction_with_water.effectOnpH + ", Ammonia - " + bacteria.interaction_with_water.effectOnAmmonia;
    }


    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearBacteriaData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearBacteriaData()
    {
        nameText.text = "";
        descriptionText.text = "";
        healthText.text = "";
        pHText.text = "";
        ammoniaToleranceText.text = "";
        growthRateText.text = "";
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
