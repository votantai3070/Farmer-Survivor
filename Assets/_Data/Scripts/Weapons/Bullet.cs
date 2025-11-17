using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Gun Bullet")]
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public int reserveAmmo;
    [HideInInspector] int magazineSize;

    [HideInInspector] public Dictionary<(string, int), (int current, int reserve, int magazineSize)> ammoMap = new();
    //public List<WeaponData> weaponDatas = new();
    string currentWeaponName;
    int level;

    public void Shot(int bulletToUse)
    {
        currentAmmo -= bulletToUse;

        currentAmmo = Mathf.Max(0, currentAmmo);

        ammoMap[(currentWeaponName, level)] = (currentAmmo, reserveAmmo, magazineSize);

        //weaponDatas.Find(w => w.weaponName == currentWeaponName).currentAmmo = currentAmmo;

        UIManager.instance.ammoText.text = $"{currentAmmo}/{reserveAmmo}";
    }

    public void Reload()
    {
        if (currentAmmo == magazineSize) return;

        if (reserveAmmo <= 0) return;

        int needed = magazineSize - currentAmmo;

        int bulletToLoad = Mathf.Min(needed, reserveAmmo);

        currentAmmo += bulletToLoad;

        reserveAmmo -= bulletToLoad;

        ammoMap[(currentWeaponName, level)] = (currentAmmo, reserveAmmo, magazineSize);

        //weaponDatas.Find(w => w.weaponName == currentWeaponName).currentAmmo = currentAmmo;
        //weaponDatas.Find(w => w.weaponName == currentWeaponName).reserveAmmo = reserveAmmo;

        UIManager.instance.ammoText.text = $"{currentAmmo}/{reserveAmmo}";
    }

    public void SetWeaponData(WeaponData weapon)
    {
        currentWeaponName = weapon.weaponName;

        level = weapon.level;

        //if (!weaponDatas.Exists(w => w.weaponName == currentWeaponName))
        //    weaponDatas.Add(weapon);
        //else
        //{
        //    int index = weaponDatas.FindIndex(w => w.weaponName == currentWeaponName);
        //    if (weapon.level > weaponDatas[index].level)
        //        weaponDatas[index] = weapon;
        //}

        //currentAmmo = weaponDatas.Find(w => w.weaponName == currentWeaponName).currentAmmo;
        //reserveAmmo = weaponDatas.Find(w => w.weaponName == currentWeaponName).reserveAmmo;
        //magazineSize = weaponDatas.Find(w => w.weaponName == currentWeaponName).magazineSize;

        ammoMap[(currentWeaponName, level)] = (weapon.currentAmmo, weapon.reserveAmmo, weapon.magazineSize);

        currentAmmo = ammoMap[(currentWeaponName, level)].current;
        reserveAmmo = ammoMap[(currentWeaponName, level)].reserve;
        magazineSize = ammoMap[(currentWeaponName, level)].magazineSize;
    }

    public void SetWeaponFromList(WeaponData weapon)
    {
        currentWeaponName = weapon.weaponName;

        level = weapon.level;

        //currentAmmo = weaponDatas.Find(w => w.weaponName == currentWeaponName).currentAmmo;
        //reserveAmmo = weaponDatas.Find(w => w.weaponName == currentWeaponName).reserveAmmo;
        //magazineSize = weaponDatas.Find(w => w.weaponName == currentWeaponName).magazineSize;

        currentAmmo = ammoMap[(currentWeaponName, level)].current;
        reserveAmmo = ammoMap[(currentWeaponName, level)].reserve;
        magazineSize = ammoMap[(currentWeaponName, level)].magazineSize;
    }

    public void AddAmmo(ItemData bullet)
    {
        reserveAmmo = ammoMap[(currentWeaponName, level)].reserve;

        //reserveAmmo = weaponDatas.Find(w => w.weaponName == currentWeaponName).reserveAmmo;

        reserveAmmo += bullet.value;

        //weaponDatas.Find(w => w.weaponName == currentWeaponName).reserveAmmo = reserveAmmo;

        ammoMap[(currentWeaponName, level)] = (currentAmmo, reserveAmmo, magazineSize);
    }

}
