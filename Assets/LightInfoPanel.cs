using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;


public class LightInfoPanel : MonoBehaviour
{


    [Header("Dependencies")]
    [SerializeField]
    private Light lightGameObject;
    [SerializeField]
    private JSONLoader jsonLoader;
    [SerializeField]
    private AudioClip clickSound;
    private AudioSource audioSource;
    private JSONLoader.Lights currentLights;
    private float originalIntensity;
    private bool isLightOn = true;

    [Header("UI Components")]
    [SerializeField]
    private TMP_Text autoOnHour, autoOnMinute, autoOffHour, autoOffMinute;
    [SerializeField]
    private Button autoOnLeftArrow, autoOnRightArrow, autoOnUpArrow, autoOnDownArrow;
    [SerializeField]
    private Button autoOffLeftArrow, autoOffRightArrow, autoOffUpArrow, autoOffDownArrow;
    [SerializeField]
    private Slider intensitySlider;
    [SerializeField]
    private TextMeshProUGUI intensityText;
    [SerializeField]
    private Button intensityLeftButton, intensityRightButton;
    [SerializeField]
    private Slider temperatureSlider;
    [SerializeField]
    private TextMeshProUGUI temperatureText;
    [SerializeField]
    private Button temperatureLeftButton, temperatureRightButton;
    [SerializeField]
    private Button powerButton;
    private float currentTime = 0.0f; // Declare currentTime
    private DateTime lastCheckedTime = DateTime.MinValue;

    // Intensity and Temperature
    private const float INTENSITY_TEMPERATURE_CLICK_ADJUSTMENT = 100f;
    private const float FAST_INTENSITY_TEMPERATURE_ADJUSTMENT_SPEED = 500f; // Adjust as needed

    // Auto On/Off
    private const int AUTO_ON_OFF_CLICK_ADJUSTMENT = 1;
    private const float FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED = 5f; // Adjust as needed


    private bool isAdjusting = false;
    private TMP_Text selectedAutoOnField, selectedAutoOffField;
    private Coroutine currentBlinkingCoroutine;
    public TimeController timeController;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;

