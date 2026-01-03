using UnityEngine;
using System.Collections;

public class Enemy2Controller : MonoBehaviour
{
    [Header("PlayerSetting")]
    public Transform player;
    public SpriteRenderer spriteRenderer;
    public float detectionRange = 6f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isFacingRight = true;
    private bool isAttacking = false;
    private Color originalColor;
    private bool isHurt;

    [Header("DashAttack")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float attackCooldown = 1.5f;

    [Header("Knockback")]
    public float knockbackForce = 4f;
    public float knockbackTime = 0.12f;
    public Color hitColor = Color.black;
    public int flashCount = 3;
    public float flashInterval = 0.08f;

    [Header("Health")]
    public float maxHealth = 3f;
    private float currentHealth;

    [Header("Death")]
    [SerializeField] private AudioClip dieSFX;
    private bool isDead = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;

        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isHurt) return;
        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            FacePlayer();
            StartCoroutine(AttackRoutine());
        }
    }
    private void FacePlayer()
    {
        float horizontal = player.position.x - transform.position.x;

        if (!isFacingRight && horizontal > 0f)
            Flip();
        else if (isFacingRight && horizontal < 0f)
            Flip();
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animator.SetTrigger("attack");
        if (audioSource != null && attackSFX != null)
            audioSource.PlayOneShot(attackSFX);

        yield return new WaitForSeconds(0.1f); 

        float dir = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    public void TakeDamage(Vector2 hitDir, float damage = 1f)
    {
        if (isDead || isHurt) return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        StartCoroutine(HurtRoutine(hitDir));
    }

    private IEnumerator HurtRoutine(Vector2 hitDir)
    {
        isHurt = true;

        // KnockBack
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-hitDir * knockbackForce, ForceMode2D.Impulse);

        // ColorGlitch
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
        }

        yield return new WaitForSeconds(knockbackTime);

        isHurt = false;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;

        animator.SetTrigger("death");
        animator.SetBool("isDead", true);

        if (dieSFX != null)
            AudioSource.PlayClipAtPoint(dieSFX, transform.position);

        Destroy(gameObject, 0.55f);
    }

}
