using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public WeaponData weaponData;

    [HideInInspector] public Transform parentBeforeDrag;
    private GameObject parentAfterDrag;

    private void Start()
    {
        parentAfterDrag = GameObject.Find("Inventory");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;
        transform.SetParent(parentAfterDrag.transform);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentBeforeDrag);
        image.raycastTarget = true;
    }

    public void SetItem(WeaponData weapon)
    {
        weaponData = weapon;
        image.sprite = GameManager.instance.UIAtlas.GetSprite(weapon.UISprite);
    }
}
