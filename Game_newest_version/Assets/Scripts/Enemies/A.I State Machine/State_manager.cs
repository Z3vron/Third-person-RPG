using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_manager : MonoBehaviour{
   [SerializeField] private  State _current_state;
   private Enemy_manager _enemy_manager; 

    private void Start() {
    _enemy_manager = GetComponentInParent<Enemy_manager>();
     Player_info.Player_death +=  Stop_enemy;
    }
    void Update(){
        Run_state_machine();
    }
    private void Run_state_machine(){
        State next_state = _current_state?.Run_current_state(_enemy_manager);
        if(next_state != null){
            Switch_to_next_state(next_state);
        }
    }
    private void Switch_to_next_state(State next_state){
        _current_state = next_state;
    }
    private void Stop_enemy(){
        _current_state = null;
    }
}
