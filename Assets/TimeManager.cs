using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private int year = 1;
    private int week = 1;
    private int day = 1;

    public TextMeshProUGUI yearText;
    public TextMeshProUGUI weekText;
    public TextMeshProUGUI dayText;

    public float timeScaleFactor = 600.0f;

    public void IncrementDay()
    {
        day++;
        if (day > 365)
        {
            day = 1;
            year++;
        }
        UpdateCalendarText();
    }

    private void UpdateCalendarText()
    {
        yearText.text = "Year: " + year;
        weekText.text = "Week: " + week;
        dayText.text = "Day: " + day;
    }

    public float CurrentTime
    {
        get { return year * 365 * 86400 + week * 7 * 86400 + day * 86400 + Time.timeSinceLevelLoad; }
    }
}
