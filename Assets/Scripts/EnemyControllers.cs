using UnityEngine;

public class EnemyControllers : MonoBehaviour
{
    public Transform player;          
    public float moveSpeed = 3f;
    public float detectionRange = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    // private Vector3 originalScale;
    private bool isFollowing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        isFollowing = distanceToPlayer <= detectionRange;
    }
    private void FixedUpdate()
    {
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

        if ((direction.x > 0 && transform.localScale.x > 0) || (direction.x < 0 && transform.localScale.x < 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }
}
