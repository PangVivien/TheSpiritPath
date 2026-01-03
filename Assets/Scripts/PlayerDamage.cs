using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Vector2 hitDir = (collision.transform.position - transform.parent.position).normalized;
         
        
        // Enemy KAPPA

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


        // Enemy UMBRELLA

        UmbrellaHealth umbrella = collision.GetComponent<UmbrellaHealth>();
        if (umbrella != null)
            umbrella.TakeDamage(damage, hitDir);

        Enemy2Controller enemy2 =
            collision.GetComponent<Enemy2Controller>();
        if (enemy2 != null)
            enemy2.TakeDamage(hitDir);


        // Enemy LANTERN
        LanternHealth lantern = collision.GetComponent<LanternHealth>();
        if (lantern != null)
            lantern.TakeDamage(damage, hitDir);

        Enemy3Controller enemy3 = collision.GetComponent<Enemy3Controller>();
        if (enemy3 != null)
            enemy3.TakeDamage(hitDir);

    }
}
