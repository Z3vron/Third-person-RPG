using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_inventory_slots : Inventory_slots{

    public List<Image> obj_inv_items_images_slots = new List<Image>();
    public List<Slot> obj_inv_items_slots = new List<Slot>();
    private void Start() {
        for(int i=0;i < 6;i++){
            if(obj_inv_items_images_slots[i].transform.parent.TryGetComponent(out Drop_slot drop_slot)){
                obj_inv_items_slots.Add(drop_slot.slot);
            }
        }
        gameObject.SetActive(false);
    }
    public void Update_inventory_object_UI(Interactable_objects object_inv_to_show){
        if(object_inv_to_show == null)
            return ;
        for(int i=0;i<object_inv_to_show.items_in_object.Count ;i++){
            Add_inventory_item_icon(object_inv_to_show.items_in_object[i],obj_inv_items_images_slots,i);
            Check_item_amount(i,obj_inv_items_images_slots,obj_inv_items_slots);
        }
        for(int i=object_inv_to_show.items_in_object.Count;i<object_inv_to_show.amount_of_item_slots;i++){
            Remove_inventory_item_icon(obj_inv_items_images_slots,i);
            Remove_invenotry_item_amount(obj_inv_items_images_slots,i);
        }
    }
}
