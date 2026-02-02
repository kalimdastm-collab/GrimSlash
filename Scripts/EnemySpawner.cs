using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;           
    public GameObject armoredEnemyPrefab;    
    public GameObject demonicSpiritPrefab;

    [Header("Spawn Settings")]
    public float spawnTime = 5f;             

    private float timer = 0f;

    void Awake()
    {
        if (FindObjectsOfType<EnemySpawner>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: enemyPrefab ست نشده!");
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        bool spawnLeft = Random.value < 0.5f;
        float zDistance = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(spawnLeft ? 0 : 1, 0, zDistance));
        spawnPos.z = 0;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        Vector3 scale = enemy.transform.localScale;
        scale.x = spawnLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        enemy.transform.localScale = scale;

        if (Time.timeSinceLevelLoad > 30f && armoredEnemyPrefab != null)
        {
            GameObject armored = Instantiate(armoredEnemyPrefab, spawnPos, Quaternion.identity);
            armored.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Vector3 armoredScale = armored.transform.localScale;
            armoredScale.x = spawnLeft ? -Mathf.Abs(armoredScale.x) : Mathf.Abs(armoredScale.x);
            armored.transform.localScale = armoredScale;
        }
        if (Time.timeSinceLevelLoad > 20f)
        {
            GameObject demon = Instantiate(demonicSpiritPrefab, spawnPos, Quaternion.identity);
            demon.transform.localScale = new Vector3(1f, 1f, 1f);
            Vector3 demonScale = demon.transform.localScale;
            demonScale.x = spawnLeft ? -Mathf.Abs(demonScale.x) : Mathf.Abs(demonScale.x);
            demon.transform.localScale = demonScale;
        }

    }
}
