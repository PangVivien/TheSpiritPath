using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject glowEffect;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip checkSFX;

    [Header("Player Settings")]
    [SerializeField] private float slowedSpeed = 5f; 

    private bool activated = false;
    private float originalSpeed;

    private void Awake()
    {
        if (glowEffect != null)
            glowEffect.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!activated)
        {
            activated = true;

            if (glowEffect != null)
                glowEffect.SetActive(true);

            if (audioSource != null && checkSFX != null)
                audioSource.PlayOneShot(checkSFX);
        }

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
        if (respawn != null)
            respawn.SetCheckpoint(transform);

        // Fill Health
        Health health = other.GetComponent<Health>();
        if (health != null)
            health.ResetHealth();
        // Fill Soul
        if (SoulManager.Instance != null)
            SoulManager.Instance.ResetSoul();

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            originalSpeed = playerController.speed;
            playerController.speed = slowedSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = originalSpeed;
        }
    }

}
