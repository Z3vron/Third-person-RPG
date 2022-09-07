using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_slots : MonoBehaviour{

    [Header("Weapons")]
    public GameObject Weapon_1;
    public GameObject Weapon_2;
    public GameObject Weapon_3;
    public GameObject Weapon_4;
    public GameObject Weapon_5;
    public GameObject Weapon_6;
    public List<GameObject> inventory_weapons_images_slots = new List<GameObject>();
    public List<Slot> inventory_weapons_slots = new List<Slot>();

    [Header("Materials")]
    public GameObject Material_1;
    public GameObject Material_2;
    public GameObject Material_3;
    public GameObject Material_4;
    public GameObject Material_5;
    public GameObject Material_6;
    public List<GameObject> inventory_materials_images_slots = new List<GameObject>();
    public List<Slot> inventory_materials_slots = new List<Slot>();
    
    [Header("Items")]
    public GameObject Item_1;
    public GameObject Item_2;
    public GameObject Item_3;
    public GameObject Item_4;
    public GameObject Item_5;
    public GameObject Item_6;
    public List<GameObject> inventory_items_images_slots = new List<GameObject>();
    public List<Slot> inventory_items_slots = new List<Slot>();

    [Header("Potions")]
    public GameObject potion_01;
    public GameObject potion_02;
    public GameObject potion_03;
    public GameObject potion_04;
    public GameObject potion_05;
    public GameObject potion_06;
    public List<GameObject> inventory_potions_images_slots = new List<GameObject>();
    public List<Slot> inventory_potions_slots = new List<Slot>();

    private void Start() {
        gameObject.SetActive(false);
        //weapons
       
        inventory_weapons_images_slots.Add(Weapon_1);
        inventory_weapons_images_slots.Add(Weapon_2);
        inventory_weapons_images_slots.Add(Weapon_3);
        inventory_weapons_images_slots.Add(Weapon_4);
        inventory_weapons_images_slots.Add(Weapon_5);
        inventory_weapons_images_slots.Add(Weapon_6);
        inventory_weapons_slots.Add(Weapon_1.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_weapons_slots.Add(Weapon_2.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_weapons_slots.Add(Weapon_3.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_weapons_slots.Add(Weapon_4.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_weapons_slots.Add(Weapon_5.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_weapons_slots.Add(Weapon_6.transform.parent.GetComponent<Drop_slot>().slot);
        for(int i=0;i<6;i++){
            inventory_weapons_slots[i].stack_amount = 0;
            inventory_weapons_slots[i].item = null;
        }
        
       
        //materials
        inventory_materials_images_slots.Add(Material_1);
        inventory_materials_images_slots.Add(Material_2);
        inventory_materials_images_slots.Add(Material_3);
        inventory_materials_images_slots.Add(Material_4);
        inventory_materials_images_slots.Add(Material_5);
        inventory_materials_images_slots.Add(Material_6);

        inventory_materials_slots.Add(Material_1.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_materials_slots.Add(Material_2.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_materials_slots.Add(Material_3.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_materials_slots.Add(Material_4.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_materials_slots.Add(Material_5.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_materials_slots.Add(Material_6.transform.parent.GetComponent<Drop_slot>().slot);

        for(int i=0;i<6;i++){
            inventory_materials_slots[i].stack_amount = 0;
            inventory_materials_slots[i].item = null;
        }

        //Items
        
        inventory_items_images_slots.Add(Item_1);
        inventory_items_images_slots.Add(Item_2);
        inventory_items_images_slots.Add(Item_3);
        inventory_items_images_slots.Add(Item_4);
        inventory_items_images_slots.Add(Item_5);
        inventory_items_images_slots.Add(Item_6);

        inventory_items_slots.Add(Item_1.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_items_slots.Add(Item_2.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_items_slots.Add(Item_3.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_items_slots.Add(Item_4.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_items_slots.Add(Item_5.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_items_slots.Add(Item_6.transform.parent.GetComponent<Drop_slot>().slot);

       for(int i=0;i<6;i++){
            inventory_items_slots[i].stack_amount = 0;
            inventory_items_slots[i].item = null;
        }

        //Potions
        
        inventory_potions_images_slots.Add(potion_01);
        inventory_potions_images_slots.Add(potion_02);
        inventory_potions_images_slots.Add(potion_03);
        inventory_potions_images_slots.Add(potion_04);
        inventory_potions_images_slots.Add(potion_05);
        inventory_potions_images_slots.Add(potion_06);

        inventory_potions_slots.Add(potion_01.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_potions_slots.Add(potion_02.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_potions_slots.Add(potion_03.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_potions_slots.Add(potion_04.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_potions_slots.Add(potion_05.transform.parent.GetComponent<Drop_slot>().slot);
        inventory_potions_slots.Add(potion_06.transform.parent.GetComponent<Drop_slot>().slot);

       for(int i=0;i<6;i++){
            inventory_potions_slots[i].stack_amount = 0;
            inventory_potions_slots[i].item = null;
        }
    }
    private void OnEnable() {
    
        //Debug.Log("Assigned images");
    }
    //could use generics - but using base class type is better ? 
    //also could use enums
    public void Add_inventory_item_icon(Item_info.Item item, int position){
        if(item.Item_icon != null){
            if(item is Weapon_info.Weapon){
                //Debug.Log("weapon");
                inventory_weapons_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
                inventory_weapons_images_slots[position].GetComponent<Image>().enabled = true;
            }
            else if(item is Materials){
                //Debug.Log("material");
                inventory_materials_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
                inventory_materials_images_slots[position].GetComponent<Image>().enabled = true;
            }
            else if(item is Potions){
                inventory_potions_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
                inventory_potions_images_slots[position].GetComponent<Image>().enabled = true;
            }
            else if(item is Item_info.Item){
                // Debug.Log("item");
                inventory_items_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
                inventory_items_images_slots[position].GetComponent<Image>().enabled = true;
            }
        }
    }
    public void Remove_inventory_item_icon(int position, List<GameObject> list){
        list[position].GetComponent<Image>().sprite = null;
        list[position].GetComponent<Image>().enabled = false;
        list[position].GetComponentInChildren<Text>().enabled = false;

        // if(item_type is Weapon_info.Weapon){
        //     inventory_weapons_images_slots[position].GetComponent<Image>().sprite = null;
        //     inventory_weapons_images_slots[position].GetComponent<Image>().enabled = false;
        //     inventory_weapons_images_slots[position].GetComponentInChildren<Text>().enabled = false;
        // }
        // else if(item_type is Materials){
        //     inventory_materials_images_slots[position].GetComponent<Image>().sprite = null;
        //     inventory_materials_images_slots[position].GetComponent<Image>().enabled = false;
        //     inventory_materials_images_slots[position].GetComponentInChildren<Text>().enabled = false;
        // }
        // else if (item_type is Item_info.Item){
        //     inventory_items_images_slots[position].GetComponent<Image>().sprite = null;
        //     inventory_items_images_slots[position].GetComponent<Image>().enabled = false;
        //     inventory_items_images_slots[position].GetComponentInChildren<Text>().enabled = false;
        // }
    }
    public void Check_item_amount(int position, List<GameObject> icon_list, List<Slot> slot_list){
        if(slot_list[position].stack_amount > 1){
            icon_list[position].GetComponentInChildren<Text>().text = icon_list[position].transform.parent.GetComponent<Drop_slot>().slot.stack_amount.ToString();
            icon_list[position].GetComponentInChildren<Text>().enabled = true;
        }
        else if(slot_list[position].stack_amount == 1){
            icon_list[position].GetComponentInChildren<Text>().enabled = false;
        }
    }
}
