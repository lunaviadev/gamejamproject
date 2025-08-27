using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothTime = 0.05f; 
    public Vector2 lookAheadOffset = new Vector2(0.5f, 0.3f);
    public Vector2 lookAheadClamp = new Vector2(2f, 1f);

    [Header("Zoom Settings")]
    public float normalSize = 5f;
    public float zoomedOutSize = 6f;
    public float zoomSpeed = 5f;

    [Header("Screen Shake Settings")]
    public float shakeDecay = 1.5f;  

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private float targetSize;

    private float shakeIntensity = 0f;
    private Vector3 shakeOffset = Vector3.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow is designed for orthographic cameras.");
        }

        targetSize = normalSize;
        cam.orthographicSize = normalSize;
    }

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
        
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        
        if (shakeIntensity > 0f)
        {
            shakeOffset = Random.insideUnitCircle * shakeIntensity;
            transform.position += shakeOffset;
            shakeIntensity = Mathf.MoveTowards(shakeIntensity, 0f, shakeDecay * Time.deltaTime);
        }
    }
    
    public void SetZoom(bool rolling)
    {
        targetSize = rolling ? zoomedOutSize : normalSize;
    }
    
    public void Shake(float intensity)
    {
        shakeIntensity = Mathf.Max(shakeIntensity, intensity);
    }
}
