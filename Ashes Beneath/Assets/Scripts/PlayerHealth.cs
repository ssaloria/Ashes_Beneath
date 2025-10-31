using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    [Header("References")]
    public HealthUI healthUI;      // Hearts parent with HealthUI
    public PlayerDeathUI deathUI;  // YouDiedCanvas with PlayerDeathUI

    bool isDead;

    void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth <= 0 ? maxHealth : currentHealth, 0, maxHealth);

        if (healthUI == null)
            healthUI = FindFirstObjectByType<HealthUI>(FindObjectsInactive.Include);
        if (deathUI == null)
            deathUI = FindFirstObjectByType<PlayerDeathUI>(FindObjectsInactive.Include);

        if (Input.GetKeyDown(KeyCode.L)) TakeDamage(999); // insta-death


        healthUI?.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - Mathf.Abs(amount), 0, maxHealth);
        healthUI?.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            deathUI?.ShowDeathScreen();
        }
    }

    // ---- test keys (remove later) ----
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) TakeDamage(1);   // lose 1 HP
        if (Input.GetKeyDown(KeyCode.L)) TakeDamage(999); // insta-death
    }
}

