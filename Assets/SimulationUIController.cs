using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationUIController : MonoBehaviour
{
    public Button playButton;
    public Button pauseButton;
    public Button fastForwardButton1x;
    public Button fastForwardButton2x;
    public TMP_Text speedText;

    private float simulationSpeed = 1.0f;

    private void Start()
    {
        playButton.onClick.AddListener(() => PlaySimulation());
        pauseButton.onClick.AddListener(() => PauseSimulation());
        fastForwardButton1x.onClick.AddListener(() => FastForward1xSimulation());
        fastForwardButton2x.onClick.AddListener(() => FastForward2xSimulation());


        UpdateSpeedText();
    }

    private void Update()
    {
        // Implement simulation update logic here with adjusted simulationSpeed
    }

    private void PlaySimulation()
    {
        // Logic to start or resume simulation
        Time.timeScale = simulationSpeed;
    }

    private void PauseSimulation()
    {
        // Logic to pause simulation
        Time.timeScale = 0;
    }

    private void FastForward1xSimulation()
    {
        SetSimulationSpeed(1.0f);
    }

    private void FastForward2xSimulation()
    {
        SetSimulationSpeed(2.0f);
    }

    private void SetSimulationSpeed(float speed)
    {
        simulationSpeed = speed;
        Time.timeScale = simulationSpeed;
        UpdateSpeedText();
    }

    private void UpdateSpeedText()
    {
        speedText.text = "Speed: " + simulationSpeed.ToString("0.0") + "x";
    }
}