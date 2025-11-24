using System.Collections;
using UnityEngine;

public class BoomerangBlade : Skill
{
    [SerializeField] Transform player;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float throwDistance = 5f;
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] float returnSpeed = 8f;
    [SerializeField] float fireRate = 2f;
    [SerializeField] float returnThreshold = 0.3f;

    // Biến upgrade
    [SerializeField] int numberOfSwords = 1; // Số lượng kiếm (nâng cấp được)
    [SerializeField] float spreadAngle = 30f; // Góc giữa các kiếm

    void Start()
    {
        StartCoroutine(ThrowBoomerang());
    }

    IEnumerator ThrowBoomerang()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            Transform target = GetClosestEnemy();
            if (target != null)
            {
                // Bắn nhiều kiếm
                for (int i = 0; i < numberOfSwords; i++)
                {
                    float angleOffset = CalculateAngleOffset(i);
                    StartCoroutine(BoomerangRoutine(target, angleOffset));
                }
            }
        }
    }

    // Tính góc offset cho mỗi kiếm
    float CalculateAngleOffset(int index)
    {
        if (numberOfSwords == 1)
            return 0f;

        // Tính góc dựa trên số lượng kiếm
        float totalSpread = spreadAngle * (numberOfSwords - 1);
        float startAngle = -totalSpread / 2f;
        return startAngle + (spreadAngle * index);
    }

    IEnumerator BoomerangRoutine(Transform target, float angleOffset)
    {
        GameObject sword = ObjectPool.instance.GetObject(swordPrefab);

        // Tắt trigger để tránh return sớm
        Collider2D col = sword.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Tính hướng ban đầu đến target
        Vector2 baseDirection = (target.position - player.position).normalized;

        // Áp dụng góc offset để tạo spread
        float angleInRadians = angleOffset * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(
            baseDirection.x * Mathf.Cos(angleInRadians) - baseDirection.y * Mathf.Sin(angleInRadians),
            baseDirection.x * Mathf.Sin(angleInRadians) + baseDirection.y * Mathf.Cos(angleInRadians)
        );

        Vector3 targetPos = player.position + (Vector3)direction * throwDistance;

        // Xoay kiếm theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sword.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // ===== GIAI ĐOẠN 1: BAY RA =====
        float elapsed = 0;
        Vector2 startPos = player.position;
        float radius = 2f;

        Vector2 newPosition = startPos + (direction.normalized * radius);
        sword.transform.position = newPosition;

        // Bật collider sau khi bay ra một chút
        yield return new WaitForSeconds(0.1f);
        if (col != null) col.enabled = true;

        while (elapsed < 1f)
        {
            if (sword == null || !sword.activeInHierarchy)
                yield break;

            elapsed += Time.deltaTime * throwSpeed / throwDistance;
            sword.transform.position = Vector3.Lerp(newPosition, targetPos, elapsed);
            sword.transform.Rotate(0, 0, 720 * Time.deltaTime);
            yield return null;
        }

        // ===== GIAI ĐOẠN 2: QUAY LẠI PLAYER =====
        while (sword != null && sword.activeInHierarchy)
        {
            float distanceToPlayer = Vector3.Distance(sword.transform.position, player.position);

            if (distanceToPlayer < returnThreshold)
            {
                ObjectPool.instance.DelayReturnToPool(sword);
                yield break;
            }

            if (player != null)
            {
                sword.transform.position = Vector3.MoveTowards(
                    sword.transform.position,
                    player.position,
                    returnSpeed * Time.deltaTime
                );
            }

            sword.transform.Rotate(0, 0, 720 * Time.deltaTime);
            yield return null;
        }

        if (sword != null && sword.activeInHierarchy)
            ObjectPool.instance.DelayReturnToPool(sword);
    }

    Transform GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

    // Hàm upgrade để tăng số lượng kiếm
    public void UpgradeNumberOfSwords(int amount)
    {
        numberOfSwords += amount;
    }

    // Hàm upgrade để thay đổi góc spread
    public void UpgradeSpreadAngle(float newSpreadAngle)
    {
        spreadAngle = newSpreadAngle;
    }
}
