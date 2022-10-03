using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_state : State{
    public Chase_state chase_state;
    public override State Run_current_state(Enemy_manager enemy_manager){
        //detect enemy to go to
        enemy_manager.animator.SetFloat("Vertical",0);
        Collider[] hit_colliders = Physics.OverlapSphere(enemy_manager.transform.position,enemy_manager.detection_radius,enemy_manager.characters_layer_mask);
        foreach( var character_collider in hit_colliders){
            Vector3 target_direction = character_collider.transform.position - enemy_manager.transform.position;
            float viewable_angle = Vector3.Angle(target_direction,enemy_manager.transform.forward);
            if(viewable_angle > enemy_manager.minimum_detection_angle && viewable_angle < enemy_manager.maximum_detection_angle && enemy_manager.player != null && enemy_manager.player.GetComponent<Player_Movemnet.Movement>().Is_enemy_to_lock_on_visible(gameObject)){ // Is_enemy_to_lock_on_visible uses insidee tranform, the original script is attached to player so transform is player object not enemy object from which i call function
                if(character_collider.CompareTag("Enemy") && character_collider.gameObject != gameObject){
                    //Debug.Log("Found another enemy");
                    //enemy_manager.targeted_character = character_collider.gameObject;
                }
                else if(character_collider.CompareTag("Player")){
                    //Debug.Log("Found player");
                    enemy_manager.targeted_character = character_collider.gameObject;

                }
            }
        }
        //change state based on enemy
        if(enemy_manager.targeted_character != null){
            return chase_state;
        }
        else{
            return this;
        }
    }
}
