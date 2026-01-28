using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float maximum = 60f;
    public float current;
    public Image mask;

    void Update() {
        current += Time.deltaTime;
        current = Mathf.Clamp(current, 0, maximum);
        float fillAmount = current / maximum;
        mask.fillAmount = fillAmount;
    }
}
