using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("EnemyAI")]
    public float idleMin = 1f;
    public float idleMax = 2.5f;

    public float walkMin = 1f;
    public float walkMax = 2f;

    public float walkSpeed = 2f;
    public float detectionRange = 3f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    private Transform player;
    private Vector2 moveDir;
    private bool isDead = false;
    private bool isChasing = false;
    private float lastAttackTime = 0;

    void Start()
    {
        currentHealth = maxHealth;

        player = PlayerController.Instance.transform;

        StartCoroutine(AI());
    }
    void Update()
    {
        if (isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            isChasing = true;
        }
        else if (dist > detectionRange + 1f)
        {
            isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            rb.linearVelocity = new Vector2(moveDir.x * walkSpeed, rb.linearVelocity.y);
    }

    private IEnumerator AI()
    {
        while (!isDead)
        {
            // Idle
            moveDir = Vector2.zero;
            animator?.Play("Idle");
            yield return new WaitForSeconds(Random.Range(idleMin, idleMax));

            // Walk
            float dir = Random.value < 0.5f ? -1 : 1;
            moveDir = new Vector2(dir, 0);
            transform.localScale = new Vector3(dir, 1, 1);
            animator?.Play("Walk");

            yield return new WaitForSeconds(Random.Range(walkMin, walkMax));
        }
    }

    private void ChasePlayer()
    {
        if (isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Face Player
        float dir = player.position.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);

        if (dist > attackRange)
        {
            // Move to Player
            rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);
            animator?.Play("Walk");
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            // Attack
            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                DoAttack();
            }
        }
    }

    private void DoAttack()
    {
        animator?.Play("Attack");

        // Damage Player
        var pc = PlayerController.Instance;

        if (!pc.isDead && !pc.isInvincible)
        {
            float dist = Vector2.Distance(transform.position, pc.transform.position);
            if (dist <= attackRange + 0.2f)
            {
                Vector2 hitDir = (pc.transform.position - transform.position).normalized;
                pc.TakeDamage(1, hitDir);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        //animator?.Play("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator?.Play("Die");
        Destroy(gameObject, 1f);
    }

    // Player Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(1);
        }
    }

    // Enemy Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = PlayerController.Instance;

            // Player attacked first ? enemy takes damage (handled in trigger)
            // Enemy hits first ? damage player
            if (!pc.isInvincible && !pc.isDead)
            {
                Vector2 dir = (pc.transform.position - transform.position).normalized;
                pc.TakeDamage(1, dir);
            }
        }
    }
}
