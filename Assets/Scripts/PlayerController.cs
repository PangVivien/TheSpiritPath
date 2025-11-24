using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;

    private float horizontal;
    private float speed = 5f;

    public float jumpingPower = 10f;
    public float fallMultiplier = 2.5f;        // gravity boost when falling
    public float lowJumpMultiplier = 2f;       // gravity boost when releasing jump early
    public float coyoteTime = 0.1f;            // short buffer after leaving ground
    private float coyoteTimeCounter;

    private bool isFacingRight = true;
    private bool jumpPressed;

    private bool canAttack = true;
    public float attackCooldown = 0.1f;

    private bool isDead = false;
    public Vector2 knockbackForce = new Vector2(5f, 5f);

    public static PlayerController Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        animator.SetFloat("isWalking", Mathf.Abs(horizontal));

        bool grounded = IsGrounded();
        float yVel = rb.linearVelocity.y;

        if (grounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else
        {
            //if (yVel > 0)
            //    animator.SetBool("isJumping", true);
            //else if (yVel < 0)
            //    animator.SetBool("isFalling", true);

            if (yVel > 0.1f)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
            else if (yVel < -0.1f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }
        }


        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        // Force Falling
        if (yVel < -0.1f)
            jumpPressed = false;

        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Smoother Gravity
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.linearVelocity.y > 0 && !jumpPressed)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
            if (coyoteTimeCounter > 0f)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (context.canceled)
            jumpPressed = false;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; 

        animator.SetTrigger("isDead");

        GetComponent<PlayerInput>().enabled = false;

        // StartCoroutine(Respawn());
    }

    public void TakeDamage(int dmg, Vector2 hitDirection)
    {
        if (isDead) return;

        // Apply knockback
        rb.linearVelocity = Vector2.zero; // reset current velocity
        rb.AddForce(new Vector2(hitDirection.x * knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    animator.SetTrigger("isAttacking"); 
        //}

        if (context.performed && canAttack)
        {
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        canAttack = false;

        animator.SetTrigger("isAttacking");

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;    
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}

