using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           KappaHealth enemy = collision.GetComponent<KappaHealth>();
            if (enemy != null)
            {
                Vector2 hitDir =
                    (collision.transform.position - transform.parent.position).normalized;

                enemy.TakeDamage(damage, hitDir);
            }
        }
    }
}
