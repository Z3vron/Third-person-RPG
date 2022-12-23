using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Weapon_slot_manager{
    public class Weapon_manager : MonoBehaviour{

        private Weapon_holder.Weapon_armed _left_slot;
        private Weapon_holder.Weapon_armed _right_slot;
        private Weapon_damage_collider_player _left_hand_weapon_collider;
        private Weapon_damage_collider_player _right_hand_weapon_collider;
        private Player_slots _player_quick_access_slots;
        private void Awake() {
            Weapon_holder.Weapon_armed[] Weapons_slots = GetComponentsInChildren<Weapon_holder.Weapon_armed>();
            foreach (Weapon_holder.Weapon_armed slot in Weapons_slots){
                if(slot.left_hand){
                    _left_slot = slot;
                }
                else if(slot.right_hand){
                    _right_slot = slot;
                }
            }
            _player_quick_access_slots = FindObjectOfType<Player_slots>();   
        }
        public void Load_weapon_to_slot(Weapon_info.Weapon weapon, bool hand){
            //load to right hand
            if(hand){
                _right_slot.Equip_weapon(weapon);
                _player_quick_access_slots.Update_quick_slot_weapon_icon(weapon,true);
               // Debug.Log("Right");
            }
            //Load to left hand
            else{
                _left_slot.Equip_weapon(weapon);
                _player_quick_access_slots.Update_quick_slot_weapon_icon(weapon,false);  
               // Debug.Log("Left");            
            }
        }
        private void Update() {
            //for weapon
            if(_left_slot.current_weapon.unarmed == false){   
                if(_left_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Left_Pivot.GetComponent<Hand_slot_activator>().activated){
                    assign_left_weapon_collider();
                    _left_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Left_Pivot.GetComponent<Hand_slot_activator>().activated = false;
                }   
            }
            //for fists
            else{
                assign_left_weapon_collider();
            }
            if(_right_slot.current_weapon.unarmed == false){
                if(_right_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>() != null){
                    if(_right_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Right_Pivot.GetComponent<Hand_slot_activator>().activated){
                        assign_right_weapon_collider();
                        _right_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Right_Pivot.GetComponent<Hand_slot_activator>().activated = false;
                    }
                }    
                else{
                    assign_right_weapon_collider();
                }
            }
            else{
               
                assign_right_weapon_collider();
            }
        }
        public void assign_left_weapon_collider(){
            _left_hand_weapon_collider = _left_slot.current_Weapon_model_instantiated.GetComponentInChildren<Weapon_damage_collider_player>();
                // if(_left_hand_weapon_collider == null)
                //     Debug.Log("left error ");
                // else
                //     Debug.Log("Left not error");
        }
        public void assign_right_weapon_collider(){
             
            _right_hand_weapon_collider = _right_slot.current_Weapon_model_instantiated.GetComponentInChildren<Weapon_damage_collider_player>();
        }
        public void Enable_collider_on_right_weapon(){
            _right_hand_weapon_collider.Enable_collider();
        }
        public void Disable_collider_on_right_weapon(){
            _right_hand_weapon_collider.Disable_collider();
        }
        public void Enable_collider_on_left_weapon(){
           // Debug.Log("test");
            _left_hand_weapon_collider.Enable_collider();
        }
        public void Disable_collider_on_left_weapon(){
            _left_hand_weapon_collider.Disable_collider();
        }
    }
}
    
