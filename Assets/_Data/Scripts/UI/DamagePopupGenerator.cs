using TMPro;
using UnityEngine;

public class DamagePopupGenerator : MonoBehaviour
{
    public static DamagePopupGenerator Instance;

    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color criticalColor;

    [Header("Container")]
    [SerializeField] private Transform popupContainer; // Gán DamagePopupCanvas
    [SerializeField] private Camera worldCamera; // Gán Main Camera (nếu dùng World Space)

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Tự tìm container nếu chưa gán
        if (popupContainer == null)
            popupContainer = transform;

        // Tự tìm camera nếu chưa gán
        if (worldCamera == null)
            worldCamera = Camera.main;
    }

    public void DisplayDamage(Vector3 position, float damageText, bool isCrit)
    {
        GameObject popup = ObjectPool.instance.GetObject(damagePrefab);

        DamagePopupAnimation popupAnimation = popup.GetComponent<DamagePopupAnimation>();

        // ✅ Set position qua method
        if (popupAnimation != null)
        {
            popupAnimation.SetStartPosition(position);
        }
        else
        {
            popup.transform.position = position;
        }

        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damageText.ToString();
        temp.color = isCrit ? criticalColor : normalColor;
    }

}
