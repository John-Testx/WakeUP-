using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapons")]
public class Weapon : Item
{
    public GameObject prefab;
    public int magazineSize;
    public int magazineCount;
    public float range;
    public WeaponType weaponType;
    public WeaponSlot slot;
}

public enum WeaponSlot { SecondarySlot, MainSlot, MeleeSlot, GrenadeSlot, Shield }
public enum WeaponType { Melee, Pistol, AssaultRifle, Shotgun, Sniper, Grenade, Shield } 
