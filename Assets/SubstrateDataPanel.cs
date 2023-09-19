using UnityEngine;
using TMPro;

public class SubstrateDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text pHEffectText;
    public TMP_Text cationExchangeCapacityText;
    public TMP_Text nutrientHoldingCapacityText;
    public TMP_Text particleSizeText;
    public TMP_Text colorText;
    public TMP_Text priceText;
    public TMP_Text descriptionText;
    public TMP_Text suitabilityForPlantsText;

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

    public void UpdateSubstrateData(Substrate substrate)
    {
        nameText.text = substrate.name;
        typeText.text = "Type: " + substrate.type;
        pHEffectText.text = "pH Effect: " + substrate.pH_effect.ToString();
        cationExchangeCapacityText.text = "Cation Exchange Capacity: " + substrate.cation_exchange_capacity.ToString();
        nutrientHoldingCapacityText.text = "Nutrient Holding Capacity: " + substrate.nutrient_holding_capacity.ToString();
        particleSizeText.text = "Particle Size (mm): " + (substrate.particle_size_mm.Length >= 2 ? substrate.particle_size_mm[0].ToString() + " - " + substrate.particle_size_mm[1].ToString() : "N/A");
        colorText.text = "Color: " + substrate.color;
        priceText.text = "$ " + substrate.price_usd.ToString();
        descriptionText.text = "Description: " + substrate.description;
        suitabilityForPlantsText.text = "Suitability for Plants: " + substrate.suitability_for_plants;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearSubstrateData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearSubstrateData()
    {
        nameText.text = "";
        typeText.text = "";
        pHEffectText.text = "";
        cationExchangeCapacityText.text = "";
        nutrientHoldingCapacityText.text = "";
        particleSizeText.text = "";
        colorText.text = "";
        priceText.text = "";
        descriptionText.text = "";
        suitabilityForPlantsText.text = "";
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
