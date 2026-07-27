using UnityEngine;
using UnityEngine.UI;

public class SoulManager : MonoBehaviour
{
    public static SoulManager Instance;

    [Header("Soul")]
    public float maxSoul = 100f;
    public float currentSoul = 0f;

    [Header("UI")]
    public Image soulFill;

    private void Awake()
    {
        Instance = this;
        currentSoul = maxSoul;
    }

    private void Update()
    {
        if (soulFill != null)
            soulFill.fillAmount = currentSoul / maxSoul;
    }

    public void AddSoul(float amount)
    {
        currentSoul = Mathf.Clamp(currentSoul + amount, 0, maxSoul);
    }

    public bool HasSoul(float amount)
    {
        return currentSoul >= amount;
    }

    public void DrainSoul(float amount)
    {
        currentSoul = Mathf.Clamp(currentSoul - amount, 0, maxSoul);
    }

    public void ResetSoul()
    {
        currentSoul = maxSoul;
    }
    public void ResetSoulAnimated()
    {
        currentSoul = maxSoul;
    }
}
