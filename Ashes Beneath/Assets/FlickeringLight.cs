using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    private Light lightSource;

    public float minDelay = 0.25f;
    public float maxDelay = 0.75f;

    void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            lightSource.enabled = !lightSource.enabled;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
