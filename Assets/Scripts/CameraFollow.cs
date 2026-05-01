using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    public float minX = -9999f;
    public float maxX = 9999f;
    public float minY = -9999f;
    public float maxY = 9999f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPosition = target.position + offset;

        // Clamp camera bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // IMPORTANT: keep original Z stable (prevents weird 2D/3D floating issues)
        newPosition.z = offset.z;

        transform.position = newPosition;
    }
}