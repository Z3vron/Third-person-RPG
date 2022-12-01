using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creation_inv_slots : Inventory_slots{
    public List<Image> creation_inv_items_images_slots = new List<Image>();
    private Creation_inv _creation_inv_recipes;
    private void Awake() {
        _creation_inv_recipes = GetComponent<Creation_inv>();
    }
    private void Start() {
        gameObject.SetActive(false);
    }
    public void Update_crafting_inventory_UI(){

        for(int i = 0; i < _creation_inv_recipes.recipes_for_items.Count; i++){
            Add_inventory_item_icon(_creation_inv_recipes.recipes_for_items[i].item_to_create,creation_inv_items_images_slots,i);
        }
        for(int i = _creation_inv_recipes.recipes_for_items.Count; i < _creation_inv_recipes.amount_of_items_possible_to_create; i++){
            Remove_inventory_item_icon(creation_inv_items_images_slots,i);
        }
         
    }
}
