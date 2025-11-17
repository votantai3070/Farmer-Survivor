using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    [SerializeField] WeaponData[] weaponDatas;
    public InventorySlot[] slots;
    [SerializeField] private DraggableItem draggablePrefab;

    private void Start()
    {
        InitializeNewWeaponInSlot();
    }

    private void InitializeNewWeaponInSlot()
    {
        for (int i = 0; i < weaponDatas.Length && i < slots.Length; i++)
        {
            DraggableItem newItem = Instantiate(draggablePrefab, slots[i].transform);
            newItem.SetItem(weaponDatas[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeSelf)
            {
                CloseInventoryBtn();
            }
            else
            {
                OpenInventoryBtn();
            }
        }

    }

    public void AddNewWeapon(WeaponData weapon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                DraggableItem newItem = Instantiate(draggablePrefab, slots[i].transform);
                newItem.SetItem(weapon);
                break;
            }
        }
    }

    public void CloseInventoryBtn()
    {
        inventory.SetActive(false);
        GameManager.instance.GameResume();
    }

    void OpenInventoryBtn()
    {
        inventory.SetActive(true);
        GameManager.instance.GamePause();
    }
}
