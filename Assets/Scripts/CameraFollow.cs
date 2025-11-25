using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.25f;

    [SerializeField] private Transform target;

    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;      
    private float currentZoom = 0f;
    private float targetZoom = 0f;

    // Update is called once per frame
    void Update()
    {
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);

        Vector3 targetPosition = target.position + offset + new Vector3(0f, 0f, currentZoom);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ZoomInForHeal()
    {
        targetZoom = zoomAmount; 
    }

    public void ZoomOutAfterHeal()
    {
        targetZoom = 0f; 
    }
}
