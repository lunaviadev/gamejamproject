using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.1f;
    public Vector2 lookAheadOffset = new Vector2(1f, 0);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPos = target.position;


        Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
        if (playerRb)
        {
            targetPos += new Vector3(playerRb.linearVelocity.x * lookAheadOffset.x, playerRb.linearVelocity.y * lookAheadOffset.y, 0);
        }

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
