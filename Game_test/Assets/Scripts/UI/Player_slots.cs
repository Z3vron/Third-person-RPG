using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_slots : MonoBehaviour{

    public Image left_weapon_icon;
    public Image right_weapon_icon;

    public void Update_weapon_icon(Weapon_info.Weapon weapon, bool isRight){
        if(isRight){
            if(weapon.Item_icon != null){
                right_weapon_icon.sprite = weapon.Item_icon;
                right_weapon_icon.enabled = true;
            }
            else{
                right_weapon_icon.enabled = false;
            } 
        }
        else{
            if(weapon.Item_icon != null){
                left_weapon_icon.sprite = weapon.Item_icon;
                left_weapon_icon.enabled = true;    
            }
            else{
                left_weapon_icon.enabled = false;
            }
        }
    }
}
