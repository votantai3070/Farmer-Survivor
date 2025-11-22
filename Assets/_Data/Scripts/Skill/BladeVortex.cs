using UnityEngine;

public class BladeVortex : MonoBehaviour
{

    [Header("Elements")]
    public Transform player;
    public GameObject swordPrefab;
    public float radius = 2f;
    public float rotationSpeed = 90f;

    private int swordCount = 2;
    private GameObject[] swords;

    void Start()
    {
        CreateSwords(swordCount);
    }

    void Update()
    {
        RotateSwords();
    }

    // Tạo các thanh kiếm dựa trên số lượng
    void CreateSwords(int count)
    {
        // Xóa kiếm cũ nếu có
        if (swords != null)
        {
            foreach (var s in swords)
            {
                if (s != null)
                    Destroy(s);
            }
        }

        swords = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            // Instantiate thanh kiếm
            swords[i] = Instantiate(swordPrefab, transform);
        }
    }

    // Xoay và đặt vị trí các thanh kiếm quanh player
    void RotateSwords()
    {
        float angleStep = 360f / swordCount;
        float angle = Time.time * rotationSpeed;

        for (int i = 0; i < swordCount; i++)
        {
            float currentAngle = angle + i * angleStep;
            float rad = currentAngle * Mathf.Deg2Rad;

            // Tính vị trí theo mặt phẳng XY (2D)
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            swords[i].transform.position = player.position + offset;

            // Xoay kiếm theo trục Z
            swords[i].transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }


    // Hàm nâng cấp chiêu thức, thêm kiếm mới
    public void UpgradeSkill()
    {
        swordCount++;
        CreateSwords(swordCount);
    }
}
