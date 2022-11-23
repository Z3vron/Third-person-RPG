using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player_inventory_slots : Inventory_slots{
    
    [Header("Weapons")]
    public List<Image> inventory_weapons_images_slots = new List<Image>();
    public List<Slot> inventory_weapons_slots = new List<Slot>();

    
    [Header("Potions")]
    public List<Image> inventory_potions_images_slots = new List<Image>();
    public List<Slot> inventory_potions_slots = new List<Slot>();
    
    [Header("Items")]
    public List<Image> inventory_items_images_slots = new List<Image>();
    public List<Slot> inventory_items_slots = new List<Slot>();

    [Header("Materials")]
    public List<Image> inventory_materials_images_slots = new List<Image>();
    public List<Slot> inventory_materials_slots = new List<Slot>();
    

    private void Start() {

        
        // compiler gives error ifI use same varaible for drop slot
        for(int i=0;i < 6;i++){
            if(inventory_weapons_images_slots[i].transform.parent.TryGetComponent( out Drop_slot drop_slot_01)){
                inventory_weapons_slots.Add(drop_slot_01.slot);
                inventory_weapons_slots[i].stack_amount = 0;
                inventory_weapons_slots[i].item = null;
            }
            if(inventory_potions_images_slots[i].transform.parent.TryGetComponent( out Drop_slot drop_slot_02)){
                inventory_potions_slots.Add(drop_slot_02.slot);
                inventory_potions_slots[i].stack_amount = 0;
                inventory_potions_slots[i].item = null;
            }
                
            if(inventory_items_images_slots[i].transform.parent.TryGetComponent( out Drop_slot drop_slot_03)){
                inventory_items_slots.Add(drop_slot_03.slot);
                inventory_items_slots[i].stack_amount = 0;
                inventory_items_slots[i].item = null;
            }
                
            if(inventory_materials_images_slots[i].transform.parent.TryGetComponent( out Drop_slot drop_slot_04)){
                inventory_materials_slots.Add(drop_slot_04.slot);
                inventory_materials_slots[i].stack_amount = 0;
                inventory_materials_slots[i].item = null;
            }
        }
        gameObject.SetActive(false);

    }
    public void Update_inventory_player_UI(Player_inventory_info.Player_inventory player_inventory){
        //weapons
        List<Item_info.Item> list_of_items = player_inventory.inventory_weapons_slots.Cast<Item_info.Item>().ToList();
        Update_inventory_player_row_UI(player_inventory,list_of_items,inventory_weapons_images_slots,inventory_weapons_slots);
        //materials
        list_of_items = player_inventory.inventory_materials_slots.Cast<Item_info.Item>().ToList();
        Update_inventory_player_row_UI(player_inventory,list_of_items,inventory_materials_images_slots,inventory_materials_slots);
        //potions
        list_of_items = player_inventory.inventory_potions_slots.Cast<Item_info.Item>().ToList();
        Update_inventory_player_row_UI(player_inventory,list_of_items,inventory_potions_images_slots,inventory_potions_slots);
        //items
        Update_inventory_player_row_UI(player_inventory,player_inventory.inventory_items_slots,inventory_items_images_slots,inventory_items_slots);
    }
    private void Update_inventory_player_row_UI(Player_inventory_info.Player_inventory player_inventory,List<Item_info.Item> list_of_items,List<Image> list_of_icons,List<Slot> list_of_slots){
        for(int i=0;i<list_of_items.Count;i++){
            Add_inventory_item_icon(list_of_items[i],list_of_icons,i);
            Check_item_amount(i,list_of_icons,list_of_slots);
        }
        for(int i=list_of_items.Count;i<player_inventory.amount_of_items_slots;i++){
            Remove_inventory_item_icon(list_of_icons,i);
            Remove_invenotry_item_amount(list_of_icons,i);
        }
    }



    // public void Add_inventory_item_icon(Item_info.Item item, int position){
    //     if(item.Item_icon != null){
    //         if(item is Weapon_info.Weapon){
    //             inventory_weapons_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
    //             inventory_weapons_images_slots[position].GetComponent<Image>().enabled = true;
    //         }
    //         else if(item is Potions){
    //             inventory_potions_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
    //             inventory_potions_images_slots[position].GetComponent<Image>().enabled = true;
    //         }
    //         else if(item is Materials){
    //             inventory_materials_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
    //             inventory_materials_images_slots[position].GetComponent<Image>().enabled = true;
    //         }
    //         else if(item is Item_info.Item){
    //             inventory_items_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
    //             inventory_items_images_slots[position].GetComponent<Image>().enabled = true;
    //         }
    //     }
    // }
   
    
}
