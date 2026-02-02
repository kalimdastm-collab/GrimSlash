using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bloodEffect;

    [Header("Sounds")]
    public AudioClip slashCorrectSound; 
    public AudioClip deathSound;        
    public AudioClip bloodSound;       

    [Header("Score Popup")]
    public GameObject scorePopupPrefab; 

    protected Animator anim;
    protected bool isDead = false;

    protected HealthSystem healthSystem;
    protected ScoreSystem scoreSystem;
    protected PotionSystem potionSystem;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        healthSystem = FindObjectOfType<HealthSystem>();
        scoreSystem = FindObjectOfType<ScoreSystem>();
        potionSystem = FindObjectOfType<PotionSystem>();
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        GetComponent<EnemyMove>()?.StopMoving();

        if (bloodEffect != null)
        {
            GameObject blood = Instantiate(bloodEffect, transform.position, Quaternion.identity);

            if (bloodSound != null)
                AudioSource.PlayClipAtPoint(bloodSound, transform.position);

            KillComboSystem.Instance?.RegisterKill(transform.position);
            Destroy(blood, 1.5f);
        }

        if (anim != null)
            anim.SetTrigger("DeathTrigger");

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (scoreSystem != null) 
            scoreSystem.AddScore(5);

        SpawnScorePopup(5);

        if (healthSystem != null)
        {
            if (healthSystem.currentHealth >= healthSystem.maxHealth)
            {
                if (potionSystem != null)
                    potionSystem.AddPotion(5);
            }
            else
            {
                healthSystem.Heal(5);
            }
        }

        Destroy(gameObject, 1.2f);
    }

    public virtual void ReachTarget()
    {
        if (isDead) return;
        isDead = true;


        if (healthSystem != null)
            healthSystem.TakeDamage(5);

        if (scoreSystem != null)
            scoreSystem.AddScore(-5);

        SpawnScorePopup(-5);

        Destroy(gameObject);
    }

    public virtual void OnSlashed(Vector2 slashDir)
    {
        if (slashCorrectSound != null)
            AudioSource.PlayClipAtPoint(slashCorrectSound, transform.position);

        Die();
    }

    private void SpawnScorePopup(int score)
    {
        if (scorePopupPrefab == null) return;

        GameObject popup = Instantiate(
            scorePopupPrefab,
            transform.position + Vector3.up * 1.2f,
            Quaternion.identity
        );

        ScorePopup sp = popup.GetComponent<ScorePopup>();
        if (sp != null)
            sp.Setup(score);
    }
}
