using UnityEngine;

public class KappaHealth : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;

    public Vector2 knockbackForce = new Vector2(5f, 5f);
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D enemyCollider;

    private bool isDead = false;

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

        rb.linearVelocity = new Vector2(hitDirection.x * knockbackForce.x, knockbackForce.y);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

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
