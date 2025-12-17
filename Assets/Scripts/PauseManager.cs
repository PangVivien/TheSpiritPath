using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    private bool isPaused = false;

    [SerializeField] private float buttonActionDelay = 0.5f;

    [SerializeField] private PlayerController player;

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        player.SetPaused(true);

        if (pauseButton != null)
            pauseButton.interactable = false;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        StartCoroutine(DelayedResume());
    }
    public void MainMenu()
    {
        if (!isPaused) return;

        StartCoroutine(DelayedMainMenu());
    }

    private IEnumerator DelayedResume()
    {
        yield return new WaitForSecondsRealtime(buttonActionDelay);

        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        player.SetPaused(false);

        if (pauseButton != null)
            pauseButton.interactable = true;
    }

    private IEnumerator DelayedMainMenu()
    {
        yield return new WaitForSecondsRealtime(buttonActionDelay);

        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}
