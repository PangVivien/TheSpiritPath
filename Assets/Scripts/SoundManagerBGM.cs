using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerBGM : MonoBehaviour
{
    public static SoundManagerBGM Instance;

    [Header("Audio Clips")]
    public AudioClip menuBGM;
    public AudioClip gameBGM;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float menuVolume = 1f;
    [Range(0f, 1f)]
    public float gameVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent != null)
                transform.parent = null;

            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 1f;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("MainMenu"))
        {
            PlayBGM(menuBGM, menuVolume);
        }
        else
        {
            PlayBGM(gameBGM, gameVolume);
        }
    }

    private void PlayBGM(AudioClip newClip, float targetVolume)
    {
        if (audioSource.clip == newClip) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeBGMClip(newClip, targetVolume, 1f));
    }


    private IEnumerator FadeBGMClip(AudioClip newClip, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;

        // Fade out
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in to targetVolume
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
