using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public Sprite icon;
    public int value;

    public Item(string name, string description, Sprite icon, int value)
    {
        this.name = name;
        this.description = description;
        this.icon = icon;
        this.value = value;
    }

    public void Use()
    {

    }
}