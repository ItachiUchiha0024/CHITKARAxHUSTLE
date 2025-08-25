using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // Set this in the Inspector

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
