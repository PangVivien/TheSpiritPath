using UnityEngine;
using System.Collections;

public class UmbrellaController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform player;
    public float detectionRange = 5f;
    public float attackCooldown = 2f;
    public float lungeDistance = 1f;
    public float lungeSpeed = 5f;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Combat")]
    public int maxHealth = 3;
    private int currentHealth;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hurtColor = Color.black;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;

    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(Attack());
        }
        else if (!isAttacking)
        {
            animator.Play("Idle");
        }

        if (!isAttacking)
            FacePlayer();
    }

    private void FacePlayer()
    {
        if (player == null) return;

        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
            (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        FacePlayer();

        // Lunge Forward
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPos = (Vector2)transform.position + new Vector2(Mathf.Sign(direction.x) * lungeDistance, 0f);

        float elapsed = 0f;
        float duration = lungeDistance / lungeSpeed;
        Vector2 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        animator.SetTrigger("attack");

        yield return new WaitForSeconds(0.8f); 

        isAttacking = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;

        StartCoroutine(FlashHurt());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashHurt()
    {
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("death");

        Destroy(gameObject, 1f); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TakeDamage();
        }
    }
}