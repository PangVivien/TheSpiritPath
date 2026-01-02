using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class ToBeContinue : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool triggered = false;
    private bool canProceed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        endPanel.SetActive(true);
        StartCoroutine(WaitBeforeInput());
    }

    private System.Collections.IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(1.5f);
        canProceed = true;

        InputSystem.onAnyButtonPress.CallOnce(_ =>
        {
            if (canProceed)
                SceneManager.LoadScene(mainMenuScene);
        });
    }
}
