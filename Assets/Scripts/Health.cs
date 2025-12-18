using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Health : MonoBehaviour
{
    public float startingHealth = 5f;
    public float currentHealth { get; private set; }

    PlayerRespawn respawn;

    private void Awake()
    {
        currentHealth = startingHealth;
        respawn = GetComponent<PlayerRespawn>();
    }

    public void TakeDamage(float _damage, Vector3 hitSource)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //Player Hurt
            Vector2 knockbackDir = (transform.position - hitSource).normalized;
            PlayerController.Instance.TakeDamage(1, knockbackDir);
        }
        else
        {
            //Player Dead
            PlayerController.Instance.Die();
            respawn.Respawn();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
    }

    public void ResetHealthAnimated(float duration = 1f)
    {
        StartCoroutine(AnimateHealthReset(duration));
    }

    private IEnumerator AnimateHealthReset(float duration)
    {
        float startHealth = currentHealth;
        float targetHealth = startingHealth;
        float timer = 0f;

        while (timer < duration)
        {
            currentHealth = Mathf.Lerp(startHealth, targetHealth, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        currentHealth = targetHealth;
    }
}
