using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    private void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);

        if (brightnessOverlay != null)
            SetBrightness(savedBrightness);

        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    public void SetBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = value;
            brightnessOverlay.color = c;
        }

        PlayerPrefs.SetFloat("Brightness", value);
    }
}
