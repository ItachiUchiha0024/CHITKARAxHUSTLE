using UnityEngine;

public class GameManager : MonoBehaviour
{
    public KeySpawner keySpawner; // Reference to KeySpawner script

    void Start()
    {
        ShowStartingMessage();
    }

    void ShowStartingMessage()
    {
        Debug.Log("ShowStartingMessage called");
        // Get all the spawn point names and make the starting message red
        string keyLocationMessage = "<color=red>Some of the doors might be locked. The keys are usually at: </color>";

        foreach (Transform spawnPoint in keySpawner.keySpawnPoints)
        {
            // Ensure room names are also red
            keyLocationMessage += "<color=red>" + spawnPoint.name + "</color> or ";
        }

        // Remove the last comma and space
        keyLocationMessage = keyLocationMessage.TrimEnd(',', ' ');

        // Show the blinking message for 1 second and then stay for 5 seconds
        UIManager.Instance.ShowMessageWithBlink(keyLocationMessage, Color.red, 1f, 10f);

    }
}
