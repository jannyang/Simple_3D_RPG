using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Recipe", menuName = "New Building Recipe")]
public class BuildingRecipe : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public GameObject spawnPrefab;
    public GameObject previewPrefab;
    public BuildingResourceCost[] cost;
}

[System.Serializable]
public class BuildingResourceCost
{
    public ItemData item;
    public int quantity;
}