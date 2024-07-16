using Unity.VisualScripting;
using UnityEngine;


public class PlayerInventory : Inventory1
{
    [SerializeField] KeyCode DropKey;
    [SerializeField] KeyCode RotateKey;
    [SerializeField] KeyCode ThrowKey;
    [SerializeField] GameObject player;
    [SerializeField] Transform Holder;
    [SerializeField] private PlayerHUD playerHUD;
    private float rotationSensitivity = 1f;
    public float throwForce = 500f;
    
    ItemSwitch itemSwitch;
    bool canDrop;
    bool canPick;

    private void Start()
    {
        InitVariables();
    }

    private void Update()
    {
        HandleKeys();
    }

    void HandleKeys()
    {
        WeaponHolder();
        ItemHolder();
    }

    public override void InitVariables()
    {
        base.InitVariables();
        playerHUD = FindObjectOfType<PlayerHUD>();
        itemSwitch = FindObjectOfType<ItemSwitch>();
    }

    void WeaponHolder()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentWeapon = (int) WeaponSlot.MeleeSlot;
        }
    }

    public void SetWeapon(int weaponToSet)
    {
        GameObject prefab = GetWeapon(weaponToSet).prefab;

        if (prefab != null)
        {
            GameObject weapon = Instantiate(prefab.gameObject, weaponHolder.transform);
            DisableCollider(weapon);
            DisableRigidBody(weapon);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = Quaternion.Euler(0, 0f, 0);
        }
    }

    public override void AddWeapon(Weapon newWeapon)
    {
        if (newWeapon == null) { 
            Debug.LogError("Weapon component is missing");
            return; 
        }
        base.AddWeapon(newWeapon);

        //Update Weapon UI

        playerHUD.UpdateWeaponUI(newWeapon);
    }

    public override void GrabItem(GameObject item)
    {
        ObjectInteractable2 interactable = item.GetComponent<ObjectInteractable2>();
        if (interactable == null)
        {
            //Debug.LogError("ObjectInteractable2 component is missing from item in GrabItem");
            return;
        }

        Item itemGrabbed = interactable.Item;
        if (itemGrabbed == null)
        {
            int capacity = ItemInventoryCapacity - 1;
            //Debug.LogError($"Item component is null in ObjectInteractable2 in GrabItem, {item.name} is being set into holder");
            
            if (capacity >= 0)
            {
                DisableRigidBody(item);
                DisableCollider(item);
                SetItem(item);
                ItemInventoryCapacity = capacity;
                return;
            }
        }

        if (item != null)
        {
            DisableCollider(item);
            DisableRigidBody(item);
            CheckItem(itemGrabbed, item); // line 93
        }
    }

    void DisableCollider(GameObject @object)
    {
        if (@object.TryGetComponent(out Collider itemCollider))
        {
            Physics.IgnoreCollision(itemCollider, player.GetComponent<Collider>(), true);
        }
    }

    void DisableRigidBody(GameObject @object)
    {
        if (@object.TryGetComponent<Rigidbody>(out Rigidbody itemRigidbody))
        {
            itemRigidbody.isKinematic = true;
            itemRigidbody.velocity = Vector3.zero;
            itemRigidbody.angularVelocity = Vector3.zero;
        }
    }

    void CheckItem(Item newItem, GameObject @object)
    {
        int capacity = ItemInventoryCapacity - 1;

        if (newItem == null)
        {
            Debug.LogError("newItem is null in CheckItem");
            return;
        }

        if (@object == null)
        {
            Debug.LogError("@object is null in CheckItem");
            return;
        }

        if (newItem.GetType() == typeof(Weapon))
        {
            Destroy(@object);
            Weapon weapon = (Weapon)newItem;
            AddWeapon(weapon);
            SetWeapon((int) weapon.slot);
        }
        else
        {
            AddItem(newItem);
            SetItem(@object); // line 99
            ItemInventoryCapacity = capacity;
        }
    }

    void SetItem(GameObject item)
    {   
        if (item == null) { return; }

        item.transform.SetParent(Holder.GameObject().transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        itemSwitch.SelectItem();
    }

    void ItemHolder()
        {
        if (Input.GetKeyUp(DropKey))
        {
            DropItem();
        }
        if (Input.GetKeyUp(ThrowKey))
        {
            ThrowItem();
        }
        if (Input.GetKey(RotateKey)) //hold key to rotate
        {
            RotateObject();
        }
        else
        {
            PlayerCamera.SetMoveMouse(!canDrop);
        }
    }

    void RotateObject()
    {
        GameObject currentItem = itemSwitch.GetCurrentItem();

        canDrop = false; //make sure throwing can't occur during rotating

        PlayerCamera.SetMoveMouse(canDrop);
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;

        //rotate the object depending on mouse X-Y Axis
        currentItem.transform.Rotate(Vector3.down, XaxisRotation);
        currentItem.transform.Rotate(Vector3.right, YaxisRotation);

    }

    public void DropItem()
    {
        GameObject currentItem = itemSwitch.GetCurrentItem();
        ItemInventoryCapacity ++;

        if (currentItem)
        {
            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = currentItem.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.freezeRotation = false;
            Collider collider = currentItem.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            currentItem.transform.SetParent(null);
            itemSwitch.SelectItem();

            //currentItem.transform.localScale = Vector3.one;
        }
    }

    public override void ThrowItem()
    {
        ItemSwitch i = FindObjectOfType<ItemSwitch>();

        GameObject currentItem = i.GetCurrentItem();
        if (currentItem == null) { return; }

        ItemInventoryCapacity++;

        if (currentItem.TryGetComponent<Grenade>(out Grenade grenade))
        {
            grenade.hasBeenThrown = true;
        }
        
        //same as drop function, but add force to object before undefining it
        DisableCollider(currentItem);
        currentItem.layer = 0;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Rigidbody>().transform.parent = null;
        currentItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        currentItem = null;

        Invoke(nameof(ShowItem), 0.5f);
    }

    void ShowItem()
    {
        itemSwitch.SelectItem();
    }

}
