using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 0f, 180f); // degrees per second

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
