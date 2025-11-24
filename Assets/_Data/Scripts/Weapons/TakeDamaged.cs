using UnityEngine;

public class TakeDamaged : MonoBehaviour
{
    [Header("TakeDamaged Setting")]

    public WeaponData weaponData;

    public void Attack(IDamagable target)
    {
        UseWeapon(target);
    }

    private void Update()
    {
        //Debug.Log(weaponData.weaponName);
        //Debug.Log(weaponData.level);
    }

    private void UseWeapon(IDamagable target)
    {
        bool isCrit = Random.value <= weaponData.criticalChange;

        int curentDamage = (int)(isCrit
            ? Random.Range(weaponData.firstDamage, weaponData.lastDamage + 1) * weaponData.criticalDamage
            : Random.Range(weaponData.firstDamage, weaponData.lastDamage + 1));

        target.TakeDamage(Mathf.FloorToInt(curentDamage), isCrit);
    }

    public void SetWeaponData(WeaponData newWeaponData)
    {
        weaponData = newWeaponData;
    }
}
