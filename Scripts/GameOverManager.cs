using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("Scene Settings")]
    public string gameOverSceneName = "GameOver";
    public float delayBeforeLoad = 0.1f;

    [Header("Audio")]
    public AudioClip gameOverSFX;

    private float survivalTime = 0f;

    [Header("References")]
    public HealthSystem healthSystem;
    public GameObject bloodFrame;

    [Header("Camera Shake")]
    public Camera mainCamera;
    public float shakeMagnitude = 0.05f;

    private bool isLowHealth = false;
    private bool isGameOver = false;
    private Vector3 cameraOriginalPos;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        Debug.Log("✅ GameOverManager Start called");
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (mainCamera == null)
            mainCamera = Camera.main;

        cameraOriginalPos = mainCamera.transform.position;

        if (bloodFrame != null)
            bloodFrame.SetActive(false);

    }

    void Update()
    {
        if (healthSystem == null)
        {
            Debug.LogError(" HealthSystem وصل نشده!");
            return;
        }

        if (isGameOver) return;

        survivalTime += Time.deltaTime;

        Debug.Log(" Current Health: " + healthSystem.currentHealth);


        if (healthSystem.currentHealth <= 0)
        {
            Debug.Log("GAME OVER TRIGGERED");
            TriggerGameOver();
            return;
        }

        if (healthSystem.currentHealth <= 20 && !isLowHealth)
        {
            Debug.Log("⚠ LOW HEALTH ACTIVE");
            ActivateLowHealthEffect();
        }
        else if (healthSystem.currentHealth > 20 && isLowHealth)
        {
            Debug.Log("✅ HEALTH BACK TO NORMAL");
            DeactivateLowHealthEffect();
        }
    }


    void ActivateLowHealthEffect()
    {
        isLowHealth = true;

        if (bloodFrame != null)
            bloodFrame.SetActive(true);

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCamera());
    }

    void DeactivateLowHealthEffect()
    {
        isLowHealth = false;

        if (bloodFrame != null)
            bloodFrame.SetActive(false);

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        mainCamera.transform.position = cameraOriginalPos;
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        PlayerPrefs.SetFloat("SavedHealth", healthSystem.currentHealth);
        PlayerPrefs.SetFloat("SavedTime", survivalTime);
        PlayerPrefs.SetInt("SavedScore", Mathf.FloorToInt(survivalTime)); 
        PlayerPrefs.Save();

        int finalScore = Mathf.FloorToInt(survivalTime);
        PlayerPrefs.SetInt("FinalScore", finalScore);
        PlayerPrefs.Save();

        if (gameOverSFX != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);
        }

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        mainCamera.transform.position = cameraOriginalPos;
        healthSystem.currentHealth = PlayerPrefs.GetFloat("SavedHealth", healthSystem.maxHealth);
        survivalTime = PlayerPrefs.GetFloat("SavedTime", 0f);


        StartCoroutine(LoadGameOverScene());
    }

    IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(gameOverSceneName);
    }

    IEnumerator ShakeCamera()
    {
        while (isLowHealth && !isGameOver)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.position = cameraOriginalPos + new Vector3(x, y, 0);

            yield return null;
        }

        mainCamera.transform.position = cameraOriginalPos;
    }

}
