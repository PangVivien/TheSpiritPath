using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class InputDeviceManager : MonoBehaviour
{
    public static InputDeviceManager Instance;

    public bool UsingController { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }


    void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }


    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!device.enabled)
            return;


        if (device is Gamepad)
        {
            UsingController = true;
        }
        else if (device is Keyboard || device is Mouse)
        {
            UsingController = false;
        }
    }
}
