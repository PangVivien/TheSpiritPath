using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Controller : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject dialogueCanvas;
    public TypeWriterr typeWriter;
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool playerInside = false;
    private bool dialogueActive = false;

    private PlayerController playerController;

    private void Awake()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    private void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            currentLine = 0;
            dialogueActive = true;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(true);

            // Stop Player Movement
            if (playerController != null)
            {
                playerController.SetPaused(true);
                playerController.SetDialogueActive(true); 
            }

            ShowNextLine();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            dialogueActive = false;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(false);

            // Resume Player Movement
            if (playerController != null)
            {
                playerController.SetPaused(false);
                playerController.SetDialogueActive(false);  
            }

            currentLine = 0;
        }
    }

    private void Update()
    {
        if (dialogueActive && playerInside)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame ||
                Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
            {
                OnPlayerInteract();
            }
        }
    }

    public void OnPlayerInteract()
    {
        if (!playerInside || dialogueCanvas == null) return;

        if (typeWriter.IsTyping())
        {
            typeWriter.FinishTyping();
        }
        else
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            if (!typeWriter.gameObject.activeInHierarchy)
                typeWriter.gameObject.SetActive(true);

            typeWriter.StartTyping(dialogueLines[currentLine]);
            currentLine++;
        }
        else
        {
            // Dialogue Finished
            dialogueActive = false;
            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(false);

            if (playerController != null)
            {
                playerController.SetPaused(false);
                playerController.SetDialogueActive(false);  
            }

            currentLine = 0;
        }
    }
}
