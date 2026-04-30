using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPosition = target.position + offset;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}