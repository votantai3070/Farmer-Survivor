using System;
using UnityEngine;

public enum SkillType
{
    BladeVortex,
    FlyingSword,
    SwordRain,
    BoomerangSword,
    ShadowBlade
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public static Action<SkillType, WeaponData> onChangeSkillType;

    public Skill[] skills;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        skills = GetComponentsInChildren<Skill>(true);
    }

    private void Start()
    {
        onChangeSkillType += HandleChangeSkill;
    }

    private void OnDestroy()
    {
        onChangeSkillType -= HandleChangeSkill;
    }

    private void HandleChangeSkill(SkillType skillType, WeaponData weapon)
    {
        //Debug.Log("Changing skill to: " + skillType.ToString());
        //Debug.Log("With skill: " + weapon.weaponName);
        //Debug.Log("Skills level: " + weapon.level);

        switch (skillType)
        {
            case SkillType.BladeVortex:
                skills[0].gameObject.SetActive(true);
                skills[0].GetComponent<BladeVortex>()?.UpgradeWeaponData(weapon);
                break;
            case SkillType.FlyingSword:
                skills[1].gameObject.SetActive(true);
                skills[1].GetComponent<FlyingSword>()?.UpgradeWeaponData(weapon);
                break;
            case SkillType.SwordRain:
                skills[2].gameObject.SetActive(true);
                skills[2].GetComponent<SwordRain>()?.UpgradeWeaponData(weapon);
                break;
            case SkillType.BoomerangSword:
                skills[3].gameObject.SetActive(true);
                skills[3].GetComponent<BoomerangBlade>()?.UpgradeWeaponData(weapon);
                break;
            case SkillType.ShadowBlade:
                skills[4].gameObject.SetActive(true);
                skills[4].GetComponent<ShadowBlade>()?.UpgradeWeaponData(weapon);

                break;
            default:
                break;
        }
    }
}
