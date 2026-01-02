using UnityEngine;
using System.Collections;

public class EnemyControllers : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (isHurt) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        isFollowing = distanceToPlayer <= detectionRange;

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
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Face Player 
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x >= 0 ? 1f : -1f);
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
