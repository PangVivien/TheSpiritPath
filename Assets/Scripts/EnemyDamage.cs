using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Collider2D damageCollider;
    [SerializeField] private float Invincibility = 1f;
    [SerializeField] private float Damage;

    private bool isDisabled = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDisabled) return;

        if (collision.CompareTag("Player"))
        {
            Health hp = collision.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(Damage, transform.position);

                StartCoroutine(DisableDamageCollider());
            }
        }
    }

    private IEnumerator DisableDamageCollider()
    {
        isDisabled = true;
        damageCollider.enabled = false;

        yield return new WaitForSeconds(Invincibility);

        damageCollider.enabled = true;
        isDisabled = false;
    }
}
