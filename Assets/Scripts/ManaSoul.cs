using UnityEngine;

public class ManaSoul : MonoBehaviour
{
    public float maxSoul = 100f;
    public float currentSoul { get; private set; }

    private void Awake()
    {
        currentSoul = 0f;
    }

    public void AddSoul(float amount)
    {
        currentSoul = Mathf.Clamp(currentSoul + amount, 0, maxSoul);
    }

    public bool ConsumeSoul(float amount)
    {
        if (currentSoul < amount)
            return false;

        currentSoul -= amount;
        return true;
    }

    public bool HasSoul(float amount)
    {
        return currentSoul >= amount;
    }

    public void ResetSoul()
    {
        currentSoul = 0;
    }
}
