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
    public float speed = 5f;

    public float jumpingPower = 10f;
    public float fallMultiplier = 2.5f;        // Gravity Boost when Falling
    public float lowJumpMultiplier = 2f;       // Gravity Boost when Releasing
    public float coyoteTime = 0.1f;            // Short Buffer after Leave Ground
    private float coyoteTimeCounter;

    private bool isFacingRight = true;
    private bool jumpPressed;

    private bool canAttack = true;
    public float attackCooldown = 0.1f;

    public bool isDead = false;
    public Vector2 knockbackForce = new Vector2(20f, 20f);

    [Header("Damage Settings")]
    public float invincibilityDuration = 2; 
    [HideInInspector] public bool isInvincible = false;

    public static PlayerController Instance;
    // private PlayerHeal playerHeal; 

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

        // playerHeal = GetComponent<PlayerHeal>();
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

        // Rumble Controller
        StartCoroutine(Rumble(0.5f));

        // StartCoroutine(Respawn());
    }

    public void TakeDamage(int dmg, Vector2 hitDirection)
    {
        if (isDead || isInvincible) return;

        // Apply KnockBack
        rb.linearVelocity = new Vector2(hitDirection.x * knockbackForce.x, knockbackForce.y);

        animator.SetTrigger("KnockBack");
        StartCoroutine(HitFreeze());
        StartCoroutine(HitShake());

        // Rumble Controller
        StartCoroutine(Rumble(0.25f));

        // Add Invincibility
        StartCoroutine(InvincibilityCoroutine());
    }

    public void Attack(InputAction.CallbackContext context)
    {


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

    private IEnumerator HitFreeze()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1f;
    }
    private IEnumerator HitShake()
    {
        Vector3 originalPos = transform.localPosition;

        for (int i = 0; i < 6; i++)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * 0.05f;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private IEnumerator Rumble(float duration)
    {
        if (Gamepad.current == null)
            yield break;

        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;

            // EaseIn & EaseOut
            float intensity = Mathf.Sin(t * Mathf.PI);

            Gamepad.current.SetMotorSpeeds(intensity * 0.5f, intensity);

            timer += Time.deltaTime;
            yield return null;
        }

        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Color Glitch
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float flashInterval = 0.1f;
        float timer = 0f;

        bool isWhite = true;

        while (timer < invincibilityDuration)
        {
            sr.color = isWhite ? Color.black : Color.white;
            isWhite = !isWhite;

            timer += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }
        sr.color = Color.white;

        isInvincible = false;
    }

    public bool IsGrounded()
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

