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

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {
            //Player Hurt
            Vector2 knockbackDir = (transform.position - PlayerController.Instance.transform.position).normalized;
            PlayerController.Instance.TakeDamage(1, knockbackDir);
        }
        else
        {
            //Player Dead
            PlayerController.Instance.Die();
        }
    }

    // For Testing
    //public void Damage(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        TakeDamage(1);
    //    }
    //}
}
