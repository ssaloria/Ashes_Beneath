using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    [Header("References")]
    public HealthUI healthUI;          // drag the Hearts object here
    public PlayerDeathUI deathUI;      // drag the PlayerDeathUI component here

    void Awake()
    {
        currentHealth = maxHealth;

        // Fallbacks if you forgot to assign
        if (healthUI == null) healthUI = FindFirstObjectByType<HealthUI>(FindObjectsInactive.Include);
        if (deathUI == null) deathUI = FindFirstObjectByType<PlayerDeathUI>(FindObjectsInactive.Include);

        if (healthUI != null) healthUI.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (healthUI != null) healthUI.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            deathUI?.ShowDeathScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            TakeDamage(1);
    }
}

