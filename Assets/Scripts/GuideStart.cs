using UnityEngine;

public class GuideStart : MonoBehaviour
{
    [SerializeField] private GameObject playerGuide;

    public void OnTriggerEnter2D(Collider2D other)
    {
        playerGuide.SetActive(true);
    }
}