        if (lightGameObject == null)
        {
            Debug.LogError("Light GameObject is not assigned!");
        }
        else
        {
            originalIntensity = lightGameObject.intensity;
        }
    }

    private void Start()
    {
        SetUpListeners();
        selectedAutoOnField = autoOnHour;
        selectedAutoOffField = autoOffHour;
        StartBlinking(selectedAutoOnField);
        StartBlinking(selectedAutoOffField);
        LoadSettings();

        // Ensure the light is ON at the start of play
        if (lightGameObject != null)
        {
            lightGameObject.enabled = true;
        }

        // Assign the reference to the TimeController script
        timeController = TimeController.Instance;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSettings();
            gameObject.SetActive(false);
        }

        // Check the current time and toggle the light based on Auto On/Off times
        CheckAutoOnOffTimes();
    }


    private void CheckAutoOnOffTimes()
    {
        // Get the current system time
        DateTime currentTime = DateTime.Now;

        // Check if a new day has started since the last check
        if (lastCheckedTime.Date != currentTime.Date)
        {
            lastCheckedTime = currentTime; // Update the last checked time

            // Parse the Auto On and Off times from your UI components
            int autoOnHourValue = int.Parse(autoOnHour.text);
            int autoOnMinuteValue = int.Parse(autoOnMinute.text);
            int autoOffHourValue = int.Parse(autoOffHour.text);
            int autoOffMinuteValue = int.Parse(autoOffMinute.text);

            // Calculate the Auto On and Off times for today
            DateTime autoOnTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, autoOnHourValue, autoOnMinuteValue, 0);
            DateTime autoOffTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, autoOffHourValue, autoOffMinuteValue, 0);

            // Check if it's time to turn the light on or off
            if (currentTime >= autoOnTime && currentTime < autoOffTime)
            {
                // Turn the light on
                lightGameObject.enabled = true;
            }
            else
            {
                // Turn the light off
                lightGameObject.enabled = false;
            }
        }
    }
    private bool IsTimeInRange(int currentHour, int currentMinute, int startHour, int startMinute, int endHour, int endMinute)
    {
        // Check if the current time is within the specified range
        if (currentHour > startHour || (currentHour == startHour && currentMinute >= startMinute))
        {
            if (currentHour < endHour || (currentHour == endHour && currentMinute < endMinute))
            {
                return true;
            }
        }
        return false;
    }

    private void SetUpListeners()
    {
        powerButton.onClick.AddListener(() => { ToggleLightPower(); PlayClickSound(); });

        autoOnLeftArrow.onClick.AddListener(() => { PlayClickSound(); SwitchSelectedField(ref selectedAutoOnField, autoOnHour, autoOnMinute); });
        autoOnRightArrow.onClick.AddListener(() => { PlayClickSound(); SwitchSelectedField(ref selectedAutoOnField, autoOnHour, autoOnMinute); });
        autoOnUpArrow.onClick.AddListener(() => { PlayClickSound(); AdjustTime(selectedAutoOnField, AUTO_ON_OFF_CLICK_ADJUSTMENT); });
        autoOnDownArrow.onClick.AddListener(() => { PlayClickSound(); AdjustTime(selectedAutoOnField, -AUTO_ON_OFF_CLICK_ADJUSTMENT); });

        autoOffLeftArrow.onClick.AddListener(() => { PlayClickSound(); SwitchSelectedField(ref selectedAutoOffField, autoOffHour, autoOffMinute); });
        autoOffRightArrow.onClick.AddListener(() => { PlayClickSound(); SwitchSelectedField(ref selectedAutoOffField, autoOffHour, autoOffMinute); });
        autoOffUpArrow.onClick.AddListener(() => { PlayClickSound(); AdjustTime(selectedAutoOffField, AUTO_ON_OFF_CLICK_ADJUSTMENT); });
        autoOffDownArrow.onClick.AddListener(() => { PlayClickSound(); AdjustTime(selectedAutoOffField, -AUTO_ON_OFF_CLICK_ADJUSTMENT); });

        intensitySlider.onValueChanged.AddListener(UpdateIntensityText);
        temperatureSlider.onValueChanged.AddListener(UpdateTemperatureText);

        AddPointerUpListener(intensityLeftButton, StopAdjustingSlider);
        AddPointerUpListener(intensityRightButton, StopAdjustingSlider);
        AddPointerUpListener(temperatureLeftButton, StopAdjustingSlider);
        AddPointerUpListener(temperatureRightButton, StopAdjustingSlider);

        // Intensity
        intensityLeftButton.onClick.AddListener(() => { PlayClickSound(); AdjustSliderByClick(intensitySlider, -INTENSITY_TEMPERATURE_CLICK_ADJUSTMENT); });
        intensityRightButton.onClick.AddListener(() => { PlayClickSound(); AdjustSliderByClick(intensitySlider, INTENSITY_TEMPERATURE_CLICK_ADJUSTMENT); });

        // Temperature
        temperatureLeftButton.onClick.AddListener(() => { PlayClickSound(); AdjustSliderByClick(temperatureSlider, -INTENSITY_TEMPERATURE_CLICK_ADJUSTMENT); });
        temperatureRightButton.onClick.AddListener(() => { PlayClickSound(); AdjustSliderByClick(temperatureSlider, INTENSITY_TEMPERATURE_CLICK_ADJUSTMENT); });

        AddPointerDownListener(intensityLeftButton, () => StartCoroutine(ContinuousAdjustSlider(intensitySlider, -FAST_INTENSITY_TEMPERATURE_ADJUSTMENT_SPEED)));
        AddPointerDownListener(intensityRightButton, () => StartCoroutine(ContinuousAdjustSlider(intensitySlider, FAST_INTENSITY_TEMPERATURE_ADJUSTMENT_SPEED)));

        AddPointerDownListener(temperatureLeftButton, () => StartCoroutine(ContinuousAdjustSlider(temperatureSlider, -FAST_INTENSITY_TEMPERATURE_ADJUSTMENT_SPEED)));
        AddPointerDownListener(temperatureRightButton, () => StartCoroutine(ContinuousAdjustSlider(temperatureSlider, FAST_INTENSITY_TEMPERATURE_ADJUSTMENT_SPEED)));

        AddPointerDownListener(autoOnUpArrow, () => StartCoroutine(ContinuousAdjustTime(selectedAutoOnField, FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED)));
        AddPointerDownListener(autoOnDownArrow, () => StartCoroutine(ContinuousAdjustTime(selectedAutoOnField, -FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED)));
        AddPointerDownListener(autoOffUpArrow, () => StartCoroutine(ContinuousAdjustTime(selectedAutoOffField, FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED)));
        AddPointerDownListener(autoOffDownArrow, () => StartCoroutine(ContinuousAdjustTime(selectedAutoOffField, -FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED)));

        AddPointerUpListener(autoOnUpArrow, StopAllCoroutines);
        AddPointerUpListener(autoOnDownArrow, StopAllCoroutines);
        AddPointerUpListener(autoOffUpArrow, StopAllCoroutines);
        AddPointerUpListener(autoOffDownArrow, StopAllCoroutines);

        intensitySlider.value = intensitySlider.maxValue;
        temperatureSlider.value = temperatureSlider.maxValue;
    }

    public Light LightGameObject
    {
        get { return lightGameObject; }
        set { lightGameObject = value; }
    }

    public Slider IntensitySlider
    {
        get { return intensitySlider; }
        set { intensitySlider = value; }
    }

    public Slider TemperatureSlider
    {
        get { return temperatureSlider; }
        set { temperatureSlider = value; }
    }


 

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("Intensity", intensitySlider.value);
        PlayerPrefs.SetFloat("Temperature", temperatureSlider.value);
        PlayerPrefs.SetInt("AutoOnHour", int.Parse(autoOnHour.text));
        PlayerPrefs.SetInt("AutoOnMinute", int.Parse(autoOnMinute.text));
        PlayerPrefs.SetInt("AutoOffHour", int.Parse(autoOffHour.text));
        PlayerPrefs.SetInt("AutoOffMinute", int.Parse(autoOffMinute.text));
        PlayerPrefs.SetInt("IsLightOn", lightGameObject.intensity > 0 ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void PlayClickSound()
    {
        audioSource.Play();
    }

    private void AddPointerDownListener(Button button, UnityEngine.Events.UnityAction callback)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => callback());
        trigger.triggers.Add(entry);
    }

    private void AddPointerUpListener(Button button, UnityEngine.Events.UnityAction callback)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => callback());
        trigger.triggers.Add(entry);
    }

    private IEnumerator ContinuousSwitchField(TMP_Text currentField, TMP_Text hourField, TMP_Text minuteField)
    {
        while (true)
        {
            SwitchSelectedField(ref currentField, hourField, minuteField);
            yield return new WaitForSeconds(FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED);
        }
    }

    private IEnumerator ContinuousAdjustTime(TMP_Text field, int adjustment)
    {
        while (true)
        {
            AdjustTime(field, adjustment);
            yield return new WaitForSeconds(FAST_AUTO_ON_OFF_ADJUSTMENT_SPEED);
        }
    }


    private void AdjustSlider(Slider slider, float adjustment)
    {
        float newValue = slider.value + adjustment;
        if (newValue > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else if (newValue < slider.minValue)
        {
            slider.value = slider.minValue;
        }
        else
        {
            slider.value = newValue;
        }
    }

    private IEnumerator ContinuousAdjustSlider(Slider slider, float adjustmentSpeed)
    {
        while (isAdjusting)
        {
            AdjustSlider(slider, adjustmentSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator ContinuousAdjustTime(TMP_Text field, float adjustmentSpeed)
    {
        while (isAdjusting)
        {
            AdjustTime(field, (int)(adjustmentSpeed * Time.deltaTime));
            yield return null; // Wait for the next frame
        }
    }

    private void StartAdjustingSlider(Slider slider, int direction)
    {
        isAdjusting = true;
    }

    private void StopAdjustingSlider()
    {
        isAdjusting = false;
    }


    private void AdjustTime(TMP_Text field, int adjustment)
    {
        int value = int.Parse(field.text);
        if (field == autoOnHour || field == autoOffHour)
        {
            value = Mathf.Clamp(value + adjustment, 0, 23);
        }
        else
        {
            value += adjustment;
            if (value >= 60)
            {
                value -= 60;
            }
            else if (value < 0)
            {
                value += 60;
            }
        }
        field.text = value.ToString("00");
    }

    private void SwitchSelectedField(ref TMP_Text currentField, TMP_Text hourField, TMP_Text minuteField)
    {
        TMP_Text previouslySelected = currentField;
        currentField = currentField == hourField ? minuteField : hourField;
        StartBlinking(currentField);

        // Update the colors
        currentField.color = Color.white;
        previouslySelected.color = Color.gray;
    }

    private void AdjustSliderByClick(Slider slider, float adjustment)
    {
        float newValue = slider.value + adjustment;
        if (newValue > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else if (newValue < slider.minValue)
        {
            slider.value = slider.minValue;
        }
        else
        {
            slider.value = newValue;
        }
    }

    private IEnumerator BlinkText(TMP_Text field)
    {
        while (true)
        {
            field.color = new Color(1f, 1f, 1f, 0.5f); // Half transparent but still white
            yield return new WaitForSeconds(0.5f);
            field.color = new Color(1f, 1f, 1f, 1f); // Fully opaque white
            yield return new WaitForSeconds(0.5f);
        }
    }


    private void StartBlinking(TMP_Text field)
    {
        // Check if the field is already blinking, if it is, do not start another coroutine
        if (field.alpha < 1.0f) return; // Assuming that when blinking it goes below 1.0f

        if (currentBlinkingCoroutine != null)
        {
            StopCoroutine(currentBlinkingCoroutine);
        }
        currentBlinkingCoroutine = StartCoroutine(BlinkText(field));
    }

    private void StopBlinking(TMP_Text field)
    {
        if (currentBlinkingCoroutine != null)
        {
            StopCoroutine(currentBlinkingCoroutine);
            field.color = new Color(1f, 1f, 1f, 1f);  // Ensure the color is reset to fully opaque white
        }
    }

    private void ToggleLightPower()
    {
        if (lightGameObject == null)
        {
            Debug.LogError("Light GameObject is not assigned!");
            return;
        }

        // Toggle the state
        isLightOn = !isLightOn;

        if (isLightOn)
        {
            lightGameObject.enabled = true; // Activate the light component
            Debug.Log("Light is now: ON");
        }
        else
        {
            lightGameObject.enabled = false; // Deactivate the light component
            Debug.Log("Light is now: OFF");
        }
    }

    private void AdjustSlider(Slider slider, int adjustment)
    {
        float newValue = slider.value + adjustment;
        if (newValue > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else if (newValue < slider.minValue)
        {
            slider.value = slider.minValue;
        }
        else
        {
            slider.value = newValue;
        }
    }

    private void UpdateIntensityText(float value)
    {
        intensityText.text = Mathf.RoundToInt(value).ToString();
        lightGameObject.intensity = value / intensitySlider.maxValue;
    }

    private void UpdateTemperatureText(float value)
    {
        temperatureText.text = Mathf.RoundToInt(value).ToString();
        lightGameObject.color = KelvinToRGB(value);
    }

    private Color KelvinToRGB(float kelvin)
    {
        float temp = kelvin / 100;
        float red, green, blue;

        if (temp <= 66)
        {
            red = 255;
            green = temp;
            green = 99.4708025861f * Mathf.Log(green) - 161.1195681661f;
            if (temp <= 19)
            {
                blue = 0;
            }
            else
            {
                blue = temp - 10;
                blue = 138.5177312231f * Mathf.Log(blue) - 305.0447927307f;
            }
        }
        else
        {
            red = temp - 60;
            red = 329.698727446f * Mathf.Pow(red, -0.1332047592f);
            green = temp - 60;
            green = 288.1221695283f * Mathf.Pow(green, -0.0755148492f);
            blue = 255;
        }

        return new Color(red / 255.0f, green / 255.0f, blue / 255.0f);
    }

    public void UpdateLightInfo(JSONLoader.Lights lightSetting)
    {
        if (lightSetting != null && lightGameObject != null)
        {
            intensitySlider.maxValue = lightSetting.light_intensity_lux;
            temperatureSlider.maxValue = lightSetting.color_temperature_kelvin;

            intensitySlider.value = intensitySlider.maxValue;
            temperatureSlider.value = temperatureSlider.maxValue;

            UpdateIntensityText(intensitySlider.value);
            UpdateTemperatureText(temperatureSlider.value);

            ToggleLight(lightSetting.isOn);
            currentLights = lightSetting;
        }
        else
        {
            Debug.LogError("Lights or lightGameObject is null in UpdateLightInfo.");
        }
    }

    private void ToggleLight(bool isOn)
    {
        if (lightGameObject != null)
        {
            if (isOn)
            {
                lightGameObject.intensity = originalIntensity;
            }
            else
            {
                lightGameObject.intensity = 0;
            }
        }
        else
        {
            Debug.LogError("lightGameObject is null in ToggleLight.");
        }
    }


    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("IsLightOn"))
        {
            isLightOn = PlayerPrefs.GetInt("IsLightOn") == 1;
        }
        else
        {
            // Default to On if the setting doesn't exist
            isLightOn = true;
        }

        // Set the light intensity based on the loaded state
        if (isLightOn)
        {
            lightGameObject.intensity = originalIntensity;
        }
        else
        {
            lightGameObject.intensity = 0;
        }

        // Now, you can toggle the light based on the loaded state
        ToggleLight(isLightOn);

        if (PlayerPrefs.HasKey("Intensity"))
        {
            intensitySlider.value = PlayerPrefs.GetFloat("Intensity");
            UpdateIntensityText(intensitySlider.value);
        }
        else
        {
            // Default intensity when starting play mode
            intensitySlider.value = intensitySlider.maxValue;
            UpdateIntensityText(intensitySlider.value);
            lightGameObject.intensity = originalIntensity; // This line ensures the light starts with its original intensity.
        }

        if (PlayerPrefs.HasKey("Temperature"))
        {
            temperatureSlider.value = PlayerPrefs.GetFloat("Temperature");
            UpdateTemperatureText(temperatureSlider.value);
        }
        else
        {
            temperatureSlider.value = temperatureSlider.maxValue;
        }

        if (PlayerPrefs.HasKey("AutoOnHour"))
        {
            autoOnHour.text = PlayerPrefs.GetInt("AutoOnHour").ToString("00");
        }

        if (PlayerPrefs.HasKey("AutoOnMinute"))
        {
            autoOnMinute.text = PlayerPrefs.GetInt("AutoOnMinute").ToString("00");
        }

        if (PlayerPrefs.HasKey("AutoOffHour"))
        {
            autoOffHour.text = PlayerPrefs.GetInt("AutoOffHour").ToString("00");
        }

        if (PlayerPrefs.HasKey("AutoOffMinute"))
        {
            autoOffMinute.text = PlayerPrefs.GetInt("AutoOffMinute").ToString("00");
        }

        lightGameObject.intensity = originalIntensity;
    }

}
