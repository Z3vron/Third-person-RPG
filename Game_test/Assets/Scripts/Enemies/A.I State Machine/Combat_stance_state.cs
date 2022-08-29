using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_stance_state : State{
    public Attack_state attack_state;
    public Chase_state chase_state;
    public Idle_state idle_state;
    [SerializeField] private bool _target_in_field_of_view = false;
    [SerializeField] private float _border_angle_to_start_rotate = 75;
    public override State Run_current_state(Enemy_manager enemy_manager){
        if(enemy_manager.targeted_character == null)
            return idle_state;
        // if(enemy_manager.player.GetComponent<Player_info>().player_dead)
        //     return idle_state;
        //could chagne look at to code so that enemy would circle around player

        Vector3 target_dir = enemy_manager.targeted_character.transform.position - enemy_manager.transform.position;
        if(!enemy_manager.performing_action && !_target_in_field_of_view){
            target_dir.Normalize();
            target_dir.y = 0;
            Quaternion target_rotation = Quaternion.LookRotation(target_dir);
            enemy_manager.transform.rotation = Quaternion.Lerp(enemy_manager.transform.rotation,target_rotation,enemy_manager.enemy_rotation_speed * Time.deltaTime);
            //enemy_manager.transform.LookAt(enemy_manager.targeted_character.transform);//could use Quaternion.lerp / Quaternion.Slerp - allow to control  enemy rotation speed
        }
        float angle = Vector3.Angle(enemy_manager.transform.forward,target_dir);
        //Debug.Log(angle);
        // if(angle < _border_angle_to_start_rotate && angle > -_border_angle_to_start_rotate){
        //     _target_in_field_of_view = true;
        // }
        if(angle < 10 && angle > -10){
            _target_in_field_of_view = true;
        }
        else{
            _target_in_field_of_view = false;
        }
        if(Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) >= enemy_manager.distance_to_attack && !enemy_manager.performing_action){
            return chase_state;
        }
        if(enemy_manager.performing_action)
            return this;
        else if(Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) < enemy_manager.distance_to_attack && !enemy_manager.performing_action && _target_in_field_of_view){
            enemy_manager.current_attack = null;
            return attack_state;
        }
        else 
            return this;    
    }
}
