using UnityEngine;
using TMPro;

public class ComboPopupSpawner : MonoBehaviour
{
    public static ComboPopupSpawner Instance;

    [Header("Prefab")]
    public GameObject comboPopupPrefab; 

    [Header("Settings")]
    public float floatSpeed = 2f;
    public float lifeTime = 1.2f;
    public Vector3 offset = new Vector3(0, 1.8f, 0);

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Spawn(string text, Vector3 position)
    {
        if (comboPopupPrefab == null) return;

        GameObject popup = Instantiate(
            comboPopupPrefab,
            position + offset,
            Quaternion.identity
        );

        TextMeshPro tmp = popup.GetComponent<TextMeshPro>();
        if (tmp != null)
            tmp.text = text;
        popup.AddComponent<ComboPopupBehavior>().Setup(floatSpeed, lifeTime);
    }
}

public class ComboPopupBehavior : MonoBehaviour
{
    private float floatSpeed;
    private float lifeTime;

    public void Setup(float floatSpeed, float lifeTime)
    {
        this.floatSpeed = floatSpeed;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}
