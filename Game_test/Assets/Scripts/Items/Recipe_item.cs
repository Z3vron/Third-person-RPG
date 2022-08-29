using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Recipe_item")]
public class Recipe_item : ScriptableObject{
    public Item_info.Item  item_to_create;
    public List<Item_info.Item> crafting_ingredients = new List<Item_info.Item>();
    
}
