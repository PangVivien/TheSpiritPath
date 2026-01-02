using System;
using UnityEngine;
using UnityEngine.Audio;

public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject FoxNPC;
    [SerializeField] private GameObject Ryokan;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        NPC.SetActive(false);
        FoxNPC.SetActive(true);
        Ryokan.SetActive(false);

        if (audioSource != null && doorSFX != null)
        {
            audioSource.PlayOneShot(doorSFX);
        }
    }
}
