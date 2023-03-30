using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool activeInventory = false;
    public void ActiveInventoryPanel()
    {
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
    }
}
