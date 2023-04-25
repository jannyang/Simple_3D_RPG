using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public CraftingRecipeUI[] recipeUIs;

    public static CraftingWindow instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
    }

    private void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
    }

    private void OnOpenInventory()
    {
        gameObject.SetActive(false);
    }

    public void Craft(CraftingRecipe recipe)
    {
        for(int i =0; i< recipe.cost.Length; i++)
        {
            Inventory.instance.RemoveItem(recipe.cost[i].item);
        }

        Inventory.instance.AddItem(recipe.itemToCraft);

        for(int i = 0; i<recipeUIs.Length; i++)
        {
            recipeUIs[i].UpdateCanCraft();
        }
    }
}
