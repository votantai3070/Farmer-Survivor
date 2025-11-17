using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeClicker : MonoBehaviour, IPointerClickHandler
{
    private GameObject upgradePanel;
    public WeaponData weaponData;
    public CharacterData characterData;
    private UpgradeManager upgradeManager;
    private InventoryManager inventoryManager;
    [SerializeField] bool upgradePlayer;

    private void Awake()
    {
        if (!upgradePlayer)
        {
            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        }
        upgradePanel = GameObject.Find("ChooseUpgradePanel");
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        upgradePanel.SetActive(false);

        if (upgradePlayer)
        {
            UpgradePlayer();
            ChooseUpgradeCharacter.Instance.CloseChooseCharacterPanel();
        }

        else
        {
            UpgradeWeapon();
            GameManager.instance.GameResume();
        }
    }

    private void UpgradeWeapon()
    {
        if (weaponData == null) return;

        bool found = false;

        for (int i = 0; i < inventoryManager.slots.Length; i++)
        {
            if (inventoryManager.slots[i].transform.childCount > 0)
            {
                DraggableItem item = inventoryManager.slots[i].transform.GetChild(0).GetComponent<DraggableItem>();
                if (item.weaponData.weaponName == weaponData.weaponName)
                {
                    item.SetItem(weaponData);
                    upgradeManager.AddUpgrade(weaponData);
                    found = true;
                    break;
                }
            }
        }

        if (!found)
        {
            inventoryManager.AddNewWeapon(weaponData);
            upgradeManager.AddUpgrade(weaponData);
        }
    }
    private void UpgradePlayer()
    {
        PlayerPrefs.SetString("Character", characterData.characterName);
        PlayerPrefs.Save();
    }

    public void SetWeaponData(WeaponData weapon)
    {
        weaponData = weapon;
    }

    public void SetCharacterData(CharacterData character)
    {
        characterData = character;
    }
}