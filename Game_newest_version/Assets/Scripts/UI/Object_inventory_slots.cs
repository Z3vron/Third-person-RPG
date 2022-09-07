using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_inventory_slots : MonoBehaviour
{
    [Header("Items")]
    // public GameObject Item_1;
    // public GameObject Item_2;
    // public GameObject Item_3;
    // public GameObject Item_4;
    // public GameObject Item_5;
    // public GameObject Item_6;

    public List<GameObject> obj_inv_items_images_slots = new List<GameObject>();
    public List<Slot> obj_inv_items_slots = new List<Slot>();
    private void Start() {
        //if(obj_inv_items_images_slots[0].transform.parent.TryGetComponent( out Drop_slot drop_slot)){
        for(int i=0;i < 6;i++){
            var help = obj_inv_items_images_slots[i].transform.parent.GetComponent<Drop_slot>();
            if(help != null){
                //Debug.Log("Drop slot found");
                obj_inv_items_slots.Add(obj_inv_items_images_slots[i].transform.parent.GetComponent<Drop_slot>().slot);
            }
        }
        gameObject.SetActive(false);
    }
     public void Add_inventory_item_icon(Item_info.Item item, int position){
        if(item.Item_icon != null){
            obj_inv_items_images_slots[position].GetComponent<Image>().sprite = item.Item_icon;
            obj_inv_items_images_slots[position].GetComponent<Image>().enabled = true;
        }
    }
    public void Remove_inventory_item_icon(int position, List<GameObject> list){
        list[position].GetComponent<Image>().sprite = null;
        list[position].GetComponent<Image>().enabled = false;
        //creation menu doesn't have text to display amount of items in specific slot
        if( list[position].GetComponentInChildren<Text>() != null)
            list[position].GetComponentInChildren<Text>().enabled = false;
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
