using UnityEngine.SceneManagement;
using UnityEngine;

public class startScene : MonoBehaviour
{

    [Header("Panels")]
    public GameObject infoPanel;
    public GameObject swordPanel;

    void Start()
    {
        infoPanel.SetActive(false);
        swordPanel.SetActive(false);
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
        swordPanel.SetActive(false);
    }

    public void OpenSwordPanel()
    {
        swordPanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        infoPanel.SetActive(false);
        swordPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
