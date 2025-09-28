using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.allLevels[0]);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
