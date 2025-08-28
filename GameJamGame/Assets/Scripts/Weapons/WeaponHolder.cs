using UnityEngine;
using System.Collections.Generic;

public class WeaponHolder : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSprite
    {
        public string weaponName;
        public SpriteRenderer spriteRenderer;
    }
    public List<WeaponSprite> weaponSprites = new List<WeaponSprite>();
    public WeaponSprite activeWeaponSprite;

    private void FixedUpdate()
    {
        RotateToMouse();
    }

    public void EquipWeapon(string weaponName)
    {

        foreach (var ws in weaponSprites)
        {
            ws.spriteRenderer.enabled = false;
        }

        activeWeaponSprite = weaponSprites.Find(w => w.weaponName == weaponName);
        if (activeWeaponSprite != null)
        {
            activeWeaponSprite.spriteRenderer.enabled = true;
        }
        else
        {
            Debug.LogWarning("No weapon sprite found for: " + weaponName);
        }
    }

    public void UnequipWeapon()
    {
        if (activeWeaponSprite != null)
        {
            activeWeaponSprite.spriteRenderer.enabled = false;
            activeWeaponSprite = null;
        }
    }

        private void RotateToMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
