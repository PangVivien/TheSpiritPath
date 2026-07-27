using UnityEngine;
using System.Collections;

public class Enemy3Controller : MonoBehaviour
{
    //ENEMY LANTERN
    [Header("Player Setting")]
    private Animator animator;

    public float patrolSpeed = 2f;
    public float patrolRange = 3f;
    private Vector3 startPos;
    public Transform player;
    public float detectionRange = 5f;
    public float followSpeed = 4f;

    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public Collider2D damageCollider;

    public SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;
    private bool isFollowing = false;
    private bool isAttacking = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSFX;

    [Header("Collision Damage")]
    public int collisionDamage = 1;
    public float collisionCooldown = 0.5f;
    private float lastCollisionTime = 0f;

    private void Awake()
    {
        startPos = transform.position;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (player == null || isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isFollowing = distance <= detectionRange;

        if (isFollowing)
        {
            if (!isAttacking && distance <= attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            Patrol();
        }

    }

    private void Patrol()
    {
        float moveDir = isFacingRight ? 1f : -1f;
        transform.position += new Vector3(moveDir * patrolSpeed * Time.deltaTime, 0f, 0f);

        if (transform.position.x > startPos.x + patrolRange && isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x < startPos.x - patrolRange && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer != null)
            spriteRenderer.flipX = !isFacingRight;
    }

    private void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isFollowing = distance <= detectionRange;
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        transform.position += direction * followSpeed * Time.deltaTime;

        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            Flip();
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        Vector3 velocityBackup = transform.position;

        if (animator != null)
            animator.SetBool("isAttacking", true);
        if (damageCollider != null)
            damageCollider.enabled = true;

        if (audioSource != null && attackSFX != null)
            audioSource.PlayOneShot(attackSFX);

        yield return new WaitForSeconds(0.5f);

        if (damageCollider != null)
            damageCollider.enabled = false;
        if (animator != null)
            animator.SetBool("isAttacking", false);

        // Attack CoolDown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    public void TakeDamage(Vector2 hitDir)
    {
        StartCoroutine(HurtRoutine(hitDir));
    }

    private IEnumerator HurtRoutine(Vector2 hitDir)
    {
        // Flash Sprite
        if (spriteRenderer != null)
        {
            Color original = spriteRenderer.color;
            spriteRenderer.color = Color.black;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = original;
        }

        // KnockBack
        transform.position += -new Vector3(hitDir.x, hitDir.y, 0) * 0.5f;

        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (Time.time - lastCollisionTime >= collisionCooldown)
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

                if (playerController != null && !playerController.isDead && !playerController.isInvincible)
                {

                    Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                    playerController.TakeDamage(collisionDamage, hitDirection);

                    lastCollisionTime = Time.time;

                    Debug.Log("Enemy hit player on collision!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastCollisionTime >= collisionCooldown)
            {
                PlayerController playerController = other.GetComponent<PlayerController>();

                if (playerController != null && !playerController.isDead && !playerController.isInvincible)
                {
                    Vector2 hitDirection = (other.transform.position - transform.position).normalized;
                    playerController.TakeDamage(collisionDamage, hitDirection);
                    lastCollisionTime = Time.time;
                }
            }
        }
    }
}