using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SaveManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerPrefs.HasKey("Save"))
            Load();
    }

    private void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
            Save();

        if (Keyboard.current.mKey.wasPressedThisFrame)
            Load();
    }
    private void Save()
    {
        SaveData data = new SaveData();

        data.playerPos = new SVec3(PlayerController.instance.transform.position).GetVector3();
        data.playerRot = new SVec3(PlayerController.instance.transform.eulerAngles).GetVector3();
        data.playerLook = new SVec3(PlayerController.instance.cameraContainer.localEulerAngles).GetVector3();

        data.health = PlayerNeeds.instance.health.curValue;
        data.hunger = PlayerNeeds.instance.hunger.curValue;
        data.thirst = PlayerNeeds.instance.thirst.curValue;
        data.sleep = PlayerNeeds.instance.sleep.curValue;

        data.inventory = new SInventorySlot[Inventory.instance.slots.Length];

        for(int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            data.inventory[i] = new SInventorySlot();
            data.inventory[i].occupied = Inventory.instance.slots[i].item != null;

            if (!data.inventory[i].occupied)
                continue;

            data.inventory[i].itemId = Inventory.instance.slots[i].item.id;
            data.inventory[i].quantity = Inventory.instance.slots[i].quantity;
            data.inventory[i].equipped = Inventory.instance.uiSlots[i].equipped;
        }

        ItemObject[] droppedItems = FindObjectsOfType<ItemObject>();
        data.droppedItems = new SDroppedItem[droppedItems.Length];

        for (int i = 0; i < droppedItems.Length; i++)
        {
            data.droppedItems[i] = new SDroppedItem();
            data.droppedItems[i].itemId = droppedItems[i].item.id;
            data.droppedItems[i].position = new SVec3(droppedItems[i].transform.position);
            data.droppedItems[i].rotation = new SVec3(droppedItems[i].transform.eulerAngles);
        }

        Building[] buildingObjects = FindObjectsOfType<Building>();
        data.buildings = new SBuilding[buildingObjects.Length];

        for(int i = 0; i<buildingObjects.Length; i++)
        {
            data.buildings[i] = new SBuilding();
            data.buildings[i].buildingId = buildingObjects[i].data.id;
            data.buildings[i].position = new SVec3(buildingObjects[i].transform.position);
            data.buildings[i].rotation = new SVec3(buildingObjects[i].transform.eulerAngles);
            data.buildings[i].customProperties = buildingObjects[i].GetCustomProperties();
        }

        data.resources = new SResource[ObjectManager.instance.resources.Length];

        for(int i = 0; i<ObjectManager.instance.resources.Length; i++)
        {
            data.resources[i] = new SResource();
            data.resources[i].index = i;
            data.resources[i].destroyed = ObjectManager.instance.resources[i] == null;
            if (!data.resources[i].destroyed)
                data.resources[i].capacity = ObjectManager.instance.resources[i].capacity;
        }

        NPC[] npcs = FindObjectsOfType<NPC>();
        data.npcs = new SNPC[npcs.Length];

        for (int i = 0; i < npcs.Length; i++)
        {
            data.npcs[i] = new SNPC();
            data.npcs[i].prefabId = npcs[i].data.id;
            data.npcs[i].position = new SVec3(npcs[i].transform.position);
            data.npcs[i].rotation = new SVec3(npcs[i].transform.eulerAngles);
            data.npcs[i].aiState = (int)npcs[i].aiState;
            data.npcs[i].hasAgentDestination = !npcs[i].agent.isStopped;
            data.npcs[i].agentDestination = new SVec3(npcs[i].agent.destination);
        }

        data.timeOfDay = DayNightCycle.instance.time;

        string rawData = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("Save", rawData);
    }

    private void Load()
    {
        SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("Save"));

        PlayerController.instance.transform.position = data.playerPos;
        PlayerController.instance.transform.eulerAngles = data.playerRot;
        PlayerController.instance.cameraContainer.localEulerAngles = data.playerLook;

        PlayerNeeds.instance.health.curValue = data.health;
        PlayerNeeds.instance.hunger.curValue = data.hunger;
        PlayerNeeds.instance.thirst.curValue = data.thirst;
        PlayerNeeds.instance.sleep.curValue = data.sleep;

        int equippedItem = 999;

        for (int i = 0; i < data.inventory.Length; i++)
        {
            if (!data.inventory[i].occupied)
                continue;

            Inventory.instance.slots[i].item = ObjectManager.instance.GetItemByID(data.inventory[i].itemId);
            Inventory.instance.slots[i].quantity = data.inventory[i].quantity;

            if (data.inventory[i].equipped)
            {
                equippedItem = i;
            }
        }
        
        if (equippedItem != 999)
        {
            Inventory.instance.SelectItem(equippedItem);
            Inventory.instance.OnEquipButton();
        }
        
        ItemObject[] droppedItems = FindObjectsOfType<ItemObject>();

        for (int i = 0; i < droppedItems.Length; i++)
        {
            Destroy(droppedItems[i].gameObject);
        }

        for (int i = 0; i < data.droppedItems.Length; i++)
        {
            GameObject prefab = ObjectManager.instance.GetItemByID(data.droppedItems[i].itemId).dropPrefab;
            Instantiate(prefab, data.droppedItems[i].position.GetVector3(), Quaternion.Euler(data.droppedItems[i].rotation.GetVector3()));
        }

        Building[] buildings = FindObjectsOfType<Building>();

        for(int i = 0; i < buildings.Length; i++)
            Destroy(buildings[i].gameObject);
        
        for(int i =0; i < data.buildings.Length; i++)
        {
            GameObject prefab = ObjectManager.instance.GetBuildingByID(data.buildings[i].buildingId).spawnPrefab;
            GameObject building = Instantiate(prefab, data.buildings[i].position.GetVector3(), Quaternion.Euler(data.buildings[i].rotation.GetVector3()));
            building.GetComponent<Building>().ReceiveCustomProperties(data.buildings[i].customProperties);
        }

        for(int i=0; i<ObjectManager.instance.resources.Length; i++)
        {
            if(data.resources[i].destroyed)
            {
                if(ObjectManager.instance.resources[i] != null)
                {
                    Destroy(ObjectManager.instance.resources[i].gameObject);
                }

                continue;
            }

            ObjectManager.instance.resources[i].capacity = data.resources[i].capacity;
        }

        NPC[] npcs = FindObjectsOfType<NPC>();

        for (int i = 0; i < npcs.Length; i++)
            Destroy(npcs[i].gameObject);

        for(int i = 0; i<data.npcs.Length; i++)
        {
            GameObject prefab = ObjectManager.instance.GetNPCByID(data.npcs[i].prefabId).spawnPrefab;
            GameObject npcObj = Instantiate(prefab, data.npcs[i].position.GetVector3(), Quaternion.Euler(data.npcs[i].rotation.GetVector3()));
            NPC npc = npcObj.GetComponent<NPC>();

            npc.aiState = (AIState)data.npcs[i].aiState;
            npc.agent.isStopped = !data.npcs[i].hasAgentDestination;

            if (!npc.agent.isStopped)
                npc.agent.SetDestination(data.npcs[i].agentDestination.GetVector3());
        }

        DayNightCycle.instance.time = data.timeOfDay;
    }
}
