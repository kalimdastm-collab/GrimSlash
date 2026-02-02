using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PotionSystem : MonoBehaviour
{
    [Header("Potion Values")]
    public float maxPotion = 100f;
    public float currentPotion = 0f;

    [Header("UI")]
    public Image potionFill;      
    public Button potionButton;

    [Header("Cooldown")]
    public float cooldownTime = 10f;
    private bool isOnCooldown = false;

    [Header("Magic Effects")]
    public GameObject magicEffectPrefab;
    public AudioClip magicReadySound;
    public AudioClip magicUseSound;

    private HealthSystem healthSystem;

    private bool readySoundPlayed = false;

    void Awake()
    {
        healthSystem = FindObjectOfType<HealthSystem>();
        currentPotion = 0f;
        readySoundPlayed = false;
        UpdateUI();
    }

    public void AddPotion(float amount)
    {
        if (healthSystem == null) return;

        if (healthSystem.currentHealth < healthSystem.maxHealth)
        {
            float healthMissing = healthSystem.maxHealth - healthSystem.currentHealth;
            float healValue = Mathf.Min(amount, healthMissing);

            healthSystem.Heal(healValue);
            return;
        }

        currentPotion += amount;
        currentPotion = Mathf.Clamp(currentPotion, 0f, maxPotion);

        if (currentPotion >= maxPotion && !readySoundPlayed)
        {
            currentPotion = maxPotion;
            readySoundPlayed = true;

            if (magicReadySound != null)
                AudioSource.PlayClipAtPoint(
                    magicReadySound,
                    Camera.main.transform.position
                );

            StartCoroutine(PulsePotionButton());
        }

        UpdateUI();
    }

    public void UsePotion()
    {
        if (currentPotion <= 0f) return;
        if (healthSystem == null) return;
        if (isOnCooldown) return;

        float healthMissing =
            healthSystem.maxHealth - healthSystem.currentHealth;

        if (healthMissing <= 0f) return;

        float healValue = Mathf.Min(currentPotion, healthMissing);

        if (magicEffectPrefab != null)
        {
            GameObject fx = Instantiate(
                magicEffectPrefab,
                healthSystem.transform.position,
                Quaternion.identity
            );
            Destroy(fx, 2f);
        }

        healthSystem.Heal(healValue);

        currentPotion -= healValue;
        currentPotion = Mathf.Clamp(currentPotion, 0f, maxPotion);

        if (magicUseSound != null)
            AudioSource.PlayClipAtPoint(
                magicUseSound,
                Camera.main.transform.position
            );

        if (currentPotion < maxPotion)
            readySoundPlayed = false;

        UpdateUI();

        if (currentPotion <= 0f)
            StartCoroutine(PotionCooldown());
    }

    void UpdateUI()
    {
        if (potionFill != null)
            potionFill.fillAmount = currentPotion / maxPotion;

        if (potionButton != null)
            potionButton.interactable =
                currentPotion >= maxPotion && !isOnCooldown;
    }

    IEnumerator PotionCooldown()
    {
        isOnCooldown = true;

        if (potionButton != null)
            potionButton.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
        UpdateUI();
    }

    IEnumerator PulsePotionButton()
    {
        if (potionButton == null) yield break;

        float t = 0f;
        Vector3 originalScale = potionButton.transform.localScale;

        while (t < 0.4f)
        {
            t += Time.deltaTime;
            float scale =
                1f + Mathf.Sin(t * Mathf.PI * 6f) * 0.08f;

            potionButton.transform.localScale =
                originalScale * scale;

            yield return null;
        }

        potionButton.transform.localScale = originalScale;
    }
}
