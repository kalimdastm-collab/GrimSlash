using UnityEngine;

public class SwordSelectManager : MonoBehaviour
{
    public void SelectSword(int swordID)
    {
        PlayerPrefs.SetInt("SelectedSword", swordID);
        PlayerPrefs.Save();

        Debug.Log("Sword Selected: " + swordID);
    }
}
