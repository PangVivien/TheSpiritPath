using UnityEngine;
using System.Collections;

public class NPC_Controller : MonoBehaviour
{
    [Header("Dialogue")]
    public GameObject dialogueCanvas;
    public TypeWriterr typeWriter;
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool playerInside = false;

    private void Awake()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            currentLine = 0;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(true);

            ShowNextLine();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(false);

            currentLine = 0;
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
            //currentLine++;
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
            typeWriter.StartTyping(dialogueLines[currentLine]);
            currentLine++;
        }
    }
}
