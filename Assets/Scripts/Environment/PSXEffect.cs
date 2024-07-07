using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSXEffect : MonoBehaviour
{
    private Camera currentActiveCamera;
    public RenderTexture PSXTexture;
    [SerializeField] private GameObject PSXRenderer;
    public bool PSXOn;
    public delegate void OnCameraChanged(Camera camera);
    public static event OnCameraChanged NotifyCameraChanged;
    public delegate void UpdatePSX (RenderTexture render,bool psxOn);
    public static event UpdatePSX ChangePSXCamera;



    private void OnEnable()
    {
        ChangePSXCamera += SetPSXEffect;
    }

    private void OnDisable()
    {
        ChangePSXCamera -= SetPSXEffect;
    }

    public void SetPSXEffect(RenderTexture PSXTexture, bool psxOn)
    {
        if (currentActiveCamera != null)
        {
            PSXOn = psxOn;

            if (psxOn && PSXTexture != null)
            {
                Debug.Log($"PSX effect applied to {currentActiveCamera.name}");
                currentActiveCamera.targetTexture = PSXTexture;
            }
            else
            {
                currentActiveCamera.targetTexture = null;
            }
        }

    }

    public void HandlePSX(bool psxOn) 
    {
        ChangePSXCamera?.Invoke(PSXTexture,psxOn);
    }


    void Update()
    {
        // Find the camera that is rendering the UI
        currentActiveCamera = FindActiveCamera();

  
    }

    private Camera FindActiveCamera()
    {
        // Iterate through all cameras in the scene
        foreach (Camera cam in Camera.allCameras)
        {
            // Check if the camera is enabled and rendering to a RenderTexture or the screen
            if (cam.enabled && (cam.targetTexture == null || cam == Camera.main))
            {
                if (PSXOn)
                {
                    ChangePSXCamera?.Invoke(PSXTexture, PSXOn);
                }

                // Return the first active camera found
                Debug.Log("Currently active camera: " + cam.name);
                return cam;
            }
        }

        // Return null if no active camera is found
        return null;
    }


}
