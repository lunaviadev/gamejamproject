using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTransition2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("End Scene");
    }
}
