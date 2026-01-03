using UnityEngine;

public class UmbrellaHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damagedSFX;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, Vector2 hitDir)
    {
        currentHealth -= damage;

        if (damagedSFX != null)
            audioSource.PlayOneShot(damagedSFX);

    }

}
