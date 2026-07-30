using UnityEngine;

public class TriggerSignBoard : MonoBehaviour
{
    [SerializeField] private GameObject signBoard;

    private void OnTriggerEnter2D(Collider2D other)
    {
        signBoard.SetActive(true);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        signBoard.SetActive(false);

    }
}
