using UnityEngine;
using static UnityEngine.Rigidbody2D;

public class GuidePoint : MonoBehaviour
{
    public int pointID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GuideManager.Instance.CurrentTarget != pointID)
            return;

        GuideManager.Instance.NextPoint();
        gameObject.SetActive(false);
    }
}
