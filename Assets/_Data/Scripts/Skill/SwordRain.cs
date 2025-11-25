using System.Collections;
using UnityEngine;

public class SwordRain : Skill
{
    public Transform player;
    public GameObject swordPrefab;
    public float spawnHeight = 5f; // Độ cao spawn kiếm
    public float fallSpeed = 10f;
    public int swordCount = 5; // Số kiếm mỗi đợt
    public float spawnRadius = 3f; // Bán kính spawn
    public float fireRate = 2f;

    void Start()
    {
        StartCoroutine(SpawnSwordRain());
    }

    IEnumerator SpawnSwordRain()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            // Spawn nhiều kiếm xung quanh player
            for (int i = 0; i < swordCount; i++)
            {
                // Vị trí random quanh player
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = new Vector3(
                    player.position.x + randomOffset.x,
                    player.position.y + spawnHeight,
                    0
                );

                GameObject sword = ObjectPool.instance.GetObject(swordPrefab);

                sword.transform.position = spawnPos;
                sword.transform.rotation = Quaternion.Euler(0, 0, 180);

                // Cho kiếm rơi xuống
                Rigidbody2D rb = sword.GetComponent<Rigidbody2D>();
                sword.GetComponent<CloseWeaponMovement>().SetWeaponData(weaponData);

                if (rb != null)
                {
                    rb.gravityScale = 0;
                    rb.linearVelocity = Vector2.down * fallSpeed;
                }

                ObjectPool.instance.DelayReturnToPool(sword, 3f);
            }
        }
    }

    public override void UpgradeWeaponData(WeaponData weapon)
    {
        base.UpgradeWeaponData(weapon);

        fallSpeed = weaponData.bulletSpeed;
        swordCount = weaponData.bulletShotSize;
        spawnRadius = weaponData.range;
        fireRate = 1f / weaponData.fireRate;
    }
}
