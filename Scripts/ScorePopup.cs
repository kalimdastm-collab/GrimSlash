using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float floatSpeed = 2f;
    public float lifeTime = 1f;
    public Color negativeColor = Color.red;

    public void Setup(int score)
    {
        if (textMesh != null)
            textMesh.text = (score > 0 ? "+" : "") + score;
        if (score < 0)
            textMesh.color = negativeColor;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}
