// Import Libraries
using UnityEngine;
using UnityEngine.AI;

public class DoorLock : MonoBehaviour
{
    [Header("Door Setup")]
    public KeyColor requiredColor;
    public int levelIndex = 0;
    public Animator animator;              // Controller with Open Animation
    public string openTrigger = "Open";
    public bool isOpen = false;

    [Header("NavMesh")]
    public NavMeshObstacle obstacleToCarve; //Potential for Antagonist interaction

    void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        obstacleToCarve = GetComponentInChildren<NavMeshObstacle>();
    }

    void OnEnable()
    {
        PlayerKeys.OnKeyCollected += OnKeyCollected;
    }

    void OnDisable()
    {
        PlayerKeys.OnKeyCollected -= OnKeyCollected;
    }

    void Start()
    {
        // If player already has the key (e.g., scene reload), open immediately
        var player = GameObject.FindGameObjectWithTag("Player");
        var inv = player ? player.GetComponentInChildren<PlayerKeys>() : null;
        if (inv && inv.HasKey(requiredColor, levelIndex))
            OpenNow();
    }

    void OnKeyCollected(KeyColor color, int lvl)
    {
        if (isOpen) return;
        if (lvl != levelIndex) return;
        if (color != requiredColor) return;

        OpenNow();
    }

    void OpenNow()
    {
        isOpen = true;

        if (obstacleToCarve) obstacleToCarve.enabled = false; // Allow Antagonists through

        if (animator)
        {
            animator.ResetTrigger(openTrigger);
            animator.SetTrigger(openTrigger);   // Play once and Freeze
        }
    }
}
