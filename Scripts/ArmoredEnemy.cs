using UnityEngine;
using System.Collections;

public enum SlashDirection { Up, Down, Left, Right }

public class ArmoredEnemy : Enemy
{
    [Header("Arrow / Slash Info")]
    public GameObject arrowIndicator; 
    public Sprite upArrow, downArrow, leftArrow, rightArrow;

    [Header("Metal Hit Effect / Sound")]
    public GameObject metalSparkPrefab;
    public AudioClip metalHitSound;

    [Header("Slash / Death / Blood Sounds")]
    public AudioClip SlashSound;
    public AudioClip armoredDeathSound;       
    public AudioClip armoredbloodSound;        

    protected SlashDirection expectedSlash;
    public int points = 10; 

    private Animator anime;

    protected override void Awake()
    {
        base.Awake();
        anime = GetComponent<Animator>();
    }

    void Start()
    {
        SetRandomExpectedSlash();
    }

    public void SetRandomExpectedSlash()
    {
        expectedSlash = (SlashDirection)Random.Range(0, 4);

        if (arrowIndicator == null) return;
        SpriteRenderer sr = arrowIndicator.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        sr.transform.localRotation = Quaternion.identity;
        sr.transform.localScale = Vector3.one;

        switch (expectedSlash)
        {
            case SlashDirection.Up: sr.sprite = upArrow; break;
            case SlashDirection.Down: sr.sprite = downArrow; break;
            case SlashDirection.Left: sr.sprite = leftArrow; break;
            case SlashDirection.Right: sr.sprite = rightArrow; break;
        }
    }

    public override void OnSlashed(Vector2 slashDir)
    {
        if (slashDir.magnitude < 0.2f) return;

        Vector2 expectedDir = GetExpectedVector();
        float dot = Vector2.Dot(slashDir.normalized, expectedDir);

        if (dot > 0.7f)
        {
            if (SlashSound != null)
                AudioSource.PlayClipAtPoint(SlashSound, transform.position);

            DieWithScore();
        }
        else
        {
            if (metalSparkPrefab != null)
            {
                GameObject spark = Instantiate(metalSparkPrefab, transform.position, Quaternion.identity);
                Destroy(spark, 1f);
            }

            if (metalHitSound != null)
                AudioSource.PlayClipAtPoint(metalHitSound, transform.position);
        }

        SetRandomExpectedSlash();
    }

    Vector2 GetExpectedVector()
    {
        switch (expectedSlash)
        {
            case SlashDirection.Up: return Vector2.up;
            case SlashDirection.Down: return Vector2.down;
            case SlashDirection.Left: return Vector2.left;
            case SlashDirection.Right: return Vector2.right;
        }
        return Vector2.zero;
    }

    protected void DieWithScore()
    {
        if (isDead) return;
        isDead = true;

        if (armoredDeathSound != null)
            AudioSource.PlayClipAtPoint(armoredDeathSound, transform.position);

        GetComponent<EnemyMove>()?.StopMoving();

        if (bloodEffect != null)
        {
            GameObject blood = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            if (armoredbloodSound != null)
                AudioSource.PlayClipAtPoint(armoredbloodSound, transform.position);
            Destroy(blood, 1.5f);
        }

        if (anime != null)
            anime.SetTrigger("DeathTrigger");

        
        ScoreSystem ss = FindObjectOfType<ScoreSystem>();
        if (ss != null)
        {
            ss.AddScore(points);
            SpawnScorePopup(points);
        }

        if (healthSystem != null)
        {
            if (healthSystem.currentHealth >= healthSystem.maxHealth)
            {
                if (potionSystem != null)
                    potionSystem.AddPotion(points);
            }
            else
            {
                healthSystem.Heal(points);
            }
        }

        KillComboSystem.Instance?.RegisterKill(transform.position);
        Destroy(gameObject, 1.2f);
    }

    public override void ReachTarget()
    {
        if (isDead) return;
        isDead = true;

        ScoreSystem ss = FindObjectOfType<ScoreSystem>();
        if (ss != null)
        {
            ss.AddScore(-points);
            SpawnScorePopup(-points);
        }

        if (healthSystem != null)
            healthSystem.TakeDamage(points);

        Destroy(gameObject);
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
