using UnityEngine;

public class SwordLoader : MonoBehaviour
{
    public SpriteRenderer swordRenderer;
    public Sprite[] swordSprites;

    void Start()
    {
        int swordID = PlayerPrefs.GetInt("SelectedSword", 0);
        swordRenderer.sprite = swordSprites[swordID];
    }
}
