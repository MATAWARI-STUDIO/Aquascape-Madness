using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FilterInfoPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text descriptionText;
    public TMP_Text effectivenessText;
    public TMP_Text filterMediaText;
    public TMP_Text filterCapacityText;
    public TMP_Text pHChangeRateText;
    public TMP_Text ammoniaChangeRateText;
    public TMP_Text nitriteChangeRateText;
    public TMP_Text nitrateChangeRateText;
    public TMP_Text oxygenChangeRateText;
    public TMP_Text priceText;
    public TMP_Text powerDrawText;
    public TMP_Text flowRateText;
    public TMP_Text airflowRateText;

    public JSONLoader jsonLoader;
    public Button powerButton;
    public Button serviceFilterButton;
    public Button nozzleLeftButton;
    public Button nozzleRightButton;
    public Button nozzleTiltLeftButton;
    public Button nozzleTiltRightButton;
    public Button venturiLeftButton;
    public Button venturiRightButton;
    public TMP_Text nozzleText;
    public TMP_Text nozzleTiltText;
    public TMP_Text venturiText;

    public WaterQualityParameters waterQualityParameters;

    private bool isActive;
    private bool isFilterOn = true;
    private string[] nozzleOptions = { "closed", "1/2", "open" };
    private string[] nozzleTiltOptions = { "downward", "straight", "upward" };
    private string[] venturiOptions = { "closed", "1/4", "1/2", "3/4", "open" };
    private int nozzleIndex = 1;
    private int nozzleTiltIndex = 1;
    private int venturiIndex = 2;
    private const float MAX_FLOW_RATE = 100.0f; // Example maximum flow rate in L/min
    private const float MAX_AIRFLOW_RATE = 50.0f; // Example maximum airflow rate in L/min

    public void SetJSONLoader(JSONLoader loader)
    {
        this.jsonLoader = loader;
    }

    private void Start()
    {
        powerButton.onClick.AddListener(ToggleFilterPower);
        serviceFilterButton.onClick.AddListener(ServiceFilter);
        nozzleLeftButton.onClick.AddListener(() => ChangeNozzleSetting(-1));
        nozzleRightButton.onClick.AddListener(() => ChangeNozzleSetting(1));
        nozzleTiltLeftButton.onClick.AddListener(() => ChangeNozzleTiltSetting(-1));
        nozzleTiltRightButton.onClick.AddListener(() => ChangeNozzleTiltSetting(1));
        venturiLeftButton.onClick.AddListener(() => ChangeVenturiSetting(-1));
        venturiRightButton.onClick.AddListener(() => ChangeVenturiSetting(1));
        SetActive(false);
        UpdateAmmoniaText();

        if (jsonLoader == null)
        {
            Debug.LogError("JSONLoader reference is not set in FilterInfoPanel.");
        }

        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters reference is not set in FilterInfoPanel.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActive(false);
        }
    }

    private void ToggleFilterPower()
    {
        isFilterOn = !isFilterOn;
        powerDrawText.text = isFilterOn ? "Power Draw: Normal" : "Power Draw: Increased";
    }

    private void ServiceFilter()
    {
        effectivenessText.text = "Effectiveness: 100%";
        powerDrawText.text = "Power Draw: Normal";
        filterMediaText.text = "Filter Media: Clean";
        filterCapacityText.text = "Filter Capacity: Full";
        pHChangeRateText.text = "pH Change Rate: Normal";
        ammoniaChangeRateText.text = "Ammonia Change Rate: Normal";
        nitriteChangeRateText.text = "Nitrite Change Rate: Normal";
        nitrateChangeRateText.text = "Nitrate Change Rate: Normal";
        oxygenChangeRateText.text = "Oxygen Change Rate: Normal";
    }

    private void ChangeNozzleSetting(int change)
    {
        nozzleIndex = Mathf.Clamp(nozzleIndex + change, 0, nozzleOptions.Length - 1);
        nozzleText.text = nozzleOptions[nozzleIndex];
        UpdateFlowRate();
    }

    private void ChangeNozzleTiltSetting(int change)
    {
        nozzleTiltIndex = Mathf.Clamp(nozzleTiltIndex + change, 0, nozzleTiltOptions.Length - 1);
        nozzleTiltText.text = nozzleTiltOptions[nozzleTiltIndex];
        // Logic to change bubble particles movement based on nozzle tilt can be added here
    }

    private void ChangeVenturiSetting(int change)
    {
        venturiIndex = Mathf.Clamp(venturiIndex + change, 0, venturiOptions.Length - 1);
        venturiText.text = venturiOptions[venturiIndex];
        UpdateAirflowRate();
    }

    private void UpdateFlowRate()
    {
        switch (nozzleOptions[nozzleIndex])
        {
            case "closed":
                flowRateText.text = "Flow Rate: 0 L/min";
                break;
            case "1/2":
                flowRateText.text = "Flow Rate: " + (MAX_FLOW_RATE * 0.5f).ToString("0.00") + " L/min";
                break;
            case "open":
                flowRateText.text = "Flow Rate: " + MAX_FLOW_RATE.ToString("0.00") + " L/min";
                break;
        }
    }

    private void UpdateAirflowRate()
    {
        switch (venturiOptions[venturiIndex])
        {
            case "closed":
                airflowRateText.text = "Airflow Rate: 0 L/min";
                break;
            case "1/4":
                airflowRateText.text = "Airflow Rate: " + (MAX_AIRFLOW_RATE * 0.25f).ToString("0.00") + " L/min";
                break;
            case "1/2":
                airflowRateText.text = "Airflow Rate: " + (MAX_AIRFLOW_RATE * 0.5f).ToString("0.00") + " L/min";
                break;
            case "3/4":
                airflowRateText.text = "Airflow Rate: " + (MAX_AIRFLOW_RATE * 0.75f).ToString("0.00") + " L/min";
                break;
            case "open":
                airflowRateText.text = "Airflow Rate: " + MAX_AIRFLOW_RATE.ToString("0.00") + " L/min";
                break;
        }
    }



    public void UpdateFilterInfo(Filter filter)
    {
        nameText.text = filter.displayName;
        typeText.text = "Type: " + filter.type;
        descriptionText.text = "Description: " + filter.description;
        effectivenessText.text = "Effectiveness: " + filter.effectiveness.ToString() + "%";
        filterMediaText.text = "Filter Media: " + filter.filterMedia;
        filterCapacityText.text = "Filter Capacity: " + filter.filterCapacity.ToString() + " L";
        pHChangeRateText.text = "pH Change Rate: " + filter.pHChangeRate.ToString("0.00") + " pH/min";
        ammoniaChangeRateText.text = "Ammonia Change Rate: " + filter.ammoniaChangeRate.ToString("0.00") + " ppm/min";
        nitriteChangeRateText.text = "Nitrite Change Rate: " + filter.nitriteChangeRate.ToString("0.00") + " ppm/min";
        nitrateChangeRateText.text = "Nitrate Change Rate: " + filter.nitrateChangeRate.ToString("0.00") + " ppm/min";
        oxygenChangeRateText.text = "Oxygen Change Rate: " + filter.oxygenChangeRate.ToString("0.00") + " ppm/min";
        priceText.text = "$ " + filter.price.ToString("0.00");
        powerDrawText.text = "Power Draw: " + filter.powerDraw.ToString("0.00") + " W";
        flowRateText.text = "Flow Rate: " + filter.flowRate.ToString("0.00") + " L/min";
        airflowRateText.text = "Airflow Rate: " + filter.airflowRate.ToString("0.00") + " L/min";
        UpdateAmmoniaText();
    }

    private void UpdateAmmoniaText()
    {
        float ammoniaLevel = waterQualityParameters.GetAmmoniaLevel();
        ammoniaChangeRateText.text = "Ammonia Change Rate: " + ammoniaLevel.ToString("0.00");
    }

    public void SynchronizeWithFilterBehavior(FilterBehavior filterBehavior)
    {
        if (filterBehavior != null && filterBehavior.filterData != null)
        {
            UpdateFilterInfo(filterBehavior.filterData); // Pass the Filter object directly
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);
    }
}
