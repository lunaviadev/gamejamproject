using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float deceleration = 100f;

    private Rigidbody2D rb;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.drag = 0f; // we’ll handle decel manually
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = input * moveSpeed;
        Vector2 velocity = rb.velocity;

        Vector2 velocityDiff = targetVelocity - velocity;

        float accelRate = (input.magnitude > 0.1f) ? acceleration : deceleration;

        Vector2 movement = velocity + velocityDiff.normalized * accelRate * Time.fixedDeltaTime;

        if (velocityDiff.magnitude < accelRate * Time.fixedDeltaTime)
            movement = targetVelocity;

        rb.velocity = movement;
    }
}
