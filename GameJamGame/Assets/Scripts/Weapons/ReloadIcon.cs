using System.Collections;
using UnityEngine;

public class ReloadIcon : MonoBehaviour
{
    [Header("Icon Settings")]
    [SerializeField] private SpriteRenderer iconRenderer; // Assign your reload icon sprite here
    [SerializeField] private float rotationSpeed = 360f;   // Degrees per second
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    private Transform playerTransform;
    private Coroutine rotateCoroutine;

    private void Awake()
    {
        if (iconRenderer != null)
            iconRenderer.gameObject.SetActive(false);
    }

    public void Initialize(Transform player)
    {
        playerTransform = player;
    }

    public void StartReloadIcon(float duration)
    {
        if (iconRenderer == null || playerTransform == null) return;

        iconRenderer.transform.position = playerTransform.position + offset;
        iconRenderer.gameObject.SetActive(true);

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateIcon(duration));
    }

    private IEnumerator RotateIcon(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (playerTransform != null)
                iconRenderer.transform.position = playerTransform.position + offset;

            iconRenderer.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        iconRenderer.gameObject.SetActive(false);
    }
}