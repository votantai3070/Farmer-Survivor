using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Skill Data", order = 51)]
public class SkillData : ScriptableObject
{
    [Header("Basic Info")]
    public string skillName;

    [Header("Icons")]
    public Sprite iconLv1_2;
    public Sprite iconLv3;

    [Header("Skill Point")]
    public int skillPointLv1;
    public int skillPointLv2;
    public int skillPointLv3;

    [Header("Level 1 Description")]
    [TextArea(5, 8)]
    public string descriptionLv1;

    [Header("Level 2 Description")]
    [TextArea(5, 8)]
    public string descriptionLv2;

    [Header("Level 3 Description")]
    [TextArea(5, 8)]
    public string descriptionLv3;

    public string GetDescription(int level)
    {
        switch (level)
        {
            case 1: return descriptionLv1;
            case 2: return descriptionLv2;
            case 3: return descriptionLv3;
            default: return descriptionLv1;
        }
    }

    public Sprite GetIcon(int level)
    {
        return level >= 3 ? iconLv3 : iconLv1_2;
    }


}
