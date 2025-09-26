using UnityEngine;

public class DeathSound : MonoBehaviour
{
    public AudioSource audioSource; // the scary sound for YOU DIED

    void Update()
    {
        // Press K to test the death sound
        if (Input.GetKeyDown(KeyCode.K))
        {
            audioSource.Play();
        }
    }
}
