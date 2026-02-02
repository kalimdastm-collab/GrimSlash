using UnityEngine;

public class KillComboSystem : MonoBehaviour
{
    public static KillComboSystem Instance;

    [Header("Combo Settings")]
    public float comboTimeWindow = 1f; 
    public GameObject comboPopupPrefab; 

    private float lastKillTime;
    private int killCount;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterKill(Vector3 position)
    {
        if (Time.time - lastKillTime <= comboTimeWindow)
        {
            killCount++;
        }
        else
        {
            killCount = 1;
        }

        lastKillTime = Time.time;

        if (killCount >= 2)
        {
            string text = "";

            if (killCount == 2) text = "DOUBLE KILL!";
            else if (killCount == 3) text = "TRIPLE KILL!";
            else text = "MULTI KILL!";

            ComboPopupSpawner.Instance?.Spawn(text, position);
        }
    }

    void ShowComboPopup(string text, Vector3 position)
    {
        if (comboPopupPrefab != null)
        {
            GameObject popup = Instantiate(
                comboPopupPrefab,
                position + Vector3.up * 2f, 
                Quaternion.identity
            );

            ComboPopupSpawner.Instance.Spawn(text, position + Vector3.up * 2f);
        }
    }
}
