using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D customCursor;
    [SerializeField] private Vector2 clickposition = Vector2.zero;



    private void Start()
    {
        Cursor.SetCursor(customCursor, clickposition, CursorMode.Auto);
    }

}
