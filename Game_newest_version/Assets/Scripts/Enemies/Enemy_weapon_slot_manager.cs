using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_weapon_slot_manager : MonoBehaviour{
    public Weapon_info.Weapon right_hand_weapon;
    public Weapon_info.Weapon left_hand_weapon;
    [SerializeField] private  Weapon_holder.Weapon_armed _right_hand_slot;
    [SerializeField] private  Weapon_holder.Weapon_armed _left_hand_slot;
    private Weapon_damage_collider_enemy _left_hand_weapon_collider;
    private Weapon_damage_collider_enemy _right_hand_weapon_collider;
    private void Awake() {
        Weapon_holder.Weapon_armed[] Weapons_slots = GetComponentsInChildren<Weapon_holder.Weapon_armed>();
        foreach (Weapon_holder.Weapon_armed slot in Weapons_slots){
            if(slot.left_hand){
                _left_hand_slot = slot;
            }
            else if(slot.right_hand){
                _right_hand_slot = slot;
            }
        }
    }
    private void Start() {
        Load_weapons_to_hands();
    }
    //this is really bad idea but it works - need to look into it, and change few things als oin player etc same issue about order of executing scripts 
    //- i try to get into component in the deactivated object because script that activates it is called in this script so first
    // this script is executing till the end and olny then other scripts called from this one  
    private void Update(){
        if(left_hand_weapon != null && _left_hand_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Left_Pivot.GetComponent<Hand_slot_activator>().activated){
            Assign_weapon_collider(false);
            _left_hand_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Left_Pivot.GetComponent<Hand_slot_activator>().activated = false;
        }
        if(right_hand_weapon != null &&_right_hand_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Right_Pivot.GetComponent<Hand_slot_activator>().activated){
            Assign_weapon_collider(true);
            _right_hand_slot.current_Weapon_model_instantiated.GetComponent<Activate_pivots.Activate_pivots>().Right_Pivot.GetComponent<Hand_slot_activator>().activated = false;
        } 
    }
    private void Load_weapon_to_slot(Weapon_info.Weapon weapon, bool isRight){
        if(isRight){
            _right_hand_slot.Equip_weapon(weapon);
            Assign_weapon_collider(isRight);
        }
        else{
            _left_hand_slot.Equip_weapon(weapon);
            Assign_weapon_collider(isRight);
        }
    }
    private void Load_weapons_to_hands(){
        if(right_hand_weapon != null){
            Load_weapon_to_slot(right_hand_weapon,true);
        }
        if(left_hand_weapon != null){
            Load_weapon_to_slot(left_hand_weapon,false);
        }
    }
    private void Assign_weapon_collider(bool isRight){
        if(isRight){
            //Debug.Log(_right_hand_slot.current_Weapon_model_instantiated.GetComponentInChildren<Weapon_damage_collider_enemy>());
            _right_hand_weapon_collider = _right_hand_slot.current_Weapon_model_instantiated.GetComponentInChildren<Weapon_damage_collider_enemy>();
        }
        else{
            _left_hand_weapon_collider = _left_hand_slot.current_Weapon_model_instantiated.GetComponentInChildren<Weapon_damage_collider_enemy>();
        }
    }
    public void Enable_collider_on_right_weapon(){
        _right_hand_weapon_collider.Enable_collider();
    }
    public void Disable_collider_on_right_weapon(){
        _right_hand_weapon_collider.Disable_collider();
    }
    public void Enable_collider_on_left_weapon(){
        _left_hand_weapon_collider.Enable_collider();
    }
    public void Disable_collider_on_left_weapon(){
        _left_hand_weapon_collider.Disable_collider();
    }
}
