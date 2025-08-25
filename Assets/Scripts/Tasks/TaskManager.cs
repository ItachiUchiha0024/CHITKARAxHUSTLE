using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    private HashSet<string> activeTasks = new HashSet<string>();
    private HashSet<string> completedTasks = new HashSet<string>();

    public List<string> taskList = new List<string>();
    private List<string> currentEmployeeTasks = new List<string>(); // Store current employee's tasks

    [Header("Audio Clips")]
    public AudioClip assignTaskSound;
    public AudioClip taskCompletedSound;
    public AudioClip allTasksCompletedSound;
    public AudioClip ticktock;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void AddTask(string task)
    {
        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
            UIManager.Instance.AddTaskToNotepad(task);
        }
    }

    public void CompleteTask(string task)
    {
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
            completedTasks.Add(task);

            UIManager.Instance.MarkTaskAsCompleted(task);
            Debug.Log($"Task completed: {task}");

            // Play task completion sound
            PlaySound(taskCompletedSound);

            // Check if all tasks are completed
            if (activeTasks.Count == 0)
            {
                Debug.Log("All current employee tasks completed.");
            }
            else
            {
                Debug.Log($"Tasks remaining: {activeTasks.Count}");
            }
        }
    }

    public string AssignRandomTask()
    {
        if (taskList.Count == 0)
        {
            Debug.LogError("No tasks available in the task list!");
            return null;
        }

        int randomIndex = Random.Range(0, taskList.Count);
        return taskList[randomIndex];
    }

    public void AssignTasks(List<string> tasks)
    {
        // Clear current tasks and notepad
        UIManager.Instance.RemoveTasksFromNotepad(currentEmployeeTasks);
        activeTasks.Clear();
        currentEmployeeTasks.Clear();

        foreach (var task in tasks)
        {
            AddTask(task);
            currentEmployeeTasks.Add(task);
        }

        // Play the first task assignment sound
        PlaySound(assignTaskSound);

        PlaySound(ticktock);
        TaskTimer.Instance.DecreaseStartingTime();
        TaskTimer.Instance.StartTimer();
    }

    // You'll need to declare these at the top of your TaskManager class:
    

    

    public bool HasActiveTasks() => activeTasks.Count > 0;

    public bool AreTasksCompleted() => activeTasks.Count == 0 && completedTasks.Count > 0;

    public void CompleteAllTasks()
    {
        completedTasks.Clear();
        UIManager.Instance.MarkAllTasksAsCompleted();
        TaskTimer.Instance.ResetTimer();

        // Play all tasks completed sound
        PlaySound(allTasksCompletedSound);

        Debug.Log("All tasks cleared and marked complete.");
    }

    public bool IsTaskActive(string task) => activeTasks.Contains(task);
    public bool IsTaskCompleted(string task) => completedTasks.Contains(task);

    public void FailAllTasksDueToTimeout()
    {
        foreach (var task in activeTasks)
        {
            UIManager.Instance.MarkAllTasksAsFailed();
        }

        activeTasks.Clear();
        completedTasks.Clear();
        UIManager.Instance.MarkAllTasksAsFailed();
        TaskTimer.Instance.ResetTimer();

        Debug.Log("All tasks failed due to timeout.");
    }

    public void ClearAllTasks()
    {
        activeTasks.Clear();
        completedTasks.Clear();
        currentEmployeeTasks.Clear();
        UIManager.Instance.RemoveTasksFromNotepad(currentEmployeeTasks);
    }

    public List<string> GetCurrentEmployeeTasks() => currentEmployeeTasks;

    // Helper method to play sounds
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
