using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint;

    PlayerController player;
    Rigidbody2D rb;
    [SerializeField] private Health playerHealth;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private float dieLength = 1f;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(respawnDelay);

        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        transform.position = currentCheckpoint.position;

        player.isDead = false;
        player.animator.Rebind();
        player.animator.Update(0f);
        player.GetComponent<PlayerInput>().enabled = true;

        yield return StartCoroutine(Fade(1f, 0f));

        if (playerHealth != null)
            playerHealth.ResetHealthAnimated(0.5f);

        if (SoulManager.Instance != null)
            SoulManager.Instance.ResetSoul();
    }

    private IEnumerator Fade(float from, float to)
    {
        yield return new WaitForSecondsRealtime(dieLength);

        float t = 0f;
        fadePanel.alpha = from;
        fadePanel.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        fadePanel.alpha = to;
        fadePanel.blocksRaycasts = to > 0f;
    }
}
