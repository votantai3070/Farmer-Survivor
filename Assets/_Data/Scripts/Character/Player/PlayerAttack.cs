using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack Setting")]
    public PlayerController player;
    private PlayerControls controls;
    public PlayerStamina playerStamina;
    public PlayerAnimation playerAnimation;
    public Rigidbody2D rb;
    public Transform firePoint;
    public Bullet bullet;
    public Weapon weapon;
    bool isReloading;

    [SerializeField] private HotBarManager hotBarManager;

    private void Update()
    {
        CreateNewWeapon();
        //Debug.Log("Weapon Data: " + hotBarManager.currentWeaponData);
        //Debug.Log("weapon: " + weapon.weaponData);
    }

    private void CreateNewWeapon()
    {
        WeaponData weaponData = hotBarManager.currentWeaponData;

        weapon = new Weapon(weaponData);

        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Time.timeScale == 0f) return;

        AssignInputEvents();
    }

    void HandlePlayerAttack(Weapon weapon)
    {
        if (!weapon.CanShoot()) return;

        if (weapon.type == WeaponType.Rifle
        || weapon.type == WeaponType.Pistol
        || weapon.type == WeaponType.Shotgun)
        {
            bullet.Shot(1);
            SoundManager.instance.SetSoundState(SoundType.range);
        }

        else
        {
            playerStamina.UseStamina(weapon.stamina);
            SoundManager.instance.SetSoundState(SoundType.melee);
        }

        HandleShooting(weapon);
    }

    private void HandleShooting(Weapon weapon)
    {
        Vector3 mousePos = InputManager.Instance.GetMousePosition();

        Vector3 lookDir = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Shoot(angle, weapon);
    }

    void Shoot(float angle, Weapon weapon)
    {
        GameObject currentBulletPrefab = player.visuals.CurrentBulletPrefab(weapon);

        weapon.Shooting(angle, currentBulletPrefab, firePoint, weapon, bullet.currentAmmo);
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Fire.performed += ctx =>
        {
            if (!GameManager.isPaused)
                HandlePlayerAttack(weapon);
        };
        controls.Player.Reload.performed += ctx =>
        {
            if (weapon.type != WeaponType.Throw)
                bullet.Reload();
        };
    }
}
