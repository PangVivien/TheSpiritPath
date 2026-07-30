using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class TriggerInstruction : MonoBehaviour
{
    public GameObject keyboardInstructions;
    public GameObject controllerInstructions;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;


        if (InputDeviceManager.Instance.UsingController)
        {
            controllerInstructions.SetActive(true);
            keyboardInstructions.SetActive(false);
        }
        else
        {
            keyboardInstructions.SetActive(true);
            controllerInstructions.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        keyboardInstructions.SetActive(false);
        controllerInstructions.SetActive(false);
    }
}
