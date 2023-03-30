using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots;
    private List<Item> items = new List<Item>();

    private void Awake()
    {
        slots = this.GetComponentsInChildren<InventorySlot>();
        for(int i =0; i<slots.Length; i++)
        {
            //slots[i].Clear();
        }
    }

    public void AddItem(Item item)
    {
        if(items.Count < slots.Length)
            items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
