using UnityEngine;
using System.Collections;
using TMPro;

public class TypeWriterr : MonoBehaviour
{
    public float typeSpeed = 0.05f;
    private TMP_Text textMesh;
    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private NPC_Controller npcController;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
        textMesh.text = "";

        // Find NPC Controller
        npcController = GetComponentInParent<NPC_Controller>();
        if (npcController == null)
            npcController = FindObjectOfType<NPC_Controller>();
    }

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        fullText = text;
        textMesh.text = "";
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        for (int i = 1; i <= fullText.Length; i++)
        {
            textMesh.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textMesh.text = fullText;
        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}