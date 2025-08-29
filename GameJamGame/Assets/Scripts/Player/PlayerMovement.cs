using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private AudioSource BackroundMusicSource;
    [SerializeField] private AudioClip BackroundMusic;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float deceleration = 100f;
    public Animator animator;

    [Header("Dodge Roll Settings")]
    public float rollSpeed = 15f;
    public float rollDuration = 0.4f;
    public float rollCooldown = 0.5f;
    public Image[] cdIcons; 
    private int cdCount = 0;
    public int CDCount => cdCount; 


    [Header("References")]
    [SerializeField] private CameraFollow cameraFollow;

    private Rigidbody2D rb;
    private Vector2 input;
    public bool isRolling = false;
    private bool canRoll = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
    }

    private void Start()
    {
        if (BackroundMusicSource != null && BackroundMusic != null)
        {
            BackroundMusicSource.clip = BackroundMusic;
            BackroundMusicSource.loop = true;
            BackroundMusicSource.volume = 0.3f;
            BackroundMusicSource.Play();
            Debug.Log("Background music started playing.");
        }
    }
    private void Update()
    {
        if (DialogueManagerTMP.IsDialogueActive) return;

        if (!isRolling)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            input = new Vector2(x, y).normalized;
        }

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll && !isRolling && input != Vector2.zero)
            StartCoroutine(DoRoll());
    }

    private void FixedUpdate()
    {
        if (isRolling) return;

        Vector2 targetVelocity = input * moveSpeed;
        Vector2 velocity = rb.linearVelocity;

        Vector2 velocityDiff = targetVelocity - velocity;
        float accelRate = (input.magnitude > 0.1f) ? acceleration : deceleration;

        Vector2 movement = velocity + velocityDiff.normalized * accelRate * Time.fixedDeltaTime;

        if (velocityDiff.magnitude < accelRate * Time.fixedDeltaTime)
            movement = targetVelocity;

        rb.linearVelocity = movement;
    }

    private System.Collections.IEnumerator DoRoll()
    {
        isRolling = true;
        canRoll = false;

        if (isRolling == true)
        {
            animator.SetBool("IsRolling", true);
        }

        if (cameraFollow != null) cameraFollow.SetZoom(true);

        Vector2 rollDirection = input;
        float elapsed = 0f;

        gameObject.layer = LayerMask.NameToLayer("Invincible");

        while (elapsed < rollDuration)
        {
            rb.linearVelocity = rollDirection * rollSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");


        if (cameraFollow != null) cameraFollow.SetZoom(false);

        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
        animator.SetBool("IsRolling", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CDObject"))
        {

            CollectCD();

            Destroy(collision.gameObject);
        }
    }


    public void CollectCD()
    {
        if (cdCount < cdIcons.Length)
        {
            cdIcons[cdCount].enabled = true;
            cdCount++;
        }
    }
}
