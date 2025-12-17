using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoundManagerSFX : MonoBehaviour
{
    public static SoundManagerSFX Instance;

    [Header("SFX Clips")]
    public AudioClip buttonClick;
    public AudioClip buttonNavigate;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent != null)
                transform.parent = null;

            DontDestroyOnLoad(gameObject);

            sfxSource = GetComponent<AudioSource>();
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();

            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return; 
        sfxSource.PlayOneShot(clip, volume);
    }

    // Game Menu
    public void PlayButtonClick()
    {
        if (buttonClick != null)
            PlaySFX(buttonClick);
    }

    public void PlayNavigationSFX()
    {
        if (buttonNavigate != null)
            PlaySFX(buttonNavigate);
    }
}
