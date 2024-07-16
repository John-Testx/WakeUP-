using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swaySmoothness;
    [SerializeField] private float multiplier;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis( mouseX, Vector3.up);

        Quaternion Rotation = rotationX * rotationY;


        transform.localRotation = Quaternion.Slerp(transform.localRotation, Rotation, swaySmoothness);
    }
}

