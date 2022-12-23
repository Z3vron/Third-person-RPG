using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_damage_collider_player : MonoBehaviour
{
    private Player_attack_handler _attack;
    private BoxCollider _weapon_collider;
    private Player_inventory_info.Player_inventory _player_inventory;
    [SerializeField] private bool _isRight;
    //decide do all weapons could be in both enemy and player hands if no that i can seperate this script into 2 for weapon for playuer and enemy - i Think this will  be beater idead
    private void Start(){
        _weapon_collider = GetComponent<BoxCollider>();
        _weapon_collider.enabled = false;
        _attack = GetComponentInParent<Player_attack_handler>();
        _player_inventory = GetComponentInParent<Player_inventory_info.Player_inventory>();
    }
    //write functions that will be handling common things/situations/code for the attack with left and right weapon
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            Disable_collider();
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
        }
    }
    public void Enable_collider(){
        _weapon_collider.enabled = true;
    }
    public void Disable_collider(){
        _weapon_collider.enabled  = false;
    }
}
