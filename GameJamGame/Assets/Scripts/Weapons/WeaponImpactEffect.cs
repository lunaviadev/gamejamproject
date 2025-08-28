using UnityEngine;

public class WeaponImpactEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private float lifetime = 0.1f;
    private float timer = 0f;
    private Vector3 startScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    void OnEnable()
    {

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        transform.localScale = startScale * Random.Range(0.4f, 0.7f);
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifetime;
        transform.localScale = Vector3.Lerp(startScale * 1.5f, startScale, t);

        Color c = sr.color;
        c.a = Mathf.Lerp(1f, 0f, t);
        sr.color = c;

        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}
