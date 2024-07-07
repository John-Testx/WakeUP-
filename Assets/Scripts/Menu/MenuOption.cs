using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuOption : MonoBehaviour
{
    public string option;
    Material objectMaterial;
    public Material meshTexture;
    public MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        objectMaterial = mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        

        if (meshTexture != null)
        {
            mesh.material = meshTexture;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Accessing " + option);
        }
    }

    private void OnMouseExit() 
    {
        mesh.material = objectMaterial;
    }
}
