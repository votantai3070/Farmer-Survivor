using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("Skill Settings")]
    protected WeaponData weaponData;

    public SkillType skillType;

    protected int maxLevel = 3;
    //[SerializeField] protected SkillHolder[] skillHolders;

    public virtual void UpgradeWeaponData(WeaponData weapon)
    {
        if (weapon == null || weapon.level > maxLevel) return;

        weaponData = weapon;
    }
}
