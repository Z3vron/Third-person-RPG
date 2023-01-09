using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_state : State{
    public Combat_stance_state combat_stance_state;
    public Idle_state idle_state;
    public override State Run_current_state(Enemy_manager enemy_manager){
        if(enemy_manager.targeted_character == null)
            return idle_state;
        if(Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) > 25){
            enemy_manager.targeted_character = null;
            enemy_manager.nav_mesh_agent.isStopped = true;
            enemy_manager.animator.SetFloat("Vertical",0);//,0.1f,Time.fixedDeltaTime);//with damping animation is still going
            return  idle_state;
        }
        else if(Vector3.Distance(enemy_manager.transform.position,enemy_manager.targeted_character.transform.position) > enemy_manager.distance_to_attack && !enemy_manager.performing_action){
            //Debug.Log("Moving to target");
            enemy_manager.nav_mesh_agent.SetDestination(enemy_manager.targeted_character.transform.position);
            if(enemy_manager.nav_mesh_agent.isStopped){
                enemy_manager.nav_mesh_agent.isStopped = false;
            }
            enemy_manager.animator.SetFloat("Vertical",1,0.1f,Time.fixedDeltaTime);
        }
        if(enemy_manager.nav_mesh_agent.remainingDistance <= enemy_manager.distance_to_attack){
            //stop nav mesh agent
            enemy_manager.animator.SetFloat("Vertical",0);//0.1f,Time.fixedDeltaTime);
            enemy_manager.nav_mesh_agent.velocity = Vector3.zero;
            enemy_manager.nav_mesh_agent.isStopped = true;
            return combat_stance_state;
        }
        else{
            return this;
        }
    }
}
