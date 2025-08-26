using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public SpriteRenderer weaponSpriteRenderer; 
    private Camera mainCam;
    private Vector2 currentOffset;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!weaponSpriteRenderer.enabled) return;

        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Apply offset
        Vector3 offset = Quaternion.Euler(0, 0, angle) * currentOffset;
        weaponSpriteRenderer.transform.localPosition = offset;
        
        if (angle > 90 || angle < -90)
            weaponSpriteRenderer.flipY = true;
        else
            weaponSpriteRenderer.flipY = false;
    }

    public void EquipWeaponSprite(Sprite weaponSprite, Vector2 offset)
    {
        weaponSpriteRenderer.sprite = weaponSprite;
        weaponSpriteRenderer.enabled = true;
        currentOffset = offset;
    }

    public void UnequipWeapon()
    {
        weaponSpriteRenderer.sprite = null;
        weaponSpriteRenderer.enabled = false;
        currentOffset = Vector2.zero;
    }
}
