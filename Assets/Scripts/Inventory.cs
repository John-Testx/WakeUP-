using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.Object flashlight;
    private Light playerLight;
    public int maxNumberOfObjects;
    public List<Transform> transforms;
    public bool lightOn;
    public UnityEngine.Object Holder;

    void Start()
    {
        playerLight= flashlight.GetComponent<Light>();
        
        lightOn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLight != null) { TurnFlashlight(); }
        DropItem();
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

    public bool AddItem(UnityEngine.Object item) {
        
        bool allowItem = allowedToPickObjects();

        if (item != null && allowItem) {
            
            item.GameObject().transform.SetParent(Holder.GameObject().transform);
            item.GameObject().transform.localPosition = new Vector3 (0, 0, 0);
            
            Collider collider = item.GetComponent<Collider>();

            if (collider != null)
            {
                collider.enabled = false;
            }
            
            if (item.GameObject().TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (rb != null) { Destroy(rb); }
                
            }

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
        ItemSwitch i = FindObjectOfType<ItemSwitch>();

        if (i != null)
        {
            GameObject currentItem = i.GetCurrentItem();

            if (currentItem != null && Input.GetKeyDown(KeyCode.G))
            {
                Rigidbody rb = currentItem.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = currentItem.AddComponent<Rigidbody>();
                }
                rb.useGravity = true;
                rb.freezeRotation = false;

                Collider collider = currentItem.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }

                currentItem.transform.SetParent(null);
                currentItem.transform.localScale = Vector3.one;
            }
        }

    }

    public void RemoveItem(int index)
    {
        Debug.Log(index);
        Transform item = Holder.GameObject().transform.GetChild(index);
        item.gameObject.SetActive(false);
    }


    public int GetKey(int i)
    {

        int childCount = Holder.GameObject().transform.childCount;

        for (int index = 0; index < childCount; index++)
        {
            Transform child = Holder.GameObject().transform.GetChild(index);

            ObjectInteractable1 keyInteractable = child.GetComponent<ObjectInteractable1>();

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
