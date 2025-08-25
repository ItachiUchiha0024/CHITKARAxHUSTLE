using UnityEngine;

public class TaskCompletionTrigger : MonoBehaviour
{
    public string[] taskNames; // Array to hold multiple task names

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool allTasksCompleted = true;

            // Loop through all task names and check if they are active
            foreach (string taskName in taskNames)
            {
                if (TaskManager.Instance.IsTaskActive(taskName))
                {
                    // Complete the task if it's active
                    TaskManager.Instance.CompleteTask(taskName);
                }
                else
                {
                    // If any task isn't active, mark it as incomplete
                    allTasksCompleted = false;
                }
            }

            // Display a message indicating whether all tasks were completed or not
            
        }
    }
}
