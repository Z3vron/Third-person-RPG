using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_animations_events_handler : MonoBehaviour
{
   private Animator _animator;
   private Weapon_slot_manager.Weapon_manager weapon_manager;
    void Start(){
        weapon_manager = GetComponentInParent<Weapon_slot_manager.Weapon_manager>();
        _animator = GetComponent<Animator>();
    }

    public void Left_weapon_collider_enable(){
        weapon_manager.Enable_collider_on_left_weapon();
        //Debug.Log("left weapon");
    }
    public void Right_weapon_collider_enable(){
        weapon_manager.Enable_collider_on_right_weapon();
    }
    public void Left_weapon_collider_disable(){
        weapon_manager.Disable_collider_on_left_weapon();
    }
    public void Right_weapon_collider_disable(){
        weapon_manager.Disable_collider_on_right_weapon();
    }
    public void Enable_invulnerability(){
        _animator.SetBool("Invulnerability",true);
    }
    public void Disable_invulnerability(){
        _animator.SetBool("Invulnerability",false);
    }
}
