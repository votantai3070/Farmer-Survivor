using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotBarManager : MonoBehaviour
{
    [Header("Hot Bar Manager")]
    public InventorySlot[] hotbarSlots;
    public List<GameObject> hotbarSlotItems = new();

    [HideInInspector] public WeaponData currentWeaponData;
    [SerializeField] Bullet bullet;

    private void Start()
    {
        StartCoroutine(InitHotbar());
    }

    private void Update()
    {
        ShowHotbarList();

        if (Input.GetKeyDown(KeyCode.Alpha1)) UseWeaponInSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseWeaponInSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseWeaponInSlot(2);

        if (currentWeaponData != null)
            UpdateWeapon();
    }

    // If the current weapon has a higher level version in the hotbar, switch to it
    void UpdateWeapon()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            WeaponData weapon = hotbarSlots[i].GetWeaponData();
            if (weapon != null && weapon.weaponName == currentWeaponData.weaponName && weapon.level > currentWeaponData.level)
            {
                UseWeaponInSlot(i);
                break;
            }
        }
    }

    // Initialize hotbar by equipping the first weapon in the first slot
    IEnumerator InitHotbar()
    {
        yield return null;
        UseWeaponInSlot(0);
    }

    // Equip weapon in the specified hotbar slot
    public void UseWeaponInSlot(int index)
    {
        if (index >= 0 && index < hotbarSlots.Length)
        {
            WeaponData weapon = hotbarSlots[index].GetWeaponData();

            if (weapon != null)
            {
                if (!bullet.ammoMap.ContainsKey((weapon.weaponName, weapon.level))
                     || bullet.ammoMap.ContainsKey((weapon.weaponName, weapon.level - 1)))
                    bullet.SetWeaponData(weapon);
                else
                    bullet.SetWeaponFromList(weapon);

                currentWeaponData = weapon;

                if (weapon.weaponType == WeaponType.Rifle
                    || weapon.weaponType == WeaponType.Pistol
                    || weapon.weaponType == WeaponType.Shotgun)
                {
                    var (current, reserve, _) = bullet.ammoMap[(weapon.weaponName, weapon.level)];

                    UIManager.instance.ammoText.enabled = true;
                    UIManager.instance.ammoText.text =
                        $"{current}/{reserve}";
                }
                else
                {
                    UIManager.instance.ammoText.enabled = false;
                }
            }
            else
            {
                Debug.Log("Slot " + index + " is empty!");
            }
        }
    }

    private void ShowHotbarList()
    {
        for (int i = 0; i < hotbarSlotItems.Count; i++)
        {
            WeaponData weapon = hotbarSlots[i].GetComponent<InventorySlot>().GetWeaponData();

            if (weapon != null)
            {
                hotbarSlotItems[i].transform.GetChild(0).GetComponent<Image>().sprite =
                    GameManager.instance.UIAtlas.GetSprite(weapon.UISprite);

                hotbarSlotItems[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{weapon.level}";
                hotbarSlotItems[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{i + 1}";
            }
            else
            {
                hotbarSlotItems[i].transform.GetChild(0).GetComponent<Image>().sprite =
                    GameManager.instance.UIAtlas.GetSprite("Panel");

                hotbarSlotItems[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                hotbarSlotItems[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{i + 1}";
            }
        }
    }
}
