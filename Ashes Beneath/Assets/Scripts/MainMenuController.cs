using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
public void NewGame()
{
    Time.timeScale = 1f;
    SaveManager.DeleteSave();
    PlayerPrefs.SetInt("ShouldLoadSave", 0);
    SceneManager.LoadScene("Environment FINAL");
}


    public void LoadGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("ShouldLoadSave", 1);
        SceneManager.LoadScene("Environment FINAL");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}
