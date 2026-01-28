using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Inscribed")]
    public float maximum;

    [Header("Dynamic")]
    public float current;
    public Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
    }

    void Update() {
        current += Time.deltaTime;
        current = Mathf.Clamp(current, 0, maximum);
        float fillAmount = current / maximum;
        slider.value = fillAmount;
    }
}
