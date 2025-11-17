using UnityEngine;

public class BulletMovement : TakeDamaged
{
    [Header("Range TakeDamaged Movement Setting")]
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer sr;
    private HotBarManager hotBarManager;

    [SerializeField] LayerMask enemyLayer;
    public Weapon weapon;

    [SerializeField] private GameObject hitEffectPrefab;

    private bool isEnemyWeapon = false;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hotBarManager = GameObject.Find("HotBarManager").GetComponent<HotBarManager>();
    }
    private void Start()
    {
        if (weaponData != null)
            sr.sprite = GameManager.instance.itemAtlas.GetSprite(weaponData.bulletSprite);
    }

    private void Update()
    {
        if (weapon != null)
            AutoFindEnemy();
    }

    public void InitializeWeapon(WeaponData weaponData) => weapon = new Weapon(weaponData);

    public void HandleBulletMovement(bool enemyWeapon, Weapon weapon)
    {
        if (this.weapon != weapon || this.weapon == null)
            this.weapon = weapon;

        if (weapon != null)
            if (enemyWeapon)
            {
                isEnemyWeapon = true;
                rb.linearVelocity = BulletEnemyDirection() * weapon.bulletSpeed;
            }
            else
            {
                isEnemyWeapon = false;
                rb.linearVelocity = BulletPlayerDirection() * weapon.bulletSpeed;
            }
    }

    private Vector3 BulletEnemyDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        return weapon.ApplySpread(direction);
    }
    private Vector3 BulletPlayerDirection()
    {
        Vector2 direction = ((Vector3)InputManager.Instance.GetMousePosition() - transform.position).normalized;
        return weapon.ApplySpread(direction);
    }

    void AutoFindEnemy()
    {
        if (hotBarManager.currentWeaponData == null) return;

        if (hotBarManager.currentWeaponData.level == 3
            && hotBarManager.currentWeaponData.weaponType == WeaponType.Throw)
        {

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 5f, enemyLayer);

            if (hitEnemies.Length > 0)
            {
                Transform nearestEnemy = null;
                float minDistance = Mathf.Infinity;

                foreach (var enemy in hitEnemies)
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = enemy.transform;
                    }
                }
                if (nearestEnemy != null)
                {
                    Vector2 direction = (nearestEnemy.position - transform.position).normalized;
                    rb.linearVelocity = direction * weaponData.bulletSpeed;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    rb.rotation = angle - 90f;
                }

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (collision.CompareTag("Player") && isEnemyWeapon)
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
            {
                CreateNewEffect(hitEffectPrefab);
                Attack(damagable);
            }
            ReturnToPool(gameObject);
            ReturnToPool(hitEffectPrefab);
        }

        else if (collision.CompareTag("Enemy") && !isEnemyWeapon)
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
            {
                CreateNewEffect(hitEffectPrefab);
                Attack(damagable);
            }
            ReturnToPool(gameObject);
            ReturnToPool(hitEffectPrefab);
        }
    }

    private void CreateNewEffect(GameObject effect)
    {
        GameObject newObjEffect = ObjectPool.instance.GetObject(effect);

        newObjEffect.transform.position = transform.position;
    }

    private void ReturnToPool(GameObject obj)
    {
        ObjectPool.instance.DelayReturnToPool(obj);
    }
}
