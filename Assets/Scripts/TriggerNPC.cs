using UnityEngine;
using System.Collections;

public class TriggerNPC : MonoBehaviour
{
    public GameObject npc;
    public float fadeOutDelay = 0.5f;

    private Animator npcAnimator;
    private bool playerInside;
    private float originalPlayerSpeed;

    public Transform player;
    private PlayerController playerController;
    public float slowSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip npcSFX;

    private void Awake()
    {
        npcAnimator = npc.GetComponent<Animator>();
        npc.SetActive(false);

        playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            originalPlayerSpeed = playerController.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (playerController != null)
            playerController.speed = slowSpeed;

        npc.SetActive(true);
        npcAnimator.ResetTrigger("NPC_Character");

        FacePlayer();

        if (audioSource != null && npcSFX != null)
            audioSource.PlayOneShot(npcSFX);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!playerInside || !other.CompareTag("Player")) return;

        FacePlayer();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (playerController != null)
            playerController.speed = originalPlayerSpeed;

        npcAnimator.SetTrigger("NPC_Character");

        npc.SetActive(false);
    }

    private void FacePlayer()
    {
        Vector3 scale = npc.transform.localScale;

        if (player.position.x > npc.transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        npc.transform.localScale = scale;
    }

    private IEnumerator DisableNPCAfterFade()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        npc.SetActive(false);
    }

    public void DisableNPC()
    {
        npc.SetActive(false);
    }
}
