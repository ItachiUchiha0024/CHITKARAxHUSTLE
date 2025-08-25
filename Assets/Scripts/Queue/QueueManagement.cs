using System.Collections.Generic;
using UnityEngine;

public class QueueManagement : MonoBehaviour
{
    public static QueueManagement Instance;

    public Transform[] queuePositions;
    public GameObject employeePrefab;
    public int maxQueueSize = 4;

    private Queue<GameObject> currentQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // Spawn the first employee automatically at game start
        SpawnEmployee();
    }

    public void SpawnEmployee()
    {
        if (currentQueue.Count >= maxQueueSize)
        {
            Debug.Log("Queue is full.");
            return;
        }

        GameObject newEmployee = Instantiate(employeePrefab, queuePositions[currentQueue.Count].position, Quaternion.identity);
        currentQueue.Enqueue(newEmployee);
    }

    public void HandleCounterInteraction()
    {
        if (currentQueue.Count == 0)
        {
            Debug.Log("No employees in queue.");
            SpawnEmployee(); // Optional: You can remove this if you don't want auto-spawn on interaction
            return;
        }

        if (TaskManager.Instance.HasActiveTasks())
        {
            if (TaskManager.Instance.AreTasksCompleted())
            {
                UIManager.Instance.RemoveTasksFromNotepad(TaskManager.Instance.GetCurrentEmployeeTasks());
                TaskManager.Instance.ClearAllTasks(); // Clear tasks from TaskManager
                RemoveFrontEmployee();
                SpawnEmployee(); // Replace the completed one
            }
            else
            {
                Debug.Log("Tasks not done yet.");
            }
        }
        else
        {
            AssignTasksFromFrontEmployee(); // Door locking moved here
        }
    }

    private void AssignTasksFromFrontEmployee()
    {
        if (currentQueue.Count == 0)
        {
            Debug.Log("No employees to assign tasks from.");
            return;
        }

        List<List<string>> availableTaskPools = new List<List<string>>()
        {
            TaskLibrary.DeansOfficeTasks,
            TaskLibrary.BabbageBlockTasks,
            TaskLibrary.StaffRoom1Tasks,
            TaskLibrary.NewtonBlockTasks,
            TaskLibrary.EdisonBlockTasks,
            TaskLibrary.FlemmingBlockTasks,
            TaskLibrary.StaffRoom2Tasks,
            TaskLibrary.TuringBlockTasks,
            TaskLibrary.PantryTasks,
            TaskLibrary.Square1Tasks,
            TaskLibrary.Square2Tasks
        };

        List<string> assignedTasks = new List<string>();

        while (assignedTasks.Count < 3 && availableTaskPools.Count > 0)
        {
            int poolIndex = Random.Range(0, availableTaskPools.Count);
            List<string> selectedPool = availableTaskPools[poolIndex];
            string task = selectedPool[Random.Range(0, selectedPool.Count)];
            assignedTasks.Add(task);
            availableTaskPools.RemoveAt(poolIndex);
        }

        TaskManager.Instance.AssignTasks(assignedTasks);
        Debug.Log($"Assigned tasks: {string.Join(", ", assignedTasks)}");

        // 🔐 Lock doors *after* new tasks are assigned
        DoorManager.Instance.InitializeDoorLocks();
        KeySpawner.Instance.SpawnKey();
    }

    private void RemoveFrontEmployee()
    {
        GameObject frontEmployee = currentQueue.Dequeue();
        Destroy(frontEmployee);
        RepositionQueue();
    }

    private void RepositionQueue()
    {
        int index = 0;
        foreach (GameObject emp in currentQueue)
        {
            emp.transform.position = queuePositions[index].position;
            index++;
        }
    }

    public int QueueCount() => currentQueue.Count;
}
