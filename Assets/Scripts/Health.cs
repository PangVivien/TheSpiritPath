using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    [SerializeField]private float startingHealth;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
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
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
    }
}
