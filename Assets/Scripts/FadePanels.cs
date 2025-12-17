using UnityEngine;

public class FadePanels : MonoBehaviour
{
    public CanvasGroup fadePanel;

    void Start()
    {
        SceneFade.Instance.RegisterFadePanel(fadePanel);
    }
}
