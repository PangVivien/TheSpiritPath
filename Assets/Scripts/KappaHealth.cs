using UnityEngine;

public class KappaHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;


    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D enemyCollider;

    private bool isDead = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damagedSFX;
    [SerializeField] private AudioClip dieSFX;

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (audioSource != null && damagedSFX != null)
            audioSource.PlayOneShot(damagedSFX);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (audioSource != null)
            audioSource.Stop();

         if (audioSource != null && dieSFX != null)
            audioSource.PlayOneShot(dieSFX);

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        enemyCollider.enabled = false;

        animator.SetTrigger("death");
        // Destroy(gameObject);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
