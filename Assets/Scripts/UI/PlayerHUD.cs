using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private WeaponUI weaponUI;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        //healthBar.SetValues(currentHealth, maxHealth);
    }

    public void UpdateWeaponUI(Weapon newWeapon)
    {
        if (weaponUI == null) { return; }
        weaponUI.UpdateInfo(newWeapon.sprite, newWeapon.magazineSize, newWeapon.magazineCount);
    }
}
