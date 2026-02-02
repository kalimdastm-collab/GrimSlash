using UnityEngine;
using TMPro;

public class ScorePopupManager : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float lifeTime = 1.2f;
    public float startScale = 0.6f;
    public float endScale = 1f;

    private TextMeshPro text;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        transform.localScale = Vector3.one * startScale;
    }

    public void Setup(int value)
    {
        text.text = value > 0 ? "+" + value : value.ToString();
        text.color = value > 0 ? Color.green : Color.red;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.localScale = Vector3.Lerp(
            Vector3.one * startScale,
            Vector3.one * endScale,
            timer / lifeTime
        );

        Color c = text.color;
        c.a = Mathf.Lerp(1, 0, timer / lifeTime);
        text.color = c;

        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}
