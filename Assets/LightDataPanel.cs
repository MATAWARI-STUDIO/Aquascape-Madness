using UnityEngine;
using TMPro;

public class LightDataPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text intensityText;
    public TMP_Text temperatureText;
    public TMP_Text priceText;

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

    public void UpdateLightData(Lights[] lights)
    {
        if (lights != null && lights.Length > 0)
        {
            Lights firstLight = lights[0];

            Debug.Log("Updating LightDataPanel with: " + firstLight.name);

            if (nameText != null)
                nameText.text = firstLight.name;
            if (typeText != null)
                typeText.text = "Type: " + firstLight.type;
            if (intensityText != null)
                intensityText.text = "Intensity: " + firstLight.intensity + " lux";
            if (temperatureText != null)
                temperatureText.text = "Temperature: " + firstLight.color_temperature_kelvin + " K";
            if (priceText != null)
                priceText.text = "$ " + firstLight.price_usd.ToString();
        }
        else
        {
            ClearLightData();
            Debug.LogWarning("No lights to display in LightDataPanel.");
        }
    }



    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);

        if (!isActive)
        {
            ClearLightData();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    private void ClearLightData()
    {
        nameText.text = "";
        typeText.text = "";
        intensityText.text = "";
        temperatureText.text = "";
        priceText.text = "";
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
