using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory1 : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private Item[] itemArray;
    [SerializeField] private int weaponInventorySize;
    [SerializeField] public GameObject weaponHolder;
    [SerializeField] private int itemInventoryCapacity;

    public int ItemInventoryCapacity { get { return itemInventoryCapacity; } set { itemInventoryCapacity = value; }  }
    public int WeaponInventorySize { get { return weaponInventorySize; } set { weaponInventorySize = value; } }

    public int currentWeapon;
    
    private void Start(){}
    
    public virtual void ThrowItem() { }
    public virtual void GrabItem(GameObject @object) { }
    
    public virtual void AddWeapon(Weapon newWeapon)
    {
        int newWeaponSlot = (int)newWeapon.slot;

        if (weapons[newWeaponSlot] != null)
        {
            RemoveWeapon(newWeaponSlot);    //Debug.Log("Slot occupied");
        }

        weapons[newWeaponSlot] = newWeapon; 
    }

    public virtual void AddItem(Item newItem)
    {
        int newItemSlot = itemArray.Length - 1;

        if (itemArray[newItemSlot] != null)
        {
            RemoveItem(newItemSlot);    //Debug.Log("Slot occupied");
        }

        itemArray[newItemSlot] = newItem;
    }

    public void RemoveItem(int index)
    {
        itemArray[index] = null;
    }

    public void RemoveWeapon(int index)
    {
        weapons[index]= null;
    }

    public Weapon GetWeapon(int index)
    {
        return weapons[index];
    }

    public virtual void InitVariables() 
    {
        var EnumCount = Enum.GetNames(typeof(WeaponSlot)).Length;
        weapons = new Weapon[EnumCount];
        itemArray = new Item[itemInventoryCapacity];
    }
}
