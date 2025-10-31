using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeathUI : MonoBehaviour
{
    public GameObject youDiedCanvas;   // drag this YouDiedCanvas

    public void ShowDeathScreen()
    {
        if (youDiedCanvas && !youDiedCanvas.activeSelf) youDiedCanvas.SetActive(true);
        StartCoroutine(PauseAfter(0.5f));
        Cursor.visible = true; Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator PauseAfter(float t) { yield return new WaitForSecondsRealtime(t); Time.timeScale = 0f; }

    public void Retry() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}


