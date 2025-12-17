using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneFade : MonoBehaviour
{
    public static SceneFade Instance;
    public float fadeDuration = 1f;
    private CanvasGroup fadeCanvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);        
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterFadePanel(CanvasGroup panel)
    {
        fadeCanvasGroup = panel;
        StartCoroutine(Fade(0f)); 
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        if (fadeCanvasGroup != null)
            yield return Fade(1f); 

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
