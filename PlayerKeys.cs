// Author: Brody Austen
// Student ID: 21139516

// Import Libraries
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    public static event Action<KeyColor, int> OnKeyCollected;

    private HashSet<string> keys = new HashSet<string>();
    string MakeId(KeyColor c, int level) => $"{level}:{c}";

    public bool HasKey(KeyColor color, int levelIndex) =>
        keys.Contains(MakeId(color, levelIndex));

    public bool AddKey(KeyColor color, int levelIndex)
    {
        if (keys.Add(MakeId(color, levelIndex)))
        {
            OnKeyCollected?.Invoke(color, levelIndex);
            return true;
        }
        return false;
    }
}
