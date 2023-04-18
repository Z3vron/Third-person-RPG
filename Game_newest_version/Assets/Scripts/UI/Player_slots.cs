using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player_slots : Inventory_slots{
    public List<Image> quick_slots_potions_icons;
    [SerializeField] private  Image _quick_slots_left_weapon_icon;
    [SerializeField] private  Image _quick_slots_right_weapon_icon;
    [SerializeField] private GameObject _left_weapon_poison_indicator;
    [SerializeField] private bool _left_weapon_poisoned;
    private Image _left_weapon_poisoned_filler;
    private float _left_weapon_poison_duration;


    [SerializeField] private GameObject _right_weapon_poison_indicator;
    [SerializeField] private bool _right_weapon_poisoned;
    private Image _right_weapon_poisoned_filler;
    private float _right_weapon_poison_duration;

    
   
    private void Start() {
        _left_weapon_poisoned_filler = _left_weapon_poison_indicator.GetComponentInChildren<Image>();
        _right_weapon_poisoned_filler = _right_weapon_poison_indicator.GetComponentInChildren<Image>();

        Player_inventory_info.Player_inventory.Update_quick_slots_potion_icon += Update_quick_slot_potion_icon;
        Player_inventory_info.Player_inventory.Update_quick_slots_potions_amount_text += Update_quick_slots_potions_amount;
        Weapon_slot_manager.Weapon_manager.Update_quick_slots_weapon_icon += Update_quick_slot_weapon_icon;

        Player_inventory_info.Player_inventory.Enable_quick_slots_weapon_poison_icon += Turn_on_weapon_poison_effect_icon;
        Player_inventory_info.Player_inventory.Disable_quick_slots_weapon_poison_icon += Turn_off_weapon_poison_effect_icon;
    }
    private void Update() {
        if(_left_weapon_poisoned){
            _left_weapon_poisoned_filler.fillAmount -= 1/_left_weapon_poison_duration * Time.deltaTime;
        }
        if(_right_weapon_poisoned){
            _right_weapon_poisoned_filler.fillAmount -= 1/_right_weapon_poison_duration * Time.deltaTime;
        }
    }
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
    public void Turn_on_weapon_poison_effect_icon(bool isRight,float poison_duration,float poison_percentage=1){
        if(isRight){
            _right_weapon_poison_indicator.SetActive(true);
            _right_weapon_poisoned = true;
            _right_weapon_poison_duration = poison_duration;
            _right_weapon_poisoned_filler.fillAmount = 1-poison_percentage;
        }
        else{
            _left_weapon_poison_indicator.SetActive(true);
            _left_weapon_poisoned = true;
            _left_weapon_poison_duration = poison_duration;
            _left_weapon_poisoned_filler.fillAmount = 1-poison_percentage;
        }
    }
    public void Turn_off_weapon_poison_effect_icon(bool isRight){
        if(isRight){
            _right_weapon_poison_indicator.SetActive(false);
            _right_weapon_poisoned = false;
        }
        else{
            _left_weapon_poison_indicator.SetActive(false);
            _left_weapon_poisoned = false;
        }
    }
    //slot_number is number 1-4 counting from the left side
    public void Update_quick_slot_potion_icon(Potions potion, int slot_number){
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