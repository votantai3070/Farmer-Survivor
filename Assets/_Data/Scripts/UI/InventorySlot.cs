using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        if (image != null)
            image.sprite = GameManager.instance.UIAtlas.GetSprite("Panel");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentBeforeDrag = transform;
        }
    }

    public WeaponData GetWeaponData()
    {
        if (transform.childCount > 0)
        {
            DraggableItem draggableItem = transform.GetChild(0).GetComponent<DraggableItem>();
            return draggableItem.weaponData;
        }
        return null;
    }
}
