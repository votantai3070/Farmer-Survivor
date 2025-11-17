using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AvailableUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradeManager available;

    [Header("Upgrade Character")]
    [SerializeField] List<CharacterData> upgradePlayerList = new();
    [Header("Available Upgrade")]
    [SerializeField] GameObject chooseUpgradePanel;
    [SerializeField] GameObject chooseUpgrade;


    // Setup available weapons
    public void SetWeaponAvailable()
    {
        chooseUpgradePanel.SetActive(true);

        List<WeaponData> availableList = available.AvaiableUpgrades();

        var availableSort = availableList
             .OrderBy(x => Random.value)                     // random
             .DistinctBy(w => new { w.weaponName, w.level }) // loại trùng
             .Take(Mathf.Min(3, availableList.Count))
             .ToList();

        //Debug.Log($"Available Weapons: {string.Join(", ", availableList.Select(w => w.weaponName + " Lv" + w.level))}");

        //Debug.Log($"Available Weapons Count: {availableSort.Count}");

        for (int i = 0; i < chooseUpgrade.transform.childCount; i++)
        {
            if (availableSort.Count > i)
            {
                chooseUpgrade.transform.GetChild(i).gameObject.SetActive(true);

                Button btn = chooseUpgrade.transform.GetChild(i).GetComponent<Button>();

                ColorBlock cb = btn.colors;

                cb.normalColor = availableSort[i].rareColor;

                cb.selectedColor = availableSort[i].rareColor;

                //cb.highlightedColor = availableSort[i].rareColor;

                btn.colors = cb;

                chooseUpgrade.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite
                    = GameManager.instance.UIAtlas.GetSprite(availableSort[i].UISprite);

                chooseUpgrade.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text
                    = $"{availableSort[i].weaponName} / Level {availableSort[i].level}";

                chooseUpgrade.transform.GetChild(i).GetComponent<UpgradeClicker>().SetWeaponData(availableSort[i]);
            }
            else
                chooseUpgrade.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Setup available players
    public void SetPlayerAvailable()
    {
        //List<CharacterData> characterDatas = upgradePlayerList;

        for (int i = 0; i < chooseUpgrade.transform.childCount; i++)
        {
            if (upgradePlayerList.Count > i)
            {
                chooseUpgrade.transform.GetChild(i).gameObject.SetActive(true);

                Button btn = chooseUpgrade.transform.GetChild(i).GetComponent<Button>();

                ColorBlock cb = btn.colors;

                cb.normalColor = upgradePlayerList[i].rareColor;

                cb.selectedColor = upgradePlayerList[i].rareColor;

                //cb.highlightedColor = upgradePlayerList[i].rareColor;

                btn.colors = cb;

                chooseUpgrade.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text
                    = $"{upgradePlayerList[i].characterName} / Level {upgradePlayerList[i].level}";

                chooseUpgrade.transform.GetChild(i).GetComponent<UpgradeClicker>().SetCharacterData(upgradePlayerList[i]);
            }
            else
                chooseUpgrade.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
