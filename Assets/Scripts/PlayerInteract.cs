using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] Camera playerCam;
    public PlayerCamera PlayerCamera;
    [SerializeField] Player1 player;
    public UIText UIText;
    [SerializeField] private int interactDistance;
    Ray ray;

    // Update is called once per frame
    void Update()
    {
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
        ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hitData;
        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 1.2f);
        Physics.Raycast(ray, out hitData, interactDistance);

        if (UIText != null)
        {
            if (hitData.collider != null)
            {
                if (hitData.collider.TryGetComponent(out IInteractable inter))
                {

                    UIText.ShowInteractMessage(true, inter.GetInteractText());
                }
                else { UIText.ShowInteractMessage(false, ""); }

                //Debug.Log(hitData.transform.gameObject.name);
                if (Input.GetKeyDown(KeyCode.E) && hitData.collider.TryGetComponent(out IInteractable interactable))
                {

                    UIText.ShowInteractMessage(false, "");
                    interactable.InteractWithPlayer(player);

                }
            }
            else { UIText.ShowInteractMessage(false, ""); }
        }
    }
    void HandleVehicleInteraction()
    {
        float interactRange = 3f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliders)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
