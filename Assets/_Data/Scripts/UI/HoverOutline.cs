using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;
    private Image image;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        if (image != null)
            image.sprite = GameManager.instance.UIAtlas.GetSprite("Panel");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
            outline.enabled = false;
    }
}
