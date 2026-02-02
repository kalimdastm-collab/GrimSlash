using UnityEngine;

public class SmallSpirit : Enemy
{
    public float speed = 2.5f;

    private bool spawnLeft;

    // فاصله‌ای که روح باید به تمپل برسد
    public float templeReachDistance = 0.5f;

    private Transform templeTransform;

    public void SetupFromSplit(bool fromLeft)
    {
        spawnLeft = fromLeft;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (fromLeft ? -1 : 1);
        transform.localScale = scale;

        EnemyMove move = GetComponent<EnemyMove>();
        if (move != null)
            move.SetDirection(fromLeft);

        SetSpeed(speed);
    }

    public void SetSpeed(float newSpeed)
    {
        EnemyMove move = GetComponent<EnemyMove>();
        if (move != null)
            move.SetSpeed(newSpeed);
    }

    protected override void Awake()
    {
        base.Awake();
        EnemyMove move = GetComponent<EnemyMove>();
        if (move != null)
            move.SetSpeed(speed);

        // پیدا کردن تمپل در صحنه
        GameObject templeObj = GameObject.FindGameObjectWithTag("Temple");
        if (templeObj != null)
            templeTransform = templeObj.transform;
    }

    void Update()
    {
        if (isDead) return;

        // چک فاصله به Temple
        if (templeTransform != null)
        {
            float distance = Vector2.Distance(transform.position, templeTransform.position);
            if (distance <= templeReachDistance)
            {
                HandleTempleHit();
            }
        }
    }

    private void HandleTempleHit()
    {
        if (isDead) return;
        isDead = true;

        // 🔹 کم کردن امتیاز و Health
        scoreSystem?.AddScore(-6);
        healthSystem?.TakeDamage(6);

        // 🔹 نمایش Score Popup
        if (scorePopupPrefab != null)
        {
            GameObject popup = Instantiate(
                scorePopupPrefab,
                transform.position + Vector3.up * 1.2f,
                Quaternion.identity
            );
            popup.GetComponent<ScorePopup>()?.Setup(-6);
        }

        // 🔹 محو شدن روح کوچک
        Destroy(gameObject);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        GetComponent<EnemyMove>()?.StopMoving();

        if (bloodEffect != null)
        {
            GameObject blood = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            Destroy(blood, 1.2f);
        }

        if (anim != null)
            anim.SetTrigger("DeathTrigger");

        // 🔹 اضافه شدن امتیاز هنگام مرگ
        scoreSystem?.AddScore(6);

        if (healthSystem != null)
        {
            if (healthSystem.currentHealth >= healthSystem.maxHealth)
                potionSystem?.AddPotion(6);
            else
                healthSystem.Heal(6f);
        }

        // 🔹 نمایش Score Popup هنگام مرگ
        if (scorePopupPrefab != null)
        {
            GameObject popup = Instantiate(
                scorePopupPrefab,
                transform.position + Vector3.up * 1.2f,
                Quaternion.identity
            );
            popup.GetComponent<ScorePopup>()?.Setup(6);
        }

        KillComboSystem.Instance?.RegisterKill(transform.position);
        Destroy(gameObject, 1.2f);
    }
}
