using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;         
    public AudioClip[] creepySounds;        
    public float minDelay = 5f;             
    public float maxDelay = 15f;           

    private float timer;

    void Start()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && creepySounds.Length > 0)
        {
            int index = Random.Range(0, creepySounds.Length);
            AudioClip chosenClip = creepySounds[index];
            audioSource.PlayOneShot(chosenClip);

            
            timer = chosenClip.length + Random.Range(minDelay, maxDelay);
        }
    }
}
