using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FishInfoPanel : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text typeText;
    public Slider pHSlider;
    public Slider ammoniaSlider;
    public Slider nitriteSlider;
    public Slider nitrateSlider;
    public Slider co2AbsorptionEffectSlider;
    public Slider healthSlider;
    public Slider stressSlider;
    public GameObject closeButton;

    public Color healthyColor = Color.green;
    public Color stressedColor = Color.red;

    private bool isActive;
    private bool isMouseOver;

    [SerializeField] private JSONLoader jsonLoader;

    private void Start()
    {
        closeButton.GetComponent<Button>().onClick.AddListener(ClosePanel);
        jsonLoader = FindObjectOfType<JSONLoader>();
    }

    public void SetJsonLoader(JSONLoader loader)
    {
        jsonLoader = loader;
    }

    public void UpdateHealthSlider(Fish fish)
    {
        healthSlider.value = fish.health / 100f;
        Image healthFillRect = healthSlider.fillRect.GetComponent<Image>();
        if (healthFillRect != null)
        {
            healthFillRect.color = fish.health >= 70 ? healthyColor : stressedColor;
        }
    }

    public void UpdateStressSlider(Fish fish)
    {
        stressSlider.value = fish.stress / 100f;
        Image stressFillRect = stressSlider.fillRect.GetComponent<Image>();
        if (stressFillRect != null)
        {
            stressFillRect.color = fish.stress <= 30 ? healthyColor : stressedColor;
        }
    }

    public void ActivatePanel(Fish fish)
    {
        SetJsonLoader(jsonLoader);
        UpdateFishInfo(fish);
        SetActive(true);
    }


    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearFishData();
        }
    }

    public void UpdateFishInfo(Fish fish)
    {
        if (fish != null)
        {
            nameLabel.text = fish.name;
            typeText.text = "Type: " + fish.type;
            pHSlider.value = fish.interaction_with_water.effectOnpH;
            ammoniaSlider.value = fish.interaction_with_water.effectOnAmmonia;
            nitriteSlider.value = fish.interaction_with_water.effectOnNitrite;
            nitrateSlider.value = fish.interaction_with_water.effectOnNitrate;
            co2AbsorptionEffectSlider.value = fish.interaction_with_water.effectOnCO2Production;

            UpdateHealthSlider(fish);
            UpdateStressSlider(fish);

            Debug.Log("pH Slider Value: " + pHSlider.value);
            Debug.Log("Ammonia Slider Value: " + ammoniaSlider.value);
            Debug.Log("Nitrite Slider Value: " + nitriteSlider.value);
            Debug.Log("Nitrate Slider Value: " + nitrateSlider.value);
            Debug.Log("CO2 Absorption Slider Value: " + co2AbsorptionEffectSlider.value);
        }
        else
        {
            Debug.LogError("Fish object is null in UpdateFishInfo.");
        }
    }

    private void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            SetActive(!isActive);
        }
    }

    private void ClearFishData()
    {
        nameLabel.text = "";
        typeText.text = "";
        pHSlider.value = 0;
        ammoniaSlider.value = 0;
        nitriteSlider.value = 0;
        nitrateSlider.value = 0;
        co2AbsorptionEffectSlider.value = 0;
        healthSlider.value = 0;
        stressSlider.value = 0;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
