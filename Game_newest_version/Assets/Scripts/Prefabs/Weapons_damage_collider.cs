using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons_damage_collider : MonoBehaviour{

    private Attack.Player_attack _attack;
    private Enemy_manager enemy_manager;
    private Enemy_weapon_slot_manager enemy_weapons;
    private BoxCollider _weapon_collider;
    private Player_inventory_info.Player_inventory _player_inventory;
    [SerializeField] private bool _isRight;
    private void Start(){
        _weapon_collider = GetComponent<BoxCollider>();
        _weapon_collider.enabled = false;
        _attack = GetComponentInParent<Attack.Player_attack>();
        enemy_manager = GetComponentInParent<Enemy_manager>();
        enemy_weapons = GetComponentInParent<Enemy_weapon_slot_manager>();
        _player_inventory = GetComponentInParent<Player_inventory_info.Player_inventory>();
    }
    //write functions that will be handling common things/situations/code for the attack with left and right weapon
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            if(_isRight){
                if(_attack.animator.GetBool("Strong_attack"))
                    other.GetComponent<Enemy_manager>().animator.CrossFadeInFixedTime("Take_damage",0f,0);
                other.GetComponent<Enemy_manager>().instance_enemy_stats.Take_damage(_attack.damage_given_right);
                _player_inventory.current_weapon_for_right_hand.Remove_durability(_attack.attack_durability_cost);
                if(_player_inventory.current_weapon_for_right_hand.isPoisoned){
                    other.GetComponent<Enemy_manager>().Poisoning(_player_inventory.current_weapon_for_right_hand.poison_duration,_player_inventory.current_weapon_for_right_hand.poison_damage);
                }
            }   
            else{
                if(_attack.animator.GetBool("Strong_attack"))
                    other.GetComponent<Enemy_manager>().animator.CrossFadeInFixedTime("Take_damage",0f,0);
                other.GetComponent<Enemy_manager>().instance_enemy_stats.Take_damage(_attack.damage_given_left);
                _player_inventory.current_weapon_for_left_hand.Remove_durability(_attack.attack_durability_cost);
                if(_player_inventory.current_weapon_for_left_hand.isPoisoned){
                    other.GetComponent<Enemy_manager>().Poisoning(_player_inventory.current_weapon_for_left_hand.poison_duration,_player_inventory.current_weapon_for_left_hand.poison_damage);
                }
            }
            //Debug.Log("Hitted enemy");
        }
        if(other.CompareTag("Player")){
            if(other.GetComponentInParent<Player_info>().player_invulnerability){
                Debug.Log("Player is player is invulnerable at that moment");
                return;
            }
            else{
                if(_isRight){
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage(enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage  + enemy_weapons.right_hand_weapon.Light_attack_damage);
                }
                else
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage(enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage+enemy_weapons.left_hand_weapon.Light_attack_damage);
                    //Debug.Log("Dont hit yourself");
            }
        }
    }
    public void Enable_collider(){
        _weapon_collider.enabled = true;
    }
    public void Disable_collider(){
        _weapon_collider.enabled  = false;
    }
}
