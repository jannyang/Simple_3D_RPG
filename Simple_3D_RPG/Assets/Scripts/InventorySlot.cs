using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image IconImage;
    public Text countText;

    private Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
        IconImage.sprite = newItem.image;
        IconImage.enabled = true;
        countText.text = newItem.count.ToString();
    }

    public void Clear()
    {
        item = null;
        IconImage.sprite = null;
        IconImage.enabled = false;
        countText.text = "";
    }
}
