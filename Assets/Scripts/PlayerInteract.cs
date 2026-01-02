using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [HideInInspector] public NPC_Controller currentNPC;

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && currentNPC != null)
        {
            currentNPC.OnPlayerInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPC_Controller>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null;
        }
    }
}
