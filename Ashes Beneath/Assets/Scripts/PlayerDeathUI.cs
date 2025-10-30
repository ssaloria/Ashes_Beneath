using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeathUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject youDiedCanvas;   // assign YouDiedCanvas
    [SerializeField] private Animator deathAnimator;     // Animator on YouDiedCanvas
    [SerializeField] private float pauseDelay = 1.0f;

    public void ShowDeathScreen()
    {
        if (youDiedCanvas != null && !youDiedCanvas.activeSelf)
            youDiedCanvas.SetActive(true);

        if (deathAnimator != null)
            deathAnimator.SetTrigger("Show");

        StartCoroutine(PauseAfter(pauseDelay));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator PauseAfter(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }

    // Optional buttons
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
