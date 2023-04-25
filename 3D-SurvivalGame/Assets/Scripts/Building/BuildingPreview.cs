using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public Material canPlaceMaterial;
    public Material cannotPlaceMaterial;
    private MeshRenderer[] meshRenderers;
    private List<GameObject> collidingObjects = new List<GameObject>();

    private void Awake()
    {
        meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
    }

    public void CanPlace()
    {
        SetMaterial(canPlaceMaterial);
    }

    public void CannotPlace()
    {
        SetMaterial(cannotPlaceMaterial);
    }

    private void SetMaterial(Material mat)
    {
        for(int i = 0; i< meshRenderers.Length; i++)
        {
            Material[] mats = new Material[meshRenderers[i].materials.Length];

            for(int j = 0; j<mats.Length; j++)
            {
                mats[j] = mat;
            }

            meshRenderers[i].materials = mats;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 12)
            collidingObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 12)
            collidingObjects.Remove(other.gameObject);
    }

    public bool CollidingWithObjects()
    {
        collidingObjects.RemoveAll(x => x == null);
        return collidingObjects.Count > 0;
    }    
}
