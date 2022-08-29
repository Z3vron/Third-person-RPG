using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_animations_events_handler : MonoBehaviour{
   private Animator _animator;
   private Enemy_weapon_slot_manager enemy_weapon_slot_manager;
    void Start(){
        enemy_weapon_slot_manager = GetComponentInParent<Enemy_weapon_slot_manager>();
        _animator = GetComponent<Animator>();
    }

    public void Left_weapon_collider_enable(){
        enemy_weapon_slot_manager.Enable_collider_on_left_weapon();
        //Debug.Log("left weapon");
    }
    public void Right_weapon_collider_enable(){
        enemy_weapon_slot_manager.Enable_collider_on_right_weapon();
    }
    public void Left_weapon_collider_disable(){
        enemy_weapon_slot_manager.Disable_collider_on_left_weapon();
    }
    public void Right_weapon_collider_disable(){
        enemy_weapon_slot_manager.Disable_collider_on_right_weapon();
    }
    public void End_of_light_right_attack_animation(){
        _animator.SetBool("Light_attack_right",false);
    }
    public void End_of_light_left_attack_animation(){
        _animator.SetBool("Light_attack_left",false);
    }
}
