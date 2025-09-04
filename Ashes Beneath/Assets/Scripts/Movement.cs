using UnityEngine;
using UnityEngine.InputSystem; // NEW Input System

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look")]
    public float mouseSensitivity = 150f; // degrees/sec scale
    public bool lockCursor = true;

    // Input actions (created in code so you don't need an InputActionAsset)
    private InputAction moveAction;
    private InputAction lookAction;

    private CharacterController controller;
    private Transform body;          // the object we translate/rotate horizontally (parent if present)
    private float pitch = 0f;        // camera pitch (X rotation)
    private float verticalVelocity;  // simple gravity

    void Awake()
    {
        controller = GetComponent<CharacterController>() ?? GetComponentInParent<CharacterController>();
        body = controller ? controller.transform : (transform.parent ? transform.parent : transform);
    }

    void OnEnable()
    {
        // WASD + Left Stick
        moveAction = new InputAction("Move", type: InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddBinding("<Gamepad>/leftStick");

        // Mouse delta + Right Stick
        lookAction = new InputAction("Look", type: InputActionType.Value);
        lookAction.AddBinding("<Mouse>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick");

        moveAction.Enable();
        lookAction.Enable();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // --- Look ---
        Vector2 look = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

        // Yaw (left/right) on the body (usually the parent "Player")
        body.Rotate(Vector3.up, look.x, Space.World);

        // Pitch (up/down) on the camera itself
        pitch -= look.y;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        var e = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(pitch, 0f, 0f);

        // --- Move ---
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 planar = (body.right * move.x + body.forward * move.y);
        planar.y = 0f;
        Vector3 displacement = planar.normalized * moveSpeed;

        if (controller)
        {
            verticalVelocity += gravity * Time.deltaTime;
            Vector3 velocity = displacement + Vector3.up * verticalVelocity;
            controller.Move(velocity * Time.deltaTime);

            // simple ground stick (optional – requires CharacterController skin width setup)
            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
        }
        else
        {
            body.position += displacement * Time.deltaTime;
        }
    }
}
