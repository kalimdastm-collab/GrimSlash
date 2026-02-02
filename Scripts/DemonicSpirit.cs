using UnityEngine;

public class DemonicSpirit : Enemy
{
    [Header("Movement")]
    public float speed = 4f;

    [Header("Split")]
    public GameObject smallSpiritPrefab;
    public GameObject splitEffectPrefab;
    public float splitOffset = 0.6f;

    private bool spawnLeftSpirit;
    private bool spawnLeft;

    protected override void Awake()
    {
        base.Awake();

        EnemyMove move = GetComponent<EnemyMove>();
        if (move != null)
            move.SetSpeed(speed);

        spawnLeftSpirit = transform.localScale.x < 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Temple"))
        {
            ReachTarget();
        }
    }

    public override void OnSlashed(Vector2 slashDir)
    {
        Split();
    }

    void Split()
    {
        if (splitEffectPrefab != null)
        {
            GameObject fx = Instantiate(splitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 1.2f);
        }

        SpawnSmallSpirit(Vector3.left * splitOffset);
        SpawnSmallSpirit(Vector3.right * splitOffset);

        Destroy(gameObject);
    }

    void SpawnSmallSpirit(Vector3 offset)
    {
        GameObject spirit = Instantiate(
            smallSpiritPrefab,
            transform.position + offset,
            Quaternion.identity
        );

        Vector3 scale = spirit.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(transform.localScale.x);
        spirit.transform.localScale = scale;

        EnemyMove move = spirit.GetComponent<EnemyMove>();
        if (move != null)
        {
            move.SetSpeed(speed * 2f);
        }
    }

    public override void ReachTarget()
    {
        if (isDead) return;
        isDead = true;

        int points = -12;
        scoreSystem?.AddScore(points);
        SpawnScorePopup(points);

        healthSystem?.TakeDamage(12);

        KillComboSystem.Instance?.RegisterKill(transform.position);
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
