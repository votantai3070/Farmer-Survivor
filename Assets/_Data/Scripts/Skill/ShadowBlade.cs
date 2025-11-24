using System.Collections;
using UnityEngine;

public class ShadowBlade : Skill
{
    [SerializeField] Transform player;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] int cloneCount = 2; // Số kiếm phân thân
    [SerializeField] float cloneDelay = 0.1f; // Delay giữa các clone
    [SerializeField] float cloneAlpha = 0.5f; // Độ trong suốt của clone
    [SerializeField] float fireRate = 1f;
    [SerializeField] private float detectionRadius = 5f;

    // Biến upgrade cho multi-direction
    [SerializeField] int numberOfDirections = 1; // Số hướng bắn (nâng cấp được)
    [SerializeField] float spreadAngle = 45f; // Góc giữa các hướng

    void Start()
    {
        StartCoroutine(ShootWithClones());
    }

    IEnumerator ShootWithClones()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            Transform target = GetClosestEnemy();
            if (target != null)
            {
                // Bắn kiếm theo nhiều hướng
                for (int dir = 0; dir < numberOfDirections; dir++)
                {
                    float angleOffset = CalculateAngleOffset(dir);

                    // Bắn kiếm chính
                    ShootSword(target, 1f, angleOffset);

                    // Bắn kiếm phân thân với delay
                    StartCoroutine(ShootClonesForDirection(target, angleOffset));
                }
            }
        }
    }

    IEnumerator ShootClonesForDirection(Transform target, float angleOffset)
    {
        for (int i = 0; i < cloneCount; i++)
        {
            yield return new WaitForSeconds(cloneDelay);
            ShootSword(target, cloneAlpha, angleOffset);
        }
    }

    // Tính góc offset cho mỗi hướng
    float CalculateAngleOffset(int index)
    {
        if (numberOfDirections == 1)
            return 0f;

        // Tính góc spread đều
        float totalSpread = spreadAngle * (numberOfDirections - 1);
        float startAngle = -totalSpread / 2f;
        return startAngle + (spreadAngle * index);
    }

    void ShootSword(Transform target, float alpha, float angleOffset)
    {
        GameObject sword = ObjectPool.instance.GetObject(swordPrefab);

        Collider2D col = sword.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Làm mờ kiếm phân thân
        SpriteRenderer sr = sword.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }

        // Tính hướng ban đầu
        Vector2 baseDirection = (target.position - player.position).normalized;

        float radius = 1f;

        sword.transform.position = (Vector2)player.position + baseDirection * radius;

        // Áp dụng góc offset
        float angleInRadians = angleOffset * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(
            baseDirection.x * Mathf.Cos(angleInRadians) - baseDirection.y * Mathf.Sin(angleInRadians),
            baseDirection.x * Mathf.Sin(angleInRadians) + baseDirection.y * Mathf.Cos(angleInRadians)
        );

        // Xoay kiếm theo hướng
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sword.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Rigidbody2D rb = sword.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f;
        }

        StartCoroutine(EnableColliderAfterDelay(col, 0.15f));
        ObjectPool.instance.DelayReturnToPool(sword, 3f);
    }

    IEnumerator EnableColliderAfterDelay(Collider2D col, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null) col.enabled = true;
    }

    Transform GetClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.position, detectionRadius, LayerMask.GetMask("Enemy"));

        if (enemies.Length == 0) return null;

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(player.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    // Upgrade số lượng clone
    public void UpgradeCloneCount()
    {
        cloneCount++;
    }

    // Upgrade số hướng bắn
    public void UpgradeNumberOfDirections(int amount)
    {
        numberOfDirections += amount;
    }

    // Upgrade góc spread
    public void UpgradeSpreadAngle(float newAngle)
    {
        spreadAngle = newAngle;
    }
}
