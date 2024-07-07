using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public List<Camera> cameras;
    public Transform playerRef;
    public Material emptyTexture;
    public int maxScreenDistance;
    RenderTexture targetTexture;

    [Range(0,32)]
    public int currentCamera = 0; 

    // Start is called before the first frame update
    void Start()
    {
        DisableAllCameras();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MeshRenderer material = GetComponent<MeshRenderer>();
        
        float distance = Vector3.Distance(playerRef.position, transform.position);
        
        if (distance > maxScreenDistance) 
        {
            material.material= emptyTexture;
            DisableAllCameras();
        }
        else
        {
            EnableCamera();
            material.material.mainTexture = targetTexture;

            if (currentCamera < 0 || currentCamera >= cameras.Count) return;

            //List<Material> materials = new List<Material>();

            //materials = material.materials.ToList();

            targetTexture = cameras[currentCamera].targetTexture;

            if (targetTexture != null)
            {
                material.material.mainTexture = targetTexture;

                //materials[1].mainTexture = targetTexture;
            }
            else
            {
                Debug.LogWarning("Target texture of the current camera is null.");
            }

        }

        

    }
    
    void DisableAllCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled= false;
        }
    }
    void EnableCamera()
    {
        cameras[currentCamera].enabled = true;

        for (int i = 0; i < cameras.Count; i++)
        {
            if(i != currentCamera)
            {
                cameras[i].enabled = false;
            }    
        }

    }

    public void SetCurrentCamera(int cameraNumber)
    {
        currentCamera = cameraNumber;
    }

    public void NextCamera()
    {
        currentCamera++;
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentCamera++;
            if (currentCamera >= cameras.Count) 
            {
                currentCamera = 0;
            }
        }
    }
}
