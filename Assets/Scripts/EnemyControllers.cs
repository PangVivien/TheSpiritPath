using UnityEngine;
using System.Collections;

public class EnemyControllers : MonoBehaviour
{
    //ENEMY KAPPA
    public Transform player;
    public float moveSpeed = 3f;
    public float detectionRange = 10f;

    [Header("Knockback Settings")]
    public Vector2 knockbackForce = new Vector2(5f, 5f);
    public float hurtDuration = 0.3f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hurtColor = Color.black;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D enemyCollider;
    private Collider2D playerCollider;

    private bool isFacingRight = true;
    private bool isFollowing = false;
    private bool isHurt = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null)
                Debug.LogError("SpriteRenderer");
        }

        originalColor = spriteRenderer.color;

        int attackLayerIndex = animator.GetLayerIndex("AttackLayer");
        if (attackLayerIndex != -1)
            animator.SetLayerWeight(attackLayerIndex, 1f);
        // animator.SetLayerWeight(animator.GetLayerIndex("AttackLayer"), 1f);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();
            // IGNORE COLLISION
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
        }
    }

    private void Update()
    {
        if (isHurt) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        isFollowing = distanceToPlayer <= detectionRange;

        if (isFollowing)
            FacePlayer();

        animator.SetFloat("speed", isFollowing ? moveSpeed : 0f);
    }

    private void FixedUpdate()
    {
        if (isHurt) return;

        if (isFollowing)
        {
            MoveTowardsPlayer();

            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            animator.SetFloat("speed", 0f);
        }

        // animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }


    private void MoveTowardsPlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private void FacePlayer()
    {
        if (player == null) return;

        float horizontal = player.position.x - transform.position.x;

        if (!isFacingRight && horizontal > 0f)
            Flip();
        else if (isFacingRight && horizontal < 0f)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public void TakeDamage(Vector2 hitDirection)
    {
        if (isHurt) return;

        isHurt = true;

        animator.SetTrigger("hurt");

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(hitDirection.x * knockbackForce.x, hitDirection.y * knockbackForce.y), ForceMode2D.Impulse);

        StartCoroutine(FlashHurt());

        StartCoroutine(HurtCooldown());
    }
    private IEnumerator FlashHurt()
    {
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator HurtCooldown()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }
}
