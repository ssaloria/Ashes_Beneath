// Author: Brody Austen
// Student ID: 21139516

// Import Library
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour
{
    public KeyColor color;
    public int levelIndex = 0; // One key per level based on Index
    public AudioSource pickupAudio; // Audio cue on collection !DEPRECIATED!

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true; // Pickup Trigger
    }

    // Collect key on collision
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<PlayerKeys>();
        if (!inv) inv = other.GetComponentInParent<PlayerKeys>();
        if (!inv) return;

        if (inv.AddKey(color, levelIndex))
        {
            if (pickupAudio) pickupAudio.Play();
            Destroy(gameObject); // Unload key
        }
    }
}
