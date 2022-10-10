using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_slots : MonoBehaviour{

    [SerializeField] private  Image _quick_slots_left_weapon_icon;
    [SerializeField] private  Image _quick_slots_right_weapon_icon;
    private List<Image> _quick_slots_potions_icons;
    
    public void Update_quick_slot_weapon_icon(Weapon_info.Weapon weapon, bool isRight){
        if(isRight){
            if(weapon.Item_icon != null){
                _quick_slots_right_weapon_icon.sprite = weapon.Item_icon;
                _quick_slots_right_weapon_icon.enabled = true;
            }
            else{
                _quick_slots_right_weapon_icon.enabled = false;
            } 
        }
        else{
            if(weapon.Item_icon != null){
                _quick_slots_left_weapon_icon.sprite = weapon.Item_icon;
                _quick_slots_left_weapon_icon.enabled = true;    
            }
            else{
                _quick_slots_left_weapon_icon.enabled = false;
            }
        }
    }
    //slot_number is number 1-4 counting from the left side
    public void Update_quick_slot_potions_icon(Potions potion, int slot_number){
        if(potion.Item_icon == null)
            return;
        _quick_slots_potions_icons[slot_number-1].sprite = potion.Item_icon;
        _quick_slots_potions_icons[slot_number-1].enabled = true;
    }
}
