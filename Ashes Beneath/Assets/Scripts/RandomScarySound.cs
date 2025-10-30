using UnityEngine;
using System.Collections;

public class RandomScarySound : MonoBehaviour
{
    public AudioClip[] creepySounds;
    public float minDelay = 1f;
    public float maxDelay = 3f;

    AudioSource src;

    void Awake()
    {
        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;       // 2D ambience
        src.volume = 1f;
        src.ignoreListenerPause = true;
    }

    void Start()
    {
        PlayOne();                   // hear something immediately
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minDelay, maxDelay));
            PlayOne();
        }
    }

    void PlayOne()
    {
        if (creepySounds == null || creepySounds.Length == 0) return;
        var clip = creepySounds[Random.Range(0, creepySounds.Length)];
        if (!clip) return;
        src.pitch = Random.Range(0.95f, 1.05f);
        src.clip = clip;
        src.Play();
    }
}
