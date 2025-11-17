using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public InputManager inputManager;
    [SerializeField] private HotBarManager hotBarManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (spriteRenderer != null && hotBarManager.currentWeaponData != null)
            spriteRenderer.sprite = GameManager.instance.itemAtlas.GetSprite(hotBarManager.currentWeaponData.ingameSprite);
    }

    void Update()
    {

        if (Time.timeScale == 0f) return;
        GetRotateWeaponFollowMouse();

        if (hotBarManager != null)
            UpdateWeapon();
    }

    void UpdateWeapon()
    {
        if (hotBarManager.currentWeaponData == null) return;

        WeaponData weaponData = hotBarManager.currentWeaponData;

        spriteRenderer.sprite = GameManager.instance.itemAtlas.GetSprite(weaponData.ingameSprite);
    }

    void GetRotateWeaponFollowMouse()
    {
        Vector3 mousePos = inputManager.GetMousePosition();

        Vector3 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        FlipPlayerFollowMouse(angle);

        if (transform.parent.localScale.x < 0)
        {
            angle += 180f;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FlipPlayerFollowMouse(float angle)
    {
        if (angle > 90 || angle < -90)
            transform.parent.localScale = new Vector3(-1, 1, 1);
        else
            transform.parent.localScale = new Vector3(1, 1, 1);
    }
}
