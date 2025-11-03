using TMPro;
using UnityEngine;

public class DamagePopupGenerator : MonoBehaviour
{
    public static DamagePopupGenerator Instance;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] Color normalColor;
    [SerializeField] Color criticalColor;
    private void Awake()
    {
        Instance = this;
    }


    public void DisplayDamage(Vector3 position, float damageText, bool isCrit)
    {
        GameObject popup = ObjectPool.instance.GetObject(damagePrefab);
        popup.transform.position = position;
        popup.transform.rotation = Quaternion.identity;

        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damageText.ToString();
        if (isCrit)
            temp.color = criticalColor;

        else
            temp.color = normalColor;
    }

}
