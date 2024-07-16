using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    public int selectedItem = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectItem();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSelection();
    }

    void WeaponSelection()
    {
        int previousSelectedWeapon = selectedItem;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedItem >= transform.childCount - 1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedItem <= 0)
            {
                selectedItem = transform.childCount - 1;
            }
            else
            {
                selectedItem--;
            }
        }

        if (previousSelectedWeapon != selectedItem)
        {
            SelectItem();
        }
    }

    public void SelectItem()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < childCount; i++)
        {
            Transform item = transform.GetChild(i);
            item.gameObject.SetActive(i == selectedItem);
        }
    }

    public GameObject GetCurrentItem()
    {
        if (selectedItem >= 0 && selectedItem < transform.childCount)
        {
            Transform selectedTransform = transform.GetChild(selectedItem);
            if (selectedTransform != null)
            {
                Debug.Log(selectedTransform.gameObject.name);
                return selectedTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("Selected transform is null.");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Selected item index is out of range.");
            return null;
        }
    }
}
