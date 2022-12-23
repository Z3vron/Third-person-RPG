using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_handler : MonoBehaviour
{
    //all variables about input have flags at the end of name
    // can i use delegates to listen to change in input?? could be easier to track input in other scripts etc
    #region Player_action_map
        public bool sprint_flag;
        public bool crouch_flag;
        public bool jump_flag;
        public bool inventory_flag;
        public bool left_weapon_flag;
        public bool right_weapon_flag;
        public bool first_potion_flag;
        public bool second_potion_flag;
        public bool third_potion_flag;
        public bool fourth_potion_flag;
        public bool interact_flag;
        public bool block_attacks_flag;
        public bool parry_attack_flag;
        public bool attack_light_flag;
        public bool attack_strong_started_flag;
        public bool attack_strong_canceled_flag;
        public bool attack_strong_performed_flag;
        public bool attack_special_flag;
        public bool attack_combo_flag;
        public bool attack_air_flag;
        public bool lock_on_flag;
        public bool switch_flag;
        public bool dash_flag;
        public Vector2 walk_input;

        private InputAction _jump_action;
        private InputAction _walk_action;
        private InputAction _sprint_action;
        private InputAction _block_attack_action;
        private InputAction _parry_attack_action;
        private InputAction _attack_1_action;
        private InputAction _attack_2_action;
        private InputAction _attack_3_action;
        private InputAction _attack_air_action;
        private InputAction _interact_action;
        private InputAction _crouch_action;
        private InputAction _weapon_left_action;
        private InputAction _weapon_right_action;
        private InputAction _first_potion_action;
        private InputAction _second_potion_action;
        private InputAction _third_potion_action;
        private InputAction _fourth_potion_action;
        private InputAction _combo_attack_action;
        private InputAction _open_inventory_action;
        private InputAction _lock_on_action;
        private InputAction _switch_action_player;
        private InputAction _dash_action;
        private InputActionPhase attack_strong_last_phase;
        
        private InputAction _help_action;
    #endregion
    #region  Inventory_action_map
        public bool mouse_right_pressed_inv_flag;
        public bool mouse_left_pressed_inv_flag;
        public bool transfer_items_inv_flag;
        public bool drop_items_inv_flag;
        public bool use_item_inv_flag;
        public bool inventory_close_inv_flag;
        public bool confirmed_action_inv_flag;
        public bool left_weapon_inv_flag;
        public bool right_weapn_inv_flag;
         public bool first_potion_inv_flag;
        public bool second_potion_inv_flag;
        public bool third_potion_inv_flag;
        public bool fourth_potion_inv_flag;
        public Vector2 mouse_position;

        private InputAction _close_inventory_action;
        private InputAction _mouse_right_pressed_inv_action;
        private InputAction _mouse_left_pressed_inv_action;
        private InputAction _transfer_items_inv_action;
        private InputAction _drop_items_inv_action;
        private InputAction _use_item_inv_action;
        private InputAction _confirm_action_inv_action;
        private InputAction _left_weapon_inv_action;
        private InputAction _right_weapon_inv_action;
         private InputAction _first_potion_inv_action;
        private InputAction _second_potion_inv_action;
        private InputAction _third_potion_inv_action;
        private InputAction _fourth_potion_inv_action;
        private InputAction _switch_action_inv;
        private InputAction _mouse_pos;
    #endregion
    private PlayerInput _player_input;
    void Start(){
        //Player action map
        _player_input = GetComponent<PlayerInput>();
        _jump_action = _player_input.actions["Jump"];
        _walk_action = _player_input.actions["Move"];
        _sprint_action = _player_input.actions["Sprint"];
        _block_attack_action  = _player_input.actions["Block"];
        _parry_attack_action = _player_input.actions["Parry"];
        _attack_1_action = _player_input.actions["Light_attack"];
        _attack_2_action = _player_input.actions["Strong_attack"];
        _attack_3_action = _player_input.actions["Special_attack"];
        _attack_air_action = _player_input.actions["Air_attack"];
        _interact_action = _player_input.actions["Interact"];
        _crouch_action  = _player_input.actions["Crouch"];
        _weapon_left_action = _player_input.actions["Weapon_left"];
        _weapon_right_action = _player_input.actions["Weapon_right"];
        _first_potion_action = _player_input.actions["First_potion"];
        _second_potion_action = _player_input.actions["Second_potion"];
        _third_potion_action = _player_input.actions["Third_potion"];
        _fourth_potion_action = _player_input.actions["Fourth_potion"];
        _combo_attack_action = _player_input.actions["Combo_attack"];
        _open_inventory_action = _player_input.actions["Inventory"];
        _help_action = _player_input.actions["Help"];
        _lock_on_action = _player_input.actions["Lock_on"];
        _switch_action_player = _player_input.actions["Switch_player"];
        _dash_action = _player_input.actions["Dash"];

        //Inventory action map
        _close_inventory_action = _player_input.actions["Exit"];
        _mouse_right_pressed_inv_action = _player_input.actions["Right_click"];
        _mouse_left_pressed_inv_action = _player_input.actions["Left_click"];
        _transfer_items_inv_action = _player_input.actions["Transfer_items"];
        _drop_items_inv_action = _player_input.actions["Drop_items"];
        _use_item_inv_action = _player_input.actions["Use"];
        _confirm_action_inv_action = _player_input.actions["Confirm"];
        _left_weapon_inv_action = _player_input.actions["Left weapon"];
        _right_weapon_inv_action = _player_input.actions["Right weapon"];
        _first_potion_inv_action = _player_input.actions["First_potion_inv"];
        _second_potion_inv_action = _player_input.actions["Second_potion_inv"];
        _third_potion_inv_action = _player_input.actions["Third_potion_inv"];
        _fourth_potion_inv_action = _player_input.actions["Fourth_potion_inv"];
        _switch_action_inv = _player_input.actions["Switch_inv"];
        _mouse_pos = _player_input.actions["Point"];
        
    }
    public void Check_flags(){
        //Player action map
        if(_sprint_action.ReadValue<float>() ==1)    sprint_flag = true;{}
        if(_crouch_action.triggered)                 crouch_flag = true;{}
        if(_jump_action.triggered)                   jump_flag = true;{}
        if(_open_inventory_action.triggered)         inventory_flag = true;{}
        if(_weapon_left_action.triggered)            left_weapon_flag = true;{}
        if(_weapon_right_action.triggered)           right_weapon_flag = true;{}
        if(_first_potion_action.triggered)           first_potion_flag = true;{}
        if(_second_potion_action.triggered)           second_potion_flag = true;{}
        if(_third_potion_action.triggered)           third_potion_flag = true;{}
        if(_fourth_potion_action.triggered)           fourth_potion_flag = true;{}
        if(_interact_action.triggered)               interact_flag = true;{}
        if(_lock_on_action.triggered)                lock_on_flag = true;{}
        if(_switch_action_player.triggered)                 switch_flag = true;{}
        if(_dash_action.triggered)                   dash_flag = true;{}
        if(_block_attack_action.ReadValue<float>() == 1) block_attacks_flag = true;{}
        if(_parry_attack_action.triggered)           parry_attack_flag = true;{}
        if(_attack_1_action.triggered)               attack_light_flag = true;{}
        if(_attack_2_action.phase == InputActionPhase.Started){
            attack_strong_started_flag = true;
            attack_strong_last_phase = _attack_2_action.phase;
           //Debug.Log("started strong attack");
        }
        //InputActionPhase.Canceled doesn't go off after realising button before required time elapsed - so this is work around - it works but there must be a better way of doing this
        if(attack_strong_last_phase == InputActionPhase.Started && _attack_2_action.phase == InputActionPhase.Waiting){
            attack_strong_started_flag = false;
            //Debug.Log("canceled strong attack");
            attack_strong_canceled_flag = true;
            attack_strong_last_phase = _attack_2_action.phase;
        }
        if(_attack_2_action.triggered){
            attack_strong_canceled_flag = false;
            attack_strong_performed_flag = true;
        }
        if(_attack_3_action.triggered)               attack_special_flag = true;{}
        if(_combo_attack_action.triggered)           attack_combo_flag = true;{}
        if(_attack_air_action.triggered)             attack_air_flag = true;{}
        walk_input = _walk_action.ReadValue<Vector2>();
       
        //Inventory action map
        if(_close_inventory_action.triggered){
            inventory_close_inv_flag = true;
            inventory_flag = true;
        }
        if(_mouse_right_pressed_inv_action.triggered)   mouse_right_pressed_inv_flag = true;{}
        if(_mouse_left_pressed_inv_action.triggered)    mouse_left_pressed_inv_flag = true;{}
        if(_transfer_items_inv_action.triggered)        transfer_items_inv_flag = true;{}
        if(_drop_items_inv_action.triggered)            drop_items_inv_flag = true;{}
        if(_use_item_inv_action.triggered)              use_item_inv_flag = true;{}
        if(_confirm_action_inv_action.triggered)        confirmed_action_inv_flag = true;{}
        if(_left_weapon_inv_action.triggered)           left_weapon_inv_flag = true;{}
        if(_right_weapon_inv_action.triggered)          right_weapn_inv_flag = true;{} 
        if(_first_potion_inv_action.triggered)          first_potion_inv_flag = true;{}          
        if(_second_potion_inv_action.triggered)         second_potion_inv_flag = true;{}
        if(_third_potion_inv_action.triggered)          third_potion_inv_flag = true;{}
        if(_fourth_potion_inv_action.triggered)         fourth_potion_inv_flag = true;{}
        if(_switch_action_inv.triggered)                switch_flag = true;{}
        mouse_position = _mouse_pos.ReadValue<Vector2>();
               
    }
    public void Switch_action_map_to_inv(){
        _player_input.SwitchCurrentActionMap("Inventory");
    }
    public void Switch_action_map_to_player(){
        _player_input.SwitchCurrentActionMap("Player");
    }
}