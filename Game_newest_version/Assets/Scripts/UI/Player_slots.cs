using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_slots : Inventory_slots{
    public List<Image> quick_slots_potions_icons;
    [SerializeField] private  Image _quick_slots_left_weapon_icon;
    [SerializeField] private  Image _quick_slots_right_weapon_icon;
   
    
    public void Update_quick_slot_weapon_icon(Weapon_info.Weapon weapon, bool isRight){
        if(isRight){
            if(weapon.Item_icon != null){
                _quick_slots_right_weapon_icon.sprite = weapon.Item_icon;
                _quick_slots_right_weapon_icon.enabled = true;
            }
            else{
                _quick_slots_right_weapon_icon.sprite = null;
                _quick_slots_right_weapon_icon.enabled = false;
            } 
        }
        else{
            if(weapon.Item_icon != null){
                _quick_slots_left_weapon_icon.sprite = weapon.Item_icon;
                _quick_slots_left_weapon_icon.enabled = true;    
            }
            else{
                _quick_slots_left_weapon_icon.sprite = null;
                _quick_slots_left_weapon_icon.enabled = false;
            }
        }
    }
    //slot_number is number 1-4 counting from the left side
    public void Update_quick_slot_potions_icon(Potions potion, int slot_number){
        if(potion.Item_icon == null){
            quick_slots_potions_icons[slot_number].sprite = null;
            quick_slots_potions_icons[slot_number].enabled = false;
            return;
        }
        quick_slots_potions_icons[slot_number].sprite = potion.Item_icon;
        quick_slots_potions_icons[slot_number].enabled = true;
    }
    public void Update_quick_slots_potions_amount(Player_inventory_info.Player_inventory player_inventory){
        for(int i=0;i<4;i++){
            Check_item_amount(i,quick_slots_potions_icons,player_inventory.quick_slots_potions);
        }
    }
}