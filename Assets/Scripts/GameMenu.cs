using UnityEngine;
using UnityEngine.UI;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuFirstButton);

            settingsPanel.SetActive(false);
            inSettings = false;
        }
    }


    public void StartGame()
    {
        SoundManagerSFX.Instance.PlayButtonClick();
        SceneFade.Instance.FadeToScene("SampleScene");
    }

    public void GameSettings()
    {
        SoundManagerSFX.Instance.PlayButtonClick();
        inSettings = true;
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void CloseSettings()
    {
        SoundManagerSFX.Instance.PlayButtonClick();
        inSettings = false;
        settingsPanel.SetActive(false);

        // Return to MainMenu
        EventSystem.current.SetSelectedGameObject(settingsButtonMenu);
    }

    public void ExitGame()
    {
        SoundManagerSFX.Instance.PlayButtonClick();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
