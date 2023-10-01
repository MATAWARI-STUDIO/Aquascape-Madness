using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI yearText;
    public TextMeshProUGUI weekText;
    public TextMeshProUGUI dayText;
    private int year = 1; // Assuming you start at year 1
    private int week = 1; // Assuming you start at week 1
    private int day = 1;  // Assuming you start at day 1

    public static TimeController Instance { get; private set; }
    public TimeManager timeManager; // Reference to the TimeManager script
    public float fastForwardMultiplier = 2f;
    private bool isPaused;
    private float timeScale = 1f;
    private float currentTime = 28800f;

    public TextMeshProUGUI stateText;
    public TextMeshProUGUI clockText;

    public Button pauseButton;
    public Button playButton;
    public Button fastForward2xButton;
    public Button fastForward4xButton;


    public float CurrentTime
    {
        get { return currentTime; }
    }

    // Properties to get the current hour and minute
    public int CurrentHour
    {
        get { return Mathf.FloorToInt(currentTime / 3600) % 24; }
    }

    public int CurrentMinute
    {
        get { return Mathf.FloorToInt((currentTime % 3600) / 60); }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        pauseButton.onClick.AddListener(TogglePause);
        playButton.onClick.AddListener(Resume);
        fastForward2xButton.onClick.AddListener(FastForward2x);
        fastForward4xButton.onClick.AddListener(FastForward4x);
    }

    private void Start()
    {
        StartCoroutine(UpdateTime());
    }

    private System.Collections.IEnumerator UpdateTime()
    {
        while (true)
        {
            if (!isPaused)
            {
                currentTime += Time.deltaTime * timeScale * 150.0f; // Further reduced to 150.0f
                if (currentTime >= 86400f)
                {
                    currentTime = 0f;
                    timeManager.IncrementDay(); // Call the IncrementDay method from TimeManager
                }
                UpdateClockText(currentTime);
            }
            yield return null;
        }
    }


    private void UpdateClockText(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600) % 24;
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        clockText.text = "Time: " + formattedTime;
    }

    public void TogglePause()
    {
        isPaused = true;
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        UpdateStateText("Paused");
    }

    public void Resume()
    {
        if (timeScale > 1f)
        {
            timeScale = 1f;
            Time.timeScale = timeScale;
            UpdateStateText("Playing");
        }
        else
        {
            isPaused = false;
            Time.timeScale = timeScale;
            UpdateStateText("Playing");
        }
    }

    public void FastForward2x()
    {
        timeScale = 2f;
        Time.timeScale = timeScale;
        UpdateStateText("FF2x");
    }

    public void FastForward4x()
    {
        timeScale = 4f;
        Time.timeScale = timeScale;
        UpdateStateText("FF4x");
    }

    private void UpdateStateText(string newState)
    {
        stateText.text = newState;
    }

    public static bool IsGamePausedOrFastForwarded()
    {
        return Time.timeScale == 0f || Time.timeScale > 1f;
    }

    public static bool IsGameFastForwarded()
    {
        return Time.timeScale > 1f;
    }

    private void IncrementDay()
    {
        day++;
        UpdateCalendarText();
    }

    private void UpdateCalendarText()
    {
        yearText.text = "Year: " + year;
        weekText.text = "Week: " + week;
        dayText.text = "Day: " + day;
    }
}
