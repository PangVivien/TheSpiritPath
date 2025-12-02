using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float followSmoothTime = 0.15f;

    [Header("FreeLook Settings")]
    [SerializeField] private float lookRange = 2f; 
    private Vector2 rightStickInput; 
    private Vector3 lookOffset;

    [Header("Heal Settings")]
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;      
    private float currentZoom = 0f;
    private float targetZoom = 0f;

    // Update is called once per frame
    void Update()
    {
        // Heal Zoom
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);

        // Free-Look
        lookOffset = new Vector3(rightStickInput.x, rightStickInput.y, 0f) * lookRange;

        // Camera Position
        Vector3 targetPosition = target.position + offset + new Vector3(0f, 0f, currentZoom) + lookOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothTime);
    }

    public void UpdateRightStick(InputAction.CallbackContext context)
    {
        rightStickInput = context.ReadValue<Vector2>();

        rightStickInput.y = Mathf.Max(0f, rightStickInput.y);
    }

    public void Look(InputAction.CallbackContext context)
    {
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.UpdateRightStick(context);
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
