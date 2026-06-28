using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerInstruction : MonoBehaviour
{
    public GameObject instructions;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (instructions != null)
            instructions.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (instructions != null)
        {
            Destroy(instructions);
            instructions = null;
        }
    }
}
