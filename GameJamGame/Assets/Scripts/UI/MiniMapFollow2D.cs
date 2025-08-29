using UnityEngine;

public class MinimapFollow2D : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = player.position;
            newPos.z = transform.position.z; // Keep camera's Z position
            transform.position = newPos;
        }
    }
}
