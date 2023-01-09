using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_damage_collider_enemy : MonoBehaviour{

    private Enemy_manager enemy_manager;
    private Enemy_weapon_slot_manager enemy_weapons;
    private BoxCollider _weapon_collider;
    [SerializeField] private bool _isRight;
    //decide do all weapons could be in both enemy and player hands if no that i can seperate this script into 2 for weapon for playuer and enemy - i Think this will  be beater idead
    private void Start(){
        _weapon_collider = GetComponent<BoxCollider>();
        _weapon_collider.enabled = false;
        enemy_manager = GetComponentInParent<Enemy_manager>();
        enemy_weapons = GetComponentInParent<Enemy_weapon_slot_manager>();
    }
    //write functions that will be handling common things/situations/code for the attack with left and right weapon
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            Disable_collider();
            if(other.GetComponentInParent<Player_info>().player_invulnerability){
                Debug.Log("Player is invulnerable at that moment");
                return;
            }
            else{
                if(_isRight){
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage(enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage  + enemy_weapons.right_hand_weapon.Light_attack_damage);
                }
                else
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage(enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage+enemy_weapons.left_hand_weapon.Light_attack_damage);
            }
        }
        if(other.CompareTag("Block_collider")){
            //Debug.Log("HITTED BLOCK");
            Disable_collider();
            if(other.GetComponentInParent<Player_info>().player_parrying){
                enemy_manager.animator.CrossFadeInFixedTime("Take_damage",0f,0);
            }
            else{//blocking
                enemy_manager.animator.CrossFadeInFixedTime("Idle",0f,0);
                if(_isRight){
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage((enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage  + enemy_weapons.right_hand_weapon.Light_attack_damage) * (100 - Game_manager.Instance.player_inventory.current_blocking_weapon.blocking_value) / 100);
                    other.GetComponentInParent<Player_info>().player_stats.Take_stamina(enemy_manager.instance_enemy_stats.Raw_strength/10);
                }
                else{
                    other.GetComponentInParent<Player_info>().player_stats.Take_damage((enemy_manager.instance_enemy_stats.Raw_strength + enemy_manager.current_attack.damage  + enemy_weapons.left_hand_weapon.Light_attack_damage) * (100 - Game_manager.Instance.player_inventory.current_blocking_weapon.blocking_value) / 100);
                    other.GetComponentInParent<Player_info>().player_stats.Take_stamina(enemy_manager.instance_enemy_stats.Raw_strength/10);
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
