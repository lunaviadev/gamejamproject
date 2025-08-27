using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.05f; 
    public Vector2 lookAheadOffset = new Vector2(0.5f, 0.3f);
    public Vector2 lookAheadClamp = new Vector2(2f, 1f);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPos = target.position;

        Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
        if (playerRb)
        {
            Vector2 clampedLook = new Vector2(
                Mathf.Clamp(playerRb.linearVelocity.x * lookAheadOffset.x, -lookAheadClamp.x, lookAheadClamp.x),
                Mathf.Clamp(playerRb.linearVelocity.y * lookAheadOffset.y, -lookAheadClamp.y, lookAheadClamp.y)
            );
            targetPos += new Vector3(clampedLook.x, clampedLook.y, 0);
        }

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
