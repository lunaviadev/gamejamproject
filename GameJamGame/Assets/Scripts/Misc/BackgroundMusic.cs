using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.3f;
    public bool loop = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.playOnAwake = false;
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("No music clip assigned to BackgroundMusic on " + gameObject.name);
        }
    }
}
