using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventroy);
    }

    private void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventroy);
    }
    private void OnOpenInventroy()
    {
        gameObject.SetActive(false);
    }
}
