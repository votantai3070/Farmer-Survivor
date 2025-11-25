using System.Collections;
using UnityEngine;

public class BladeVortex : Skill
{
    [Header("Upgrade Info")]

    [Header("Elements")]
    public Transform player;
    public GameObject swordPrefab;
    public float radius = 2f;
    public float rotationSpeed = 90f;

    [SerializeField] int swordCount = 2;
    private GameObject[] swords;
    private bool isCreatingSwords = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        CreateSwords(swordCount);
    }

    void Update()
    {
        if (swords != null && swords.Length > 0 && !isCreatingSwords)
        {
            RotateSwords();
        }
    }
    void CreateSwords(int count)
    {
        if (isCreatingSwords) return;

        Debug.Log($"CreateSwords called with count: {count}");
        StartCoroutine(CreateSwordsCoroutine(count));
    }

    IEnumerator CreateSwordsCoroutine(int count)
    {
        isCreatingSwords = true;

        // Xóa kiếm cũ
        if (swords != null)
        {
            Debug.Log($"Destroying {swords.Length} old swords");
            foreach (var s in swords)
            {
                if (s != null)
                    ObjectPool.instance.DelayReturnToPool(s);
            }
        }

        yield return null; // Đợi 1 frame

        // Tạo mảng mới
        swords = new GameObject[count];
        int successCount = 0;

        for (int i = 0; i < count; i++)
        {
            GameObject sword = ObjectPool.instance.GetObject(swordPrefab, transform);

            if (sword != null)
            {
                swords[i] = sword;

                ColliderSkill collider = sword.GetComponent<ColliderSkill>();
                if (collider != null)
                {
                    collider.SetWeaponData(weaponData);
                }

                successCount++;
            }
            else
            {
                Debug.LogError($"Failed to get sword {i} from pool!");
            }
        }

        swordCount = count;
        Debug.Log($"Created {successCount}/{count} swords successfully");

        isCreatingSwords = false;
    }

    void RotateSwords()
    {
        if (swords == null || swords.Length == 0) return;

        int swordsPerRing = 5;
        float radiusIncrement = 1.5f;
        float angle = Time.time * rotationSpeed;

        int currentSwordIndex = 0;
        int ringNumber = 0;

        // Dùng swords.Length thay vì swordCount
        while (currentSwordIndex < swords.Length)
        {
            int swordsInThisRing = Mathf.Min(swordsPerRing, swords.Length - currentSwordIndex);
            float currentRadius = radius + (ringNumber * radiusIncrement);
            float angleStep = 360f / swordsInThisRing;
            float ringAngleOffset = ringNumber * 30f;

            for (int i = 0; i < swordsInThisRing; i++)
            {
                if (currentSwordIndex >= swords.Length) break;

                if (swords[currentSwordIndex] != null)
                {
                    float currentAngle = angle + ringAngleOffset + (i * angleStep);
                    float rad = currentAngle * Mathf.Deg2Rad;

                    Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * currentRadius;
                    swords[currentSwordIndex].transform.position = player.position + offset;
                    swords[currentSwordIndex].transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                }

                currentSwordIndex++;
            }

            ringNumber++;
        }
    }

    public override void UpgradeWeaponData(WeaponData weapon)
    {


        Debug.Log($"=== UPGRADE: {swordCount} -> {weapon.bulletShotSize} swords ===");

        base.UpgradeWeaponData(weapon);

        radius = weaponData.range;
        rotationSpeed = weaponData.bulletSpeed;

        CreateSwords(weaponData.bulletShotSize);
    }

    void OnDisable()
    {
        if (swords != null)
        {
            foreach (var s in swords)
            {
                if (s != null)
                    ObjectPool.instance.DelayReturnToPool(s);
            }
        }
        swords = null;
    }
}
