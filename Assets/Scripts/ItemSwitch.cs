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

    void SelectItem()
    {
        int i = 0;
        foreach (Transform item in transform)
        {
            if (i == selectedItem)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            i++;
        }
    }
    public GameObject GetCurrentItem()
    {
        if (selectedItem >= 0 && selectedItem < transform.childCount)
        {
            return transform.GetChild(selectedItem).gameObject;
        }
        else
        {
            return null; // Return null if selectedItem is out of range
        }
    }
}
