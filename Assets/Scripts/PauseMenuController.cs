using UnityEngine;
using UnityEngine.UI;  // For working with buttons and UI elements
using TMPro; // For working with TextMeshPro
using UnityEngine.SceneManagement; // For scene management (required for Restart)

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    public TMP_Text pausedText; // Reference to the TMP "Paused" text
    public Button resumeButton; // Reference to the resume button (Unity's Button class)
    public Button restartButton; // Reference to the restart button (Unity's Button class)
    public Button quitButton; // Reference to the quit button (Unity's Button class)
    public AudioSource backgroundAudio; // Reference to the AudioSource playing the background music

    private bool isPaused = false; // To track the game state (paused or not)

    private void Start()
    {
        // Hide the pause menu on start
        pauseMenu.SetActive(false);

        // Add listeners to buttons
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Check if the player presses the pause button (e.g., "Esc")
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Pause the game and show the pause menu
        Time.timeScale = 0f; // Freezes the game
        isPaused = true;
        pauseMenu.SetActive(true); // Show the pause menu
        pausedText.text = "PAUSED"; // Change the paused text

        // Pause the background audio
        if (backgroundAudio != null)
        {
            backgroundAudio.Pause(); // Pause the background audio
        }
    }

    public void ResumeGame()
    {
        // Unpause the game and hide the pause menu
        Time.timeScale = 1f; // Unfreeze the game
        isPaused = false;
        pauseMenu.SetActive(false); // Hide the pause menu

        // Resume the background audio
        if (backgroundAudio != null)
        {
            backgroundAudio.UnPause(); // Unpause the background audio
        }
    }

    public void RestartGame()
    {
        // Restart the current scene
        Time.timeScale = 1f; // Unfreeze the game (in case it was paused)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene

        // Restart the background audio after scene reload
        if (backgroundAudio != null)
        {
            backgroundAudio.Stop(); // Stop the current background music
            backgroundAudio.Play(); // Restart the background music
        }
    }

    public void QuitGame()
    {
        // Quit the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops the game in the editor
#else
        Application.Quit(); // Quit the game in a build
#endif
    }
}
