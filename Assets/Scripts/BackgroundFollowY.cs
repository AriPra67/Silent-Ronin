using UnityEngine;

public class BackgroundFollowY : MonoBehaviour
{
    public Transform target;
    public float parallaxX = 0.5f;
    public float zPosition = 10f;
    public float snapAmount = 0.05f;

    private Vector3 startPosition;
    private Vector3 targetStartPosition;

    void Start()
    {
        startPosition = transform.position;

        if (target != null)
        {
            targetStartPosition = target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float xMovement = (target.position.x - targetStartPosition.x) * parallaxX;
        float y = Mathf.Round(target.position.y / snapAmount) * snapAmount;

        transform.position = new Vector3(
            startPosition.x + xMovement,
            y,
            zPosition
        );
    }
}