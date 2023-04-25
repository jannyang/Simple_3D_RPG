using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingRecipeUI : MonoBehaviour
{
    public BuildingRecipe recipe;
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI itemName;
    public Image[] resourceCosts;

    public Color canBuildColor;
    public Color cannotBuildColor;
    private bool canBuild;

    private void OnEnable()
    {
        UpdateCanBuild();
    }

    private void Start()
    {
        icon.sprite = recipe.icon;
        itemName.text = recipe.displayName;

        for (int i = 0; i < resourceCosts.Length; i++)
        {
            if (i < recipe.cost.Length)
            {
                resourceCosts[i].gameObject.SetActive(true);
                resourceCosts[i].sprite = recipe.cost[i].item.icon;
                resourceCosts[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = recipe.cost[i].quantity.ToString();
            }

            else
                resourceCosts[i].gameObject.SetActive(false);
        }
    }
    public void OnClickButton()
    {
        if (canBuild)
        {
            EquipBuildingKit.instance.SetNewBuildingRecipe(recipe);
        }

        else
        {
            PlayerController.instance.ToggleCursor(false);
            EquipBuildingKit.instance.buildingWindow.SetActive(false);
        }
    }
    private void UpdateCanBuild()
    {
        canBuild = true;

        for (int i = 0; i < recipe.cost.Length; i++)
        {
            if (!Inventory.instance.HasItems(recipe.cost[i].item, recipe.cost[i].quantity))
            {
                canBuild = false;
                break;
            }
        }

        backgroundImage.color = canBuild ? canBuildColor : cannotBuildColor;
    }
}
