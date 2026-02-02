using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image fillImage; 

    public void SetHealth(float percent)
    {
        percent = Mathf.Clamp01(percent);
        if(fillImage != null)
            fillImage.fillAmount = percent;
    }
}
