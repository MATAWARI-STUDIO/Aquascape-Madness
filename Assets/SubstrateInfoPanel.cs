using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubstrateInfoPanel : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text typeText;
    public TMP_Text suitabilityForPlantsText;
    public TMP_Text colorText;
    public Slider pHEffectSlider;
    public Slider cationExchangeSlider;
    public Slider nutrientHoldingSlider;
    public Slider particleSizeSlider;
    public TMP_Text priceText;
    public GameObject closeButton;

    private Substrate currentSubstrate;
    public JSONLoader jsonLoader;

    public void UpdateSubstrateInfo(Substrate substrate)
    {
        Debug.Log("UpdateSubstrateInfo started.");

        if (substrate == null)
        {
            Debug.LogError("Substrate object is null.");
            return;
        }

        Debug.Log("Substrate name: " + substrate.name);
        Debug.Log("Substrate type: " + substrate.type);

        nameLabel.text = substrate.name;
        typeText.text = "Type: " + substrate.type;
        suitabilityForPlantsText.text = "Suitability: " + substrate.suitability_for_plants;
        colorText.text = "Color: " + substrate.color;

        pHEffectSlider.value = substrate.pH_effect;
        cationExchangeSlider.value = substrate.cation_exchange_capacity;
        nutrientHoldingSlider.value = substrate.nutrient_holding_capacity;
        particleSizeSlider.minValue = substrate.particle_size_mm[0];
        particleSizeSlider.maxValue = substrate.particle_size_mm[1];
        particleSizeSlider.value = (substrate.particle_size_mm[0] + substrate.particle_size_mm[1]) / 2f;
        priceText.text = "Price: " + substrate.price_usd + " USD";

        currentSubstrate = substrate;

        Debug.Log("UpdateSubstrateInfo finished.");
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);

        if (!active)
        {
            ClearSubstrateData();
        }
    }

    public void ClearSubstrateData()
    {
        pHEffectSlider.value = 0;
        cationExchangeSlider.value = 0;
        nutrientHoldingSlider.value = 0;
        particleSizeSlider.value = 0;
        nameLabel.text = "";
        typeText.text = "";
        suitabilityForPlantsText.text = "";
        colorText.text = "";
        priceText.text = $"{0}";

    }

    public void OnCloseButtonClicked()
    {
        ClosePanel();
    }
}
