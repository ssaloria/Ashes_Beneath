using UnityEngine;

// @author: Enzo 18011129
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 90f;
    
    [Header("Audio Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    public float footstepRate = 0.5f;
    
    [Header("Flashlight Settings")]
    public Light flashlight;
    public float maxBatteryLife = 100f;
    public float batteryDrainRate = 1f;
    public KeyCode flashlightToggle = KeyCode.F;
    
    // Private variables
    private CharacterController characterController;
    private Camera playerCamera;
    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0;
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isGrounded = true;
    private float currentBattery;
    private bool flashlightOn = false;
    private float footstepTimer = 0f;
    private float originalHeight;
    private Vector3 originalCenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialize battery
        currentBattery = maxBatteryLife;
        
        // Store original controller dimensions for crouching
        originalHeight = characterController.height;
        originalCenter = characterController.center;
        
        // Make sure flashlight starts off
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleFlashlight();
        HandleFootsteps();
        HandleCrouch();
        
        // Debug info (remove in final build)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

     void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate camera up/down
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        
        // Rotate player body left/right
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Check if grounded
        isGrounded = characterController.isGrounded;
        
        // Get input
        currentInput = new Vector2((isCrouching ? 0.5f : 1f) * Input.GetAxis("Horizontal"), (isCrouching ? 0.5f : 1f) * Input.GetAxis("Vertical"));
        
        // Check for running
        isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
        
        // Calculate movement direction
        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        float movementDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.y + transform.TransformDirection(Vector3.right) * currentInput.x) * currentSpeed;
        moveDirection.y = movementDirectionY;
        
        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            moveDirection.y = jumpForce;
        }
        
        // Apply gravity
        if (!isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
        else if (moveDirection.y < 0)
        {
            moveDirection.y = -0.5f; // Small downward force to keep grounded
        }
        
        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
    }
    
    void HandleFlashlight()
    {
        if (flashlight == null) return;
        
        // Toggle flashlight
        if (Input.GetKeyDown(flashlightToggle))
        {
            flashlightOn = !flashlightOn;
            flashlight.enabled = flashlightOn;
        }
        
        // Drain battery when flashlight is on
        if (flashlightOn && currentBattery > 0)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            
            // Dim light as battery drains
            float batteryPercent = currentBattery / maxBatteryLife;
            flashlight.intensity = Mathf.Lerp(0.2f, 2f, batteryPercent);
            
            // Turn off when battery is dead
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                flashlightOn = false;
                flashlight.enabled = false;
            }
        }
    }
    
    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
        }
        
        // Adjust character controller height
        if (isCrouching)
        {
            characterController.height = originalHeight * 0.5f;
            characterController.center = new Vector3(originalCenter.x, originalCenter.y * 0.5f, originalCenter.z);
        }
        else
        {
            characterController.height = originalHeight;
            characterController.center = originalCenter;
        }
    }
    
    void HandleFootsteps()
    {
        if (!isGrounded || currentInput.magnitude == 0 || footstepAudioSource == null || footstepSounds.Length == 0)
            return;
        
        footstepTimer -= Time.deltaTime;
        
        if (footstepTimer <= 0)
        {
            // Adjust footstep rate based on movement speed
            float currentFootstepRate = footstepRate;
            if (isRunning)
                currentFootstepRate *= 0.6f;
            else if (isCrouching)
                currentFootstepRate *= 2f;
            
            footstepTimer = currentFootstepRate;
            
            // Play random footstep sound
            AudioClip footstepClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepAudioSource.PlayOneShot(footstepClip);
        }
    }
    
    // Public methods for UI or other scripts to access
    public float GetBatteryPercent()
    {
        return currentBattery / maxBatteryLife;
    }
    
    public bool IsFlashlightOn()
    {
        return flashlightOn;
    }
    
    public void AddBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0, maxBatteryLife);
    }
    
    // Draw battery info in editor for debugging
    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Battery: {currentBattery:F1}%");
            GUI.Label(new Rect(10, 30, 200, 20), $"Flashlight: {(flashlightOn ? "ON" : "OFF")}");
            GUI.Label(new Rect(10, 50, 200, 20), $"Crouching: {isCrouching}");
            GUI.Label(new Rect(10, 70, 200, 20), $"Running: {isRunning}");
        }
    }
}


