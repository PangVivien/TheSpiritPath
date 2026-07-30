using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ToBeContinue : MonoBehaviour
{
    public GameObject endingImage;
    public VideoPlayer endingVideo;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool triggered = false;
    private bool canProceed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        endingImage.SetActive(true);

        endingVideo.loopPointReached += OnVideoFinished;
        endingVideo.Play();

    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        endingVideo.loopPointReached -= OnVideoFinished;

        endPanel.SetActive(true);

        canProceed = true;

        InputSystem.onAnyButtonPress.CallOnce(_ =>
        {
            if (canProceed)
                SceneManager.LoadScene(mainMenuScene);
        });
    }
}
