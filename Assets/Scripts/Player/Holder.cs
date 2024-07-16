using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public int selectedWeapon = 0;
    public int selectedItem = 0;

    public Transform weaponInventory;
    public Transform itemInventory;

    private const int itemSlots = 3;

    void Start()
    {
        SelectWeapon();
        SelectItem();
    }

    void Update()
    {
        WeaponSelection();
        ItemSelection();
    }

    void WeaponSelection()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= weaponInventory.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = weaponInventory.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void ItemSelection()
    {
        int previousSelectedItem = selectedItem;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItem = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedItem = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedItem = 2;
        }

        if (previousSelectedItem != selectedItem)
        {
            SelectItem();
        }
    }

    public void SelectWeapon()
    {
        int childCount = weaponInventory.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < weaponInventory.childCount; i++)
        {
            if (i > childCount) { return; }

            Transform weapon = weaponInventory.GetChild(i);
            weapon.gameObject.SetActive(i == selectedWeapon);
        }
    }

    public void SelectItem()
    {

        int childCount = itemInventory.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < itemSlots; i++)
        {
            if (itemSlots > childCount) { return; }

            Transform item = itemInventory.GetChild(i);
            item.gameObject.SetActive(i == selectedItem);
        }
    }

    public GameObject GetCurrentWeapon()
    {
        if (selectedWeapon >= 0 && selectedWeapon < weaponInventory.childCount)
        {
            Transform selectedTransform = weaponInventory.GetChild(selectedWeapon);
            return selectedTransform?.gameObject;
        }
        else
        {
            Debug.LogWarning("Selected weapon index is out of range.");
            return null;
        }
    }

    public GameObject GetCurrentItem()
    {
        if (selectedItem >= 0 && selectedItem < itemSlots)
        {
            Transform selectedTransform = itemInventory.GetChild(selectedItem);
            return selectedTransform?.gameObject;
        }
        else
        {
            Debug.LogWarning("Selected item index is out of range.");
            return null;
        }
    }
}
