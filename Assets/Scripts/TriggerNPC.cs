using UnityEngine;
using System.Collections;

public class TriggerNPC : MonoBehaviour
{
    public GameObject npc;
    public float fadeOutDelay = 0.5f;
    public float disappearDelay = 3f;

    private Animator npcAnimator;
    private bool playerInside;
    private float originalPlayerSpeed;

    public Transform player;
    private PlayerController playerController;
    public float slowSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip npcSFX;

    [Header("Fade Settings")]
    public float fadeSpeed = 2f;
    private SpriteRenderer npcSprite;
    private bool isFadingOut = false;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        npcAnimator = npc.GetComponent<Animator>();
        npcSprite = npc.GetComponent<SpriteRenderer>();
        npc.SetActive(false);

        playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            originalPlayerSpeed = playerController.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Cancel FADE
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        isFadingOut = false;

        playerInside = true;
        if (playerController != null)
            playerController.speed = slowSpeed;

        // Show NPC
        if (npcSprite != null)
        {
            Color color = npcSprite.color;
            color.a = 1f;
            npcSprite.color = color;
        }

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

    private IEnumerator FadeOutAfterDelay()
    {
        isFadingOut = true;

        // Wait B4 Start Fade
        yield return new WaitForSeconds(disappearDelay);

        if (npcSprite != null)
        {
            float fadeDuration = 3f / fadeSpeed;
            float timer = 0f;
            Color color = npcSprite.color;
            float startAlpha = color.a;

            while (timer < fadeDuration && isFadingOut)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
                color.a = alpha;
                npcSprite.color = color;
                yield return null;
            }

            color.a = 0f;
            npcSprite.color = color;
        }

        // Disable NPC 
        isFadingOut = false;
        npc.SetActive(false);
        fadeCoroutine = null;
    }

    private IEnumerator DisableNPCAfterFade()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        npc.SetActive(false);
    }

    public void DisableNPC()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        isFadingOut = false;
        npc.SetActive(false);

        if (npcSprite != null)
        {
            Color color = npcSprite.color;
            color.a = 1f;
            npcSprite.color = color;
        }
    }
}
