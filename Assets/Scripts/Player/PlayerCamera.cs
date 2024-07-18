using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerCamera : MonoBehaviour
{

    public enum CameraType
    {
        Player,
        Vehicle,
    }

    public CameraType cameraType;
    public Transform Objeto;
    public Transform arms;
    public static List<PlayerCamera> playerCameras = new List<PlayerCamera>(); // List to store all PlayerCamera instances
    public Camera playerCamera;
    private float RotacionVertical = 0; //Eje de rotacion X
    private float RotacionHorizontal = 0; //Eje de rotacion Y
    [SerializeField] private bool canRotate;
    [SerializeField] private static bool moveMouse = true;

    [SerializeField] private Vector2 mouseSensitivity;
    public Vector2 MouseSensitivity { get { return mouseSensitivity; } set { mouseSensitivity = value; } }


    void Awake()
    {
        playerCameras.Add(this); // Add this instance to the list of PlayerCameras
    }


    public static bool SetMoveMouse(bool move) => moveMouse = move; 

    void Start()
    {

        if (moveMouse)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

    }

    public void SetPSXEffect(RenderTexture PSXTexture) 
    {
        if (playerCamera != null)
        {
            playerCamera.targetTexture = PSXTexture;
        }
    }

    void Update()
    {
        
        RegularCameraMovement();

        //float angle = camara.localEulerAngles.x - mouseY * mouseSensivity;
        //camara.localEulerAngles= Vector3.right * angle;
    }

    void RegularCameraMovement()
    {
        if (moveMouse && canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            RotacionVertical += -mouseY;
            RotacionHorizontal = mouseX;

            // GetHorizontalRotation();

            GetVerticalRotation();//Esto hace que no pueda pasar de 80 o -80 en el eje de rotacion X

            if (arms == null) { transform.localRotation = Quaternion.Euler(RotacionVertical, 0, 0);     }
            else { arms.localRotation = Quaternion.Euler(RotacionVertical, 0, 0);   }
            
            if (Objeto != null)
            {
                Objeto.Rotate(Vector3.up * RotacionHorizontal);
            }
        }
    }


    void GetHorizontalRotation()
    {
        switch (cameraType)
        {
            case CameraType.Vehicle:
                RotacionHorizontal = Mathf.Clamp(RotacionHorizontal, -45f, 45f);
                break;
        }
    }

    void GetVerticalRotation()
    {
        switch (cameraType)
        {
            case CameraType.Player:
                RotacionVertical = Mathf.Clamp(RotacionVertical, -90f, 71f);
                break;
            case CameraType.Vehicle:
                RotacionVertical = Mathf.Clamp(RotacionVertical, -90f, 0f);
                break;
        }
        
    }

}
