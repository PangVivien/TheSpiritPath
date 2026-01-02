using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Vector2 hitDir = (collision.transform.position - transform.parent.position).normalized;

        KappaHealth kappa = collision.GetComponent<KappaHealth>();
        if (kappa != null)
        {
            kappa.TakeDamage(damage, hitDir);
        }

        EnemyControllers enemy = collision.GetComponent<EnemyControllers>();
        if (enemy != null)
        {
            enemy.TakeDamage(hitDir); 
        }
    }
}
