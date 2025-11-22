using System;
using UnityEngine;

public enum SkillType
{
    BladeVortex,
    AutoShoot,
    SwordRain,
    BoomerangSword,
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public static Action<SkillType> onChangeSkillType;

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
        onChangeSkillType += HandleChangeSkillType;
    }

    private void OnDestroy()
    {
        onChangeSkillType -= HandleChangeSkillType;
    }

    private void HandleChangeSkillType(SkillType skillType)
    {

        foreach (var skill in skills)
        {
            if (skill.skillType == skillType)
                skill.gameObject.SetActive(true);
        }
    }
}
