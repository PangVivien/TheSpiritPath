using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuFirstButton;

    [SerializeField] private GameObject settingsFirstButton;
    [SerializeField] private GameObject settingsButtonMenu;

    private bool inSettings = false;

    void Update()
    {
        if (inSettings)
        {
            if (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame)
                CloseSettings();

            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                CloseSettings();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GameSettings()
    {
        inSettings = true;
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void CloseSettings()
    {
        inSettings = false;
        settingsPanel.SetActive(false);

        // Return to MainMenu
        EventSystem.current.SetSelectedGameObject(settingsButtonMenu);
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}
