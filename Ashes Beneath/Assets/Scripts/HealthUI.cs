using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;          // 5 images
    [Range(0f, 1f)] public float emptyAlpha = 0.25f;

    public void SetHealth(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            bool withinMax = i < max;
            if (hearts[i]) hearts[i].gameObject.SetActive(withinMax);
            if (!withinMax) continue;

            bool filled = i < current;
            var c = hearts[i].color;
            c.a = filled ? 1f : emptyAlpha;
            hearts[i].color = c;
        }
    }
}
