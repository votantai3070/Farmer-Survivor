using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    [SerializeField] SkillData skillData;
    [SerializeField] WeaponData weaponData;

    [SerializeField] Sprite skillIcon;
    Image skillImage;
    GameObject lockOverlay;
    GameObject selected;
    Button skillButton;

    [SerializeField] SkillType skillType;

    private bool isUnlocked;

    private void Awake()
    {
        skillImage = transform.GetChild(0).GetComponent<Image>();
        lockOverlay = transform.GetChild(1).gameObject;
        selected = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        if (skillImage.sprite == null && skillIcon != null)
            skillImage.sprite = skillIcon;

        SelectSkill(false);

        skillButton = GetComponent<Button>();

        skillButton.onClick.AddListener(() => UnlockedSkill());
    }

    private void Update()
    {
        if (isUnlocked)
            Unlocked();
        else
            Locked();
    }

    private void Locked()
    {
        lockOverlay.SetActive(true);
    }


    private void Unlocked()
    {
        lockOverlay.SetActive(false);
        isUnlocked = true;
    }

    public void SelectSkill(bool isSelected)
    {
        if (!IsUnlocked())
            return;

        selected.SetActive(isSelected);
    }

    public SkillData SkillData => skillData;

    private bool IsUnlocked() => isUnlocked;

    public void UnlockedSkill()
    {
        Debug.Log(UIManager.instance.GetCurrentDefeatEnemy());

        if (!CanUnlock())
            return;
        Unlocked();

        SelectSkill(true);

        Debug.Log("Unlocked Skill: " + skillType.ToString());
        Debug.Log("Weapon Level: " + weaponData.level.ToString());

        int skillPoints = SkillPoint();

        UIManager.instance.MinusDefeatEnemy(skillPoints);
        SkillManager.onChangeSkillType?.Invoke(skillType, weaponData);
    }

    private bool CanUnlock()
    {
        if (UIManager.instance.GetCurrentDefeatEnemy() >= SkillPoint() && !isUnlocked)
            return true;
        return false;
    }

    private int SkillPoint()
    {
        if (weaponData.level == 1)
            return skillData.skillPointLv1;
        else if (weaponData.level == 2)
            return skillData.skillPointLv2;
        else if (weaponData.level == 3)
            return skillData.skillPointLv3;

        return 0;
    }
}
