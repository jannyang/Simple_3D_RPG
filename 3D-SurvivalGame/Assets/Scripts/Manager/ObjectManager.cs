using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public ItemData[] items;
    public Resource[] resources;
    public BuildingData[] buildings;
    public NPCData[] npcs;

    public static ObjectManager instance;

    private void Awake()
    {
        instance = this;

        items = Resources.LoadAll<ItemData>("Items");
        buildings = Resources.LoadAll<BuildingData>("Buildings");
        npcs = Resources.LoadAll<NPCData>("NPCs");
    }

    private void Start()
    {
        resources = FindObjectsOfType<Resource>();
    }

    public ItemData GetItemByID(string id)
    {
        for(int i = 0; i< items.Length; i++)
        {
            if (items[i].id == id)
                return items[i];
        }

        Debug.LogError("No item has been found");
        return null;
    }

    public BuildingData GetBuildingByID(string id)
    {
        for (int i = 0; i< buildings.Length; i++)
        {
            if (buildings[i].id == id)
                return buildings[i];
        }

        Debug.LogError("No buildings has been found");
        return null;
    }

    public NPCData GetNPCByID(string id)
    {
        for(int i = 0; i<npcs.Length; i++)
        {
            if (npcs[i].id == id)
                return npcs[i];
        }

        Debug.LogError("No npc has been found");
        return null;
    }


}
