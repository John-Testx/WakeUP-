using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.Object flashlight;
    public GameObject player;
    private Light playerLight;
    [SerializeField] KeyCode DropKey;
    [SerializeField] KeyCode RotateKey;
    [SerializeField] KeyCode ThrowKey;
    public float throwForce = 500f;
    private float rotationSensitivity = 1f;
    public int maxNumberOfObjects;
    ItemSwitch itemSwitch;
    public List<Transform> transforms;
    public bool lightOn;
    bool canDrop;
    public UnityEngine.Object Holder;

    void Start()
    {
        //playerLight= flashlight.GetComponent<Light>();
        itemSwitch = FindObjectOfType<ItemSwitch>();
        lightOn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLight != null) { TurnFlashlight(); }

        if (itemSwitch != null)
        {
            if (Input.GetKeyUp(DropKey))
            {
                DropItem();
            }
            if (Input.GetKeyUp(ThrowKey))
            {
                ThrowObject();
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
    }

    bool allowedToPickObjects()
    {
        if (Holder.GameObject().transform.childCount < maxNumberOfObjects ) {
            
            return true;

        }
        else
        {
            return false;
        }
    }

    public bool AddItem( GameObject item) {
        
        bool allowItem = allowedToPickObjects();

        if (item != null && allowItem) 
        {

            if (item.GameObject().TryGetComponent<Rigidbody>(out Rigidbody itemRigidbody))
            {
                itemRigidbody.isKinematic = true;
                itemRigidbody.velocity = Vector3.zero;
                itemRigidbody.angularVelocity = Vector3.zero;
            }

            if (item.TryGetComponent<Collider>(out Collider itemCollider))
            {
                Physics.IgnoreCollision(itemCollider, player.GetComponent<Collider>(), true);
            }

            item.transform.SetParent(Holder.GameObject().transform);

            Vector3 inventoryPosition = new Vector3(0, 0, 0); // Replace with your inventory offset
            item.transform.localPosition = inventoryPosition;
            item.transform.localRotation = Quaternion.identity;

            item.GameObject().transform.localRotation = Quaternion.identity; 
            return true;
        }

        else
        {
            //Debug.Log("Can't pick any more Items.");
            return false;
        }

        /*.Log(Holder.GameObject().transform.childCount);
        bool l = allowedToPickObjects();
        Debug.Log(l);
        */

    }

    public void TurnFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerLight.enabled = !lightOn;
            lightOn = !lightOn;
        }
    }

    public void MoveItem()
    {

    }

    public void DropItem()
    {

        GameObject currentItem = itemSwitch.GetCurrentItem();

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
            //currentItem.transform.localScale = Vector3.one;
        }

    }

    public void RemoveItem(int index)
    {
        Debug.Log(index);
        Transform item = Holder.GameObject().transform.GetChild(index);
        item.gameObject.SetActive(false);
    }

    void ThrowObject()
    {
        ItemSwitch i = FindObjectOfType<ItemSwitch>();
        GameObject currentItem = i.GetCurrentItem();

        if(currentItem.TryGetComponent<Grenade>(out Grenade grenade))
        {
            grenade.hasBeenThrown = true;
        }

        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(currentItem.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        currentItem.layer = 0;
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Rigidbody>().transform.parent = null;
        currentItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        currentItem = null;
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


    public int GetKey(int i)
    {

        int childCount = Holder.GameObject().transform.childCount;

        for (int index = 0; index < childCount; index++)
        {
            Transform child = Holder.GameObject().transform.GetChild(index);

            ObjectInteractable2 keyInteractable = child.GetComponent<ObjectInteractable2>();

            if (keyInteractable != null)
            {
                int code = keyInteractable.GetKeyCode();
                
                if (code == i)
                {
                    RemoveItem(index);
                    return code;
                }
            }
        }

        return 0;
    }

    

}
