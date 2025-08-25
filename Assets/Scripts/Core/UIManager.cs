using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Message Display")]
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;
    private Coroutine messageRoutine;

    [Header("Notepad")]
    public GameObject notepadPanel;
    public GameObject taskEntryPrefab;
    public Transform taskListParent;

    private Dictionary<string, TMP_Text> taskEntries = new Dictionary<string, TMP_Text>();
    public TMP_Text permanentMessageText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        messagePanel.SetActive(false);
        if (notepadPanel != null)
            notepadPanel.SetActive(false); // Ensure notepad is hidden by default

        if (permanentMessageText != null)
        {
            permanentMessageText.text = "This is a permanent message"; // Set your message here
            permanentMessageText.gameObject.SetActive(true); // Ensure it's always visible
        }
    }

    private void Update()
    {
        // Open the notepad when 'N' is held down
        if (Input.GetKey(KeyCode.N))
        {
            if (notepadPanel != null && !notepadPanel.activeSelf)
            {
                notepadPanel.SetActive(true); // Open the notepad
                // Enable all task entries when the notepad is opened
                foreach (var entry in taskEntries)
                {
                    entry.Value.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            // Close the notepad when 'N' is released
            if (notepadPanel != null && notepadPanel.activeSelf)
            {
                notepadPanel.SetActive(false); // Close the notepad
                // Optionally, hide task entries when the notepad is closed
                foreach (var entry in taskEntries)
                {
                    entry.Value.gameObject.SetActive(false);
                }
            }
        }
    }

    // Show blinking message followed by regular display
    public void ShowMessageWithBlink(string message, Color color, float blinkDuration = 1f, float displayDuration = 5f)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(BlinkMessageRoutine(message, color, blinkDuration, displayDuration));
    }

    private IEnumerator BlinkMessageRoutine(string message, Color color, float blinkDuration, float displayDuration)
    {
        messageText.text = message;
        messageText.color = color;
        messagePanel.SetActive(true);

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < blinkDuration)
        {
            messageText.enabled = isVisible;
            isVisible = !isVisible;
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        messageText.enabled = true;
        yield return new WaitForSeconds(displayDuration - blinkDuration);
        messagePanel.SetActive(false);
    }

    // Instant message with color (no blink)
    public void ShowMessage(string message, Color color, float duration = 5f)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(DisplayMessage(message, color, duration));
    }

    private IEnumerator DisplayMessage(string message, Color color, float duration)
    {
        messageText.text = message;
        messageText.color = color;
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }

    // NOTEPAD FUNCTIONS

    // Add task to the notepad, but only if the notepad is open
    public void AddTaskToNotepad(string taskText)
    {
        if (taskEntries.ContainsKey(taskText))
            return; // Prevent duplicate tasks from being added

        // Instantiate the task entry prefab and set it as a child of the taskListParent
        GameObject entry = Instantiate(taskEntryPrefab, taskListParent);
        TMP_Text text = entry.GetComponent<TMP_Text>();

        if (text == null)
        {
            Debug.LogError("TMP_Text component not found in task entry prefab.");
            return;
        }

        // Set the task text to the TMP_Text component
        text.text = taskText;

        // Ensure the task entry is disabled initially
        entry.SetActive(false);

        // Add the task to the dictionary for later reference
        taskEntries.Add(taskText, text);
    }

    // Mark a task as completed in the notepad UI
    public void MarkTaskAsCompleted(string taskText)
    {
        if (taskEntries.TryGetValue(taskText, out TMP_Text text))
        {
            text.color = Color.gray;
            text.fontStyle = FontStyles.Strikethrough;
        }
    }

    public void MarkAllTasksAsCompleted()
    {
        foreach (var taskEntry in taskEntries.Values)
        {
            taskEntry.color = Color.gray;
            taskEntry.fontStyle = FontStyles.Strikethrough;
        }
    }

    public void MarkAllTasksAsFailed()
    {
        // Highlight all tasks in red or strike them out (can be customized)
        foreach (var taskEntry in taskEntries.Values)
        {
            taskEntry.color = Color.red;
            taskEntry.fontStyle = FontStyles.Strikethrough;
        }
    }

    // New method to remove completed tasks from the notepad
    public void RemoveTasksFromNotepad(List<string> tasksToRemove)
    {
        foreach (string task in tasksToRemove)
        {
            if (taskEntries.TryGetValue(task, out TMP_Text entry))
            {
                Destroy(entry.gameObject); // Remove the task entry object from the notepad
                taskEntries.Remove(task); // Remove it from the task entries dictionary
            }
        }
    }

    // Update task list in the notepad (newly added method)
    public void UpdateTaskList(HashSet<string> activeTasks)
    {
        // Clear existing tasks
        foreach (Transform child in taskListParent)
        {
            Destroy(child.gameObject); // Remove existing task entries
        }

        // Re-add all active tasks
        foreach (string task in activeTasks)
        {
            AddTaskToNotepad(task);
        }
    }
}
