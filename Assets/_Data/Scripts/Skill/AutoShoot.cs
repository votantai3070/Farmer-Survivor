using System.Collections;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;
    public GameObject bulletPrefab;
    public float detectionRadius = 10f;
    public float fireRate = 1f; // Bắn mỗi giây
    public float bulletSpeed = 10f;
    public int bulletCount = 1; // Số đạn bắn cùng lúc

    void Start()
    {
        StartCoroutine(AutoFireRoutine());
    }

    // Coroutine bắn tự động
    IEnumerator AutoFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            // Bắn nhiều đạn theo số lượng bulletCount
            for (int i = 0; i < bulletCount; i++)
            {
                Transform target = GetClosestEnemy();
                if (target != null)
                {
                    ShootBullet(target);
                }
            }
        }
    }

    // Tìm enemy gần nhất
    Transform GetClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.position, detectionRadius, LayerMask.GetMask("Enemy"));

        if (enemies.Length == 0)
            return null;

        Transform closest = null;
        float closestDistance = detectionRadius;

        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(player.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    // Bắn đạn từ player về phía enemy
    void ShootBullet(Transform target)
    {
        // Spawn bullet tại vị trí player
        GameObject bullet = ObjectPool.instance.GetObject(bulletPrefab);
        bullet.transform.position = player.position;

        // Tính hướng bay
        Vector2 direction = (target.position - player.position).normalized;

        // Xoay đạn theo hướng bay (2D)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Cho đạn bay
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        WeaponData bulletSpeed = bullet.GetComponent<CloseWeaponMovement>().weaponData;
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed.bulletSpeed;
        }

        ObjectPool.instance.DelayReturnToPool(bullet, 3f);
    }

    // Nâng cấp: Tăng số đạn bắn cùng lúc
    public void UpgradeBulletCount()
    {
        bulletCount++;
    }

    // Nâng cấp: Tăng tốc độ bắn
    public void UpgradeFireRate()
    {
        fireRate = Mathf.Max(0.1f, fireRate - 0.1f);
    }

}
