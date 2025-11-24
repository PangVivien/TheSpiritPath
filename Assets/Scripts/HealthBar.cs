using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private Vector3 originalPos;
    private float lastHealth;

    private void Start()
    {
        originalPos = transform.localPosition;
        lastHealth = playerHealth.currentHealth;

        totalHealthBar.fillAmount = playerHealth.currentHealth / 5;
    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 5f;

        if (playerHealth.currentHealth < lastHealth)
        {
            float magnitude = 10f;
            transform.localPosition = originalPos + new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), 0);

            Invoke(nameof(ResetPosition), 0.08f);
        }

        lastHealth = playerHealth.currentHealth;
    }

    private void ResetPosition()
    {
        transform.localPosition = originalPos;
    }
}
