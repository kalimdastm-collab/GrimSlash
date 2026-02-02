using UnityEngine;

public class DemonicSpiritSpawner : MonoBehaviour
{
    public GameObject demonicSpiritPrefab;

    [Header("Spawn Timing")]
    public float startAfterSeconds = 90f; 
    public float spawnInterval = 1.2f;    

    [Header("Spawn Area")]
    public float yMin = -4f;
    public float yMax = 4f;

    public float leftX = -6f;
    public float rightX = 6f;

    void Start()
    {
        InvokeRepeating(
            nameof(Spawn),
            startAfterSeconds,
            spawnInterval
        );
    }

    void Spawn()
    {
        float y = Random.Range(yMin, yMax);
        bool spawnLeft = Random.value > 0.5f;

        float x = spawnLeft ? leftX : rightX;

        Instantiate(
            demonicSpiritPrefab,
            new Vector3(x, y, 0f),
            Quaternion.identity
        );
    }
}
