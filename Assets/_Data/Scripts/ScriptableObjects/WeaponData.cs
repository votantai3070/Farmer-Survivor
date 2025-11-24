using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "TakeDamaged/WeaponData", order = 51)]
public class WeaponData : ScriptableObject
{
    [Header("═══ Basic Info ═══")]
    [Tooltip("Display name of the weapon")]
    public string weaponName;

    [Tooltip("Current weapon level (1-3)")]
    [Range(1, 3)]
    public int level;

    [TextArea(3, 5)]
    [Tooltip("Weapon description shown in UI")]
    public string description;

    public WeaponType weaponType;

    [Tooltip("Rarity color (Common, Rare, Epic, Legendary)")]
    public Color rareColor = Color.white;

    [Header("═══ Visuals ═══")]
    [Tooltip("Sprite path for in-game weapon model")]
    public string ingameSprite;

    [Tooltip("Sprite path for bullet/projectile")]
    public string bulletSprite;

    [Tooltip("Sprite path for UI icon")]
    public string UISprite;

    [Header("═══ Damage Stats ═══")]
    [Tooltip("Minimum damage per hit")]
    public int firstDamage = 10;

    [Tooltip("Maximum damage per hit")]
    public int lastDamage = 15;

    [Tooltip("Critical hit chance (0-100%)")]
    [Range(0f, 1f)]
    public float criticalChange = 5f;

    [Tooltip("Critical damage multiplier (1.5x = 150% damage)")]
    [Range(1f, 5f)]
    public float criticalDamage = 2f;

    [Header("═══ Range & Speed ═══")]
    [Tooltip("Maximum effective range in meters")]
    public float range = 50f;

    [Tooltip("Bullet speed (units per second)")]
    public float bulletSpeed = 100f;

    [Header("═══ Ammo & Magazine ═══")]
    [Tooltip("Bullets per magazine")]
    public int magazineSize = 30;

    [Tooltip("Bullets shot per click/burst")]
    public int bulletShotSize = 1;

    [Tooltip("Current ammo in magazine (runtime)")]
    [HideInInspector]
    public int currentAmmo;

    [Tooltip("Reserve ammo count (runtime)")]
    [HideInInspector]
    public int reserveAmmo;

    [Header("═══ Fire Rate & Timing ═══")]
    [Tooltip("Shots per second")]
    public float fireRate = 10f;

    [Tooltip("Delay before first shot after trigger")]
    public float attackDelayTime = 0.1f;

    [Tooltip("Time to reload in seconds")]
    public float reloadTime = 2f;

    [Tooltip("Hold to fire continuously?")]
    public bool isAutomatic = true;

    [Header("═══ Accuracy ═══")]
    [Tooltip("Bullet spread angle in degrees (0 = perfect accuracy)")]
    [Range(0f, 45f)]
    public float baseSpreadAngle = 2f;

    [Header("═══ Resource Cost ═══")]
    [Tooltip("Stamina consumed per shot")]
    public float stamina = 1f;

    // Helper methods
    [Header("═══ Calculated Stats (Read Only) ═══")]
    [SerializeField, Tooltip("Average damage per shot")]
    private float averageDamage;

    [SerializeField, Tooltip("Damage per second (theoretical)")]
    private float DPS;

    private void OnValidate()
    {
        // Auto-calculate stats when values change in Inspector
        averageDamage = (firstDamage + lastDamage) / 2f;
        DPS = averageDamage * fireRate;

        // Ensure min/max damage is logical
        if (lastDamage < firstDamage)
        {
            lastDamage = firstDamage;
        }
    }
}
