using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_state : State{
    public Combat_stance_state combat_stance_state;
    public override State Run_current_state(Enemy_manager enemy_manager){
        //Debug.Log("Start  attacking");
        if(enemy_manager.current_attack == null){
            enemy_manager.Get_new_attack();
            return this;
        }
        else{
            Vector3 target_direction = enemy_manager.targeted_character.transform.position - enemy_manager.transform.position;
            float viewable_angle = Vector3.Angle(target_direction,enemy_manager.transform.forward);
            //check for distance and angles to use specific attack if something doesn't match than get new attack
            if(Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) < enemy_manager.current_attack.minimum_distance_to_attack || Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) > enemy_manager.current_attack.maximum_distance_to_attack
            || viewable_angle < enemy_manager.current_attack.minimum_attack_angle || viewable_angle > enemy_manager.current_attack.maximum_attack_angle ){
                enemy_manager.current_attack = null;
                return this;
            }
            enemy_manager.performing_action = true;
            //recovery_timer includes time for the attack animation so each attack recovery timer should be bigger or just change where i put this - handled in enemy manager by counting time if in idle state
            enemy_manager.current_recovery_time = enemy_manager.current_attack.recovery_time;
            enemy_manager.animator.CrossFadeInFixedTime(enemy_manager.current_attack.animation_name,0f,0);
            //enemy_manager.current_attack = null;
            return combat_stance_state;
            
            
        }
    }
}
