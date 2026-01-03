using System;
using Unity.Cinemachine;
using UnityEngine;

public class HealObtain : MonoBehaviour
{
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private PlayerHeal obtainHeal;

    private void Awake()
    {
        if (glowEffect != null)
            glowEffect.SetActive(false);

        obtainHeal.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            glowEffect.SetActive(true);
            obtainHeal.enabled=true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
