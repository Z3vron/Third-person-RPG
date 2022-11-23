using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_slots : MonoBehaviour{


    public void Add_inventory_item_icon(Item_info.Item item,List<Image> list_of_icons,int position){
        if(item.Item_icon == null)
            return;
        list_of_icons[position].GetComponent<Image>().sprite = item.Item_icon;
        list_of_icons[position].GetComponent<Image>().enabled = true;
    }
    public void Remove_inventory_item_icon(List<Image> list_of_icons,int position){
        list_of_icons[position].sprite = null;
        list_of_icons[position].enabled = false;
    }
    public void Remove_invenotry_item_amount(List<Image> list_of_icons,int position){
        list_of_icons[position].GetComponentInChildren<Text>().text = null;
        list_of_icons[position].GetComponentInChildren<Text>().enabled = false;
    }
    public void Check_item_amount(int position, List<Image> icon_list, List<Slot> slot_list){
        if(slot_list[position].stack_amount > 1){
            icon_list[position].GetComponentInChildren<Text>().text = icon_list[position].transform.parent.GetComponent<Drop_slot>().slot.stack_amount.ToString();
            icon_list[position].GetComponentInChildren<Text>().enabled = true;
        }
        else if(slot_list[position].stack_amount == 1){
            icon_list[position].GetComponentInChildren<Text>().enabled = false;
        }
    }
}
