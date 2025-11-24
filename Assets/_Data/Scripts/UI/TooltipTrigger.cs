using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int currentLevel = 1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillData skillData = GetComponent<SkillHolder>()?.SkillData;

        if (skillData != null)
        {
            string header = $"{skillData.skillName} Lv.{currentLevel}";
            string content = skillData.GetDescription(currentLevel);

            TooltipSystem.Show(content, header);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}
