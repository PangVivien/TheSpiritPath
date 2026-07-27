using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerHeal : MonoBehaviour
{
    [Header("Heal Settings")]
    public float healAmount = 1f;
    public float healTime = 2f;      
    public bool isHealing = false;

    private PlayerController player;
    private Health health;

    [Header("Soul Settings")]
    public float soulCostPerHeal = 20f;
    public float soulDrainSpeed = 20f;

    private Coroutine rumbleCoroutine;
    [SerializeField] private CameraFollow cameraFollow;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        health = GetComponent<Health>();
    }

    public void Heal(InputAction.CallbackContext context)
    {
        // Button Hold
        if (context.started)
        {
            cameraFollow?.ZoomInForHeal();
            StartHeal();
        }

        // Button Release
        if (context.canceled)
        {
            cameraFollow?.ZoomOutAfterHeal();
            StopHeal();
        }
    }

    private void StartHeal()
    {
        if (isHealing) return;
        if (player.isDead) return;

        if (!player.IsGrounded()) return;

        StartCoroutine(HealRoutine());
    }

    private IEnumerator HealRoutine()
    {
        isHealing = true;

        float originalSpeed = player.speed;
        player.speed = 0f;

        player.animator.SetBool("isHealing", true);

        // Rumble Controller
        if (rumbleCoroutine != null) StopCoroutine(rumbleCoroutine);
        rumbleCoroutine = StartCoroutine(HealRumble());

        // Check Soul B4 Heal
        if (!SoulManager.Instance.HasSoul(soulCostPerHeal))
        {
            BreakHeal(originalSpeed);
            yield break;
        }


        float timer = 0f;

        while (timer < healTime)
        {
            // Heal Cancelled
            if (!isHealing)
            {
                BreakHeal(originalSpeed);
                yield break;
            }

            // Stop NO Soul
            if (!SoulManager.Instance.HasSoul(Time.deltaTime * soulDrainSpeed))
            {
                BreakHeal(originalSpeed);
                yield break;
            }

            SoulManager.Instance.DrainSoul(Time.deltaTime * soulDrainSpeed);

            timer += Time.deltaTime;
            yield return null;
        }

        // Heal Done
        health.Heal(healAmount);
        BreakHeal(originalSpeed);

        cameraFollow?.ZoomOutAfterHeal();
    }

    private void StopHeal()
    {
        if (!isHealing) return;

        isHealing = false;
        player.animator.SetBool("isHealing", false);

        if (rumbleCoroutine != null)
        {
            StopCoroutine(rumbleCoroutine);
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }

    private void BreakHeal(float originalSpeed)
    {
        isHealing = false;
        player.speed = originalSpeed;

        player.animator.SetBool("isHealing", false);
    }

    private IEnumerator HealRumble()
    {
        if (Gamepad.current == null)
            yield break;

        float timer = 0f;

        while (isHealing)
        {
            timer += Time.deltaTime;
            float intensity = Mathf.Clamp01(timer / healTime) * 0.5f;
            Gamepad.current.SetMotorSpeeds(intensity, intensity);
            yield return null;
        }

        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
