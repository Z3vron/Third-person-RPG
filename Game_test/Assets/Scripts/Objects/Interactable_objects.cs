using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_objects : MonoBehaviour
{
   
    public float Distance_to_interact = 0.9f;
    public string interactable_text;
    // could use scripttable objects - slots to store data in objkect - than no list - not sure which option is better
    public List<Item_info.Item> items_in_object = new List<Item_info.Item>();
    public List<int> obj_inv_item_amount_on_slot = new List<int>();
    public int amount_of_item_slots = 6;

    private void Start() {
        for(int i=0;i<items_in_object.Count;i++){
            if(items_in_object[i] is Weapon_info.Weapon ){
                Weapon_info.Weapon temp;
                temp = (Weapon_info.Weapon) items_in_object[i];
                temp.Reset_durability();
            }
            else  if(items_in_object[i] is Whetstones ){
                Whetstones temp;
                temp = (Whetstones) items_in_object[i];
                temp.Reset_durability();
            }  
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,Distance_to_interact);
        
    }
    //parametr item_amount i unnecessary because func Set_object_inv_weapons_amount from inventories set anount of each weapon from _obj_slots to _object internal lists
    public void Add_item_to_object(Item_info.Item item, int item_amount){
        items_in_object.Add(item);
        obj_inv_item_amount_on_slot.Add(item_amount);
    }


    //could change to function with one argument becasue always delete weapon and responding to it it's count
    //weapon_index will be always the same as weapon_amount_index
    public void Remove_item_from_object(int item_index){
        //this removes first element of this type in lsit not specific element
        // weapons_in_object.Remove(weapons_in_object[weapon_index]);
        // inventory_weapons_amount_on_slot.Remove(inventory_weapons_amount_on_slot[weapon_index]);
       
        items_in_object.RemoveAt(item_index);
        obj_inv_item_amount_on_slot.RemoveAt(item_index);
    }
    public void Change_item_amount(int item_amount_index,  int amount_to_add){
        obj_inv_item_amount_on_slot[item_amount_index] += amount_to_add;
    }
    
}
