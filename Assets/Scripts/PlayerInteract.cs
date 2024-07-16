using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] Camera playerCam;
    public PlayerCamera PlayerCamera;
    [SerializeField] Player1 player;
    [SerializeField] KeyCode interactKey;
    public UIText UIText;
    [SerializeField] private int interactDistance;
    Ray ray;

    // Update is called once per frame
    void Update()
    {
        if (PlayerCamera == null) return;

        switch (PlayerCamera.cameraType)
        {
            case PlayerCamera.CameraType.Player:
                HandlePlayerInteraction();
                break;
            case PlayerCamera.CameraType.Vehicle:
                HandleVehicleInteraction();
                break;
        }
    }

    void HandlePlayerInteraction()
    {
        if (playerCam == null || UIText == null) return;

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitData, interactDistance )) //, interactionMask
        {
            ProcessRaycastHit(hitData);
        }
        else
        {
            UIText.ShowInteractMessage(false, "");
        }
    }

    private void ProcessRaycastHit(RaycastHit hitData)
    {
        if (hitData.collider != null)
        {
            if (hitData.collider.TryGetComponent(out IInteractable interactable))
            {
                UIText.ShowInteractMessage(true, interactable.GetInteractText());

                if (Input.GetKeyDown(interactKey))
                {
                    UIText.ShowInteractMessage(false, "");
                    interactable.InteractWithPlayer(player);
                }
            }
            else{   UIText.ShowInteractMessage(false, "");  }
        }
        else{  UIText.ShowInteractMessage(false, "");  }
    }


    void HandleVehicleInteraction()
    {
        float interactRange = 3f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliders)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    UIText.ShowInteractMessage(true, interactable.GetInteractText());
                    interactable.InteractWithPlayer(player);
                }
            }
        }
    }
}
