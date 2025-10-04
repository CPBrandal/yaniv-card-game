using UnityEngine;

public class SeatedPlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 200f;  // Increased from 100 to 200
    public float maxLookUp = 60f;
    public float maxLookDown = -60f;
    public float maxLookLeft = 90f;   // Increased from 80 to 90
    public float maxLookRight = 90f;  // Increased from 80 to 90
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;
    public float minZoom = 30f;  // More zoomed in (narrower FOV)
    public float maxZoom =75f;  // Default view (wider FOV)
    public float zoomSmoothness = 5f;  // How smooth the zoom transition is
    
    [Header("References")]
    public Transform cameraTransform;
    
    private float xRotation = 0f; // Up/Down (pitch)
    private float yRotation = 0f; // Left/Right (yaw)
    private Camera cam;
    private float targetFOV;
    
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
    
    // Get camera component
    cam = cameraTransform.GetComponent<Camera>();
    
    // Set initial FOV to maxZoom instead of reading from camera
    if (cam != null)
    {
        targetFOV = maxZoom;  // <-- Changed from cam.fieldOfView to maxZoom
        cam.fieldOfView = maxZoom;  // <-- Set camera to match
    }
}
    
    void Update()
    {
        // Handle mouse look
        HandleMouseLook();
        
        // Handle zoom
        HandleZoom();
        
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
    
    void HandleZoom()
    {
        if (cam == null)
            return;
        
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // Adjust target FOV based on scroll
        // Negative because scrolling up should zoom IN (lower FOV)
        targetFOV -= scrollInput * zoomSpeed;
        
        // Clamp the target FOV
        targetFOV = Mathf.Clamp(targetFOV, minZoom, maxZoom);
        
        // Smoothly interpolate to target FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSmoothness);
    }
}