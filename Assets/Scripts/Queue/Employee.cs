using UnityEngine;

public class Employee : MonoBehaviour
{
    public string[] tasks; // Array of task names assigned to this employee

    // Method to get the tasks assigned to this employee
    public string[] GetTasks()
    {
        return tasks;
    }

    // Method to assign tasks to the player
    public void AssignTasksToPlayer()
    {
        // Iterate through the tasks and add each one to the player's task list
        foreach (string task in tasks)
        {
            TaskManager.Instance.AddTask(task);  // Adding the task to the TaskManager
        }

        // Log the assigned tasks for debugging purposes
        Debug.Log($"Assigned tasks from {gameObject.name}: {string.Join(", ", tasks)}");
    }
}
