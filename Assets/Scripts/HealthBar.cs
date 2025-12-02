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

    [SerializeField] private float shakeDuration = 0.25f; 
    [SerializeField] private float shakeMagnitude = 15f;  
    [SerializeField] private float shakeSpeed = 0.02f;

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
            StopAllCoroutines();
            StartCoroutine(ShakeBar());
        }

        lastHealth = playerHealth.currentHealth;
    }

    private IEnumerator ShakeBar()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            Vector3 offset = new Vector3(
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude),
                0f
            );

            transform.localPosition = originalPos + offset;

            timer += shakeSpeed;
            yield return new WaitForSeconds(shakeSpeed);
        }

        transform.localPosition = originalPos;
    }
    private void ResetPosition()
    {
        transform.localPosition = originalPos;
    }
}
