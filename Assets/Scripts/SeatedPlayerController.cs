using UnityEngine;

public class SeatedPlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float maxLookUp = 60f;
    public float maxLookDown = -60f;
    public float maxLookLeft = 80f;
    public float maxLookRight = 80f;

    [Header("References")]
    public Transform cameraTransform;

    private float xRotation = 0f;  // Up/Down (pitch)
    private float yRotation = 0f;  // Left/Right (yaw)

    void Start()
    {
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // If camera not assigned, try to find Main Camera as child
        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // No position/rotation changes - character stays where it is in the scene
    }

    void Update()
    {
        // Handle mouse look
        HandleMouseLook();

        // Allow ESC to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Click to re-lock cursor
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void HandleMouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera up/down (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDown, maxLookUp);

        // Rotate camera left/right (yaw) - clamped for realistic head turning
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -maxLookLeft, maxLookRight);

        // Apply both rotations to camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}