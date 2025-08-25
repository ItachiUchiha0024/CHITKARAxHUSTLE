using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The object that the camera should follow (in this case, the player).
    public float smoothing = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10);  // **IMPORTANT:  Set a default offset here!**
    [SerializeField]
    private float offsetDistance = 5f;
    [SerializeField]
    private float horizontalOffsetAmount = 2f; // Added for horizontal offset control
    [SerializeField]
    private float verticalOffsetAmount = 2f;   // Added for vertical offset control

    private Vector3 velocity = Vector3.zero;

    private void Awake()  // Add an Awake function
    {
        // **CRITICAL FIX:** Ensure the camera's Z position is set correctly *before* LateUpdate
        if (target != null)
        {
            transform.position = target.position + offset; // Set initial position
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target is null!  Please assign a target in the Inspector.");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Quaternion playerRotation = target.rotation;
        float playerRotationY = playerRotation.eulerAngles.y;

        // Calculate rotated offset, applying horizontal and vertical offsets
        Vector3 rotatedOffset = new Vector3(
            Mathf.Cos(playerRotationY * Mathf.Deg2Rad) * horizontalOffsetAmount + (Mathf.Sin(playerRotationY * Mathf.Deg2Rad) * offsetDistance), // Combine with horizontal
            Mathf.Sin(playerRotationY * Mathf.Deg2Rad) * offsetDistance + (Mathf.Cos(playerRotationY * Mathf.Deg2Rad) * verticalOffsetAmount),   // Combine with vertical
            offset.z // Keep original Z
        );
        desiredPosition = target.position + rotatedOffset;

        desiredPosition = target.position + rotatedOffset;


        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothing);
        transform.position = smoothedPosition;
    }
}