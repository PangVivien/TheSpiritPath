using UnityEngine;
using static UnityEngine.Rigidbody2D;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Instance;

    public Transform[] waypoints;

    public float speed = 10f;

    private int currentTarget = 0;
    public int CurrentTarget => currentTarget;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (currentTarget >= waypoints.Length)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            waypoints[currentTarget].position,
            speed * Time.deltaTime);
    }

    public void StartGuide()
    {
        currentTarget = 0;
        transform.position = waypoints[0].position;
        gameObject.SetActive(true);
    }

    public void NextPoint()
    {
        currentTarget++;

        if (currentTarget >= waypoints.Length)
        {
            gameObject.SetActive(false);
        }
    }
}
