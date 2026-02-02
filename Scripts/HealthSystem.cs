using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Values")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float CurrentHealth => currentHealth;

    public HealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
            healthUI.SetHealth(currentHealth / maxHealth);
        else
            Debug.LogError("HealthUI not connected");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthUI != null)
            healthUI.SetHealth(currentHealth / maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthUI != null)
            healthUI.SetHealth(currentHealth / maxHealth);
    }
}
