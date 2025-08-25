using UnityEngine;
using TMPro;

public class TaskTimer : MonoBehaviour
{
    public static TaskTimer Instance;

    public float taskDuration = 120f; // Initial starting time in seconds
    public float minDuration = 30f;   // Minimum allowed time
    private float remainingTime;
    private bool isTimerRunning = false;

    public TextMeshProUGUI timerText; // Assign in inspector
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= -60f) // Stop the timer at -60 seconds
            {
                remainingTime = -60f;
                TimerExpired();
            }
        }
    }

    // Start the timer with an optional custom duration (if none, it uses taskDuration)
    public void StartTimer(float customDuration = -1f)
    {
        remainingTime = customDuration > 0 ? customDuration : taskDuration;
        isTimerRunning = true;
        UpdateTimerUI();
    }

    // Reset the timer to 0
    public void ResetTimer()
    {
        remainingTime = 0f;
        isTimerRunning = false;
        UpdateTimerUI();
    }

    // Stop the timer manually
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Check if the timer is running
    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }

    // Decrease the starting time by a given amount, ensuring it doesn't go below the minimum duration
    public void DecreaseStartingTime(float amount = 30f)
    {
        taskDuration -= amount;
        if (taskDuration < minDuration)
            taskDuration = minDuration;

        Debug.Log($"New taskDuration: {taskDuration} seconds");
    }

    // Handles timer expiration logic
    private void TimerExpired()
    {
        isTimerRunning = false;
        remainingTime = -60f; // Stop at -60 seconds
        UpdateTimerUI();

        Debug.Log("Task timer expired!");

        // Call TaskManager to cancel tasks and deduct penalty
        TaskManager.Instance.FailAllTasksDueToTimeout();
    }

    // Update the timer UI and handle color change when time goes negative
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int totalSeconds = Mathf.FloorToInt(remainingTime);
            int minutes = Mathf.Abs(totalSeconds / 60);
            int seconds = Mathf.Abs(totalSeconds % 60);

            // Display "-" prefix if time is negative
            string prefix = remainingTime < 0 ? "-" : "";

            // Change color when timer goes below 0
            timerText.color = remainingTime < 0 ? warningColor : normalColor;

            timerText.text = $"{prefix}{minutes:00}:{seconds:00}";
        }
    }
}
