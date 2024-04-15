using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("The transform of the target the camera should follow.")]
    private Transform targetTransform;
    [SerializeField, Tooltip("The speed at which the camera moves towards its target position.")]
    private float smoothSpeed = 0.125f;

    [Tooltip("The offset from the target's position that the camera should maintain.")]
    [SerializeField] private Vector3 offset;

    [Header("Camera bounds")]
    [SerializeField, Tooltip("The minimum bounds that the camera should stay within.")]
    private Vector3 minCameraBounds;
    [SerializeField, Tooltip("The maximum bounds that the camera should stay within.")]
    private Vector3 maxCameraBounds;

    // Velocity used for smoothing the camera movement
    private Vector3 velocity = Vector3.zero;

#if UNITY_EDITOR
    public void OnEnable()
    {
        // If the target transform is not assigned, stop the game in the editor
        if (targetTransform == null)
        {
            Debug.LogError("PlayerCameraController: targetTransform is NULL, game will be stopped");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
    }
#endif

    /// <summary>
    /// Moves the camera to follow the target smoothly.
    /// </summary>
    public void CameraBehavior()
    {
        // If the target transform is not assigned, do nothing
        if (targetTransform == null)
            return;

        // Calculate the desired position for the camera
        Vector3 desiredPosition = targetTransform.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;

        // Clamp camera's position between min and max bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minCameraBounds.x, maxCameraBounds.x),
            Mathf.Clamp(transform.position.y, minCameraBounds.y, maxCameraBounds.y),
            Mathf.Clamp(transform.position.z, minCameraBounds.z, maxCameraBounds.z));
    }

#if UNITY_EDITOR
    [Header("TLS Control - only UNITY_EDITOR")]
    [SerializeField, Tooltip("Whether to display the minimum camera bounds in the editor.")]
    private bool showCameraMinBounds = false;
    [SerializeField, Tooltip("The color of the marker for minimum camera bounds.")]
    private Color minBoundsMarkColor = Color.green;
    [SerializeField, Tooltip("Whether to display the maximum camera bounds in the editor.")]
    private bool showCameraMaxBounds = false;
    [SerializeField, Tooltip("The color of the marker for maximum camera bounds.")]
    private Color maxBoundsMarkColor = Color.red;
    [SerializeField, Tooltip("The radius of the marker spheres.")]
    private float markRadius = 0.25f;
    void OnDrawGizmos()
    {
        if (showCameraMinBounds)
        {
            Gizmos.color = minBoundsMarkColor;
            Gizmos.DrawSphere(minCameraBounds, markRadius);
        }
        if (showCameraMaxBounds)
        {
            Gizmos.color = maxBoundsMarkColor;
            Gizmos.DrawSphere(maxCameraBounds, markRadius);
        }
    }
#endif
}
