using UnityEngine;

public class LanternHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damagedSFX;
    [SerializeField] private AudioClip dieSFX;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, Vector2 hitDir)
    {
        currentHealth -= damage;

        if (damagedSFX != null && audioSource != null)
            audioSource.PlayOneShot(damagedSFX);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dieSFX != null)
            AudioSource.PlayClipAtPoint(dieSFX, transform.position);

        if (animator != null)
            animator.SetTrigger("death");

        Destroy(gameObject, 0.5f);
    }
}
