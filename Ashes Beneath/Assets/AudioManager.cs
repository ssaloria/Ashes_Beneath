using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;         
    public AudioClip[] creepySounds;        
    public float minDelay = 5f; // Before the new sound can play            
    public float maxDelay = 15f; // before the new sound can play      

    private float timer; // It counts down until the next sound

    void Start()
    {
        timer = Random.Range(minDelay, maxDelay); // picks random delay between minDelay and maxDelay
    }

    void Update()
    {
        timer -= Time.deltaTime; // It decreases the timer every frame
        // if the timer reaches 0 and still have sounds available
        if (timer <= 0f && creepySounds.Length > 0)
        {
            // chooses random sound 
            int index = Random.Range(0, creepySounds.Length);
            AudioClip chosenClip = creepySounds[index];

            audioSource.PlayOneShot(chosenClip);

            // resets the timer , wait until the clip finishes + random delay
            timer = chosenClip.length + Random.Range(minDelay, maxDelay);
        }
    }
}
