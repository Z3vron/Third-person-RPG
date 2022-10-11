using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Attack{
    public class Player_attack : MonoBehaviour{
        
        public float damage_given_left = 0;
        public float damage_given_right = 0;
        public float attack_durability_cost = 0;
        
        [SerializeField] private Rigidbody _projectile_prefab_model;
        [SerializeField] private Transform _root_for_shooting_projectile;
        [SerializeField] private float _projectile_speed = 2000f;
        private float _player_strength;
        private float _player_stamina;
        private bool _Is_in_air;
        private bool _Is_crouching;
        private int _last_light_attack = 0;
        private Player_statistics _player_stats;
        private Player_inventory_info.Player_inventory _player_inventory;
        public Animator animator;
        // !! Before Instantiate cant get to the data but after it i can just use get component as example below, cant assing objects to the overall prefab but to the instantiate in ine scene yes.
        // change flags that scripts get from prefabs to script on pfebbas to get values by themselves - think about it - probably change quite few things - especially arming player  with weapons - pivots left and right lots of 
        // unnecessary code and values etc
        private void Start() {
            animator = gameObject.GetComponentInChildren<Animator>();
            _player_stats = GetComponent<Player_info>().player_stats;
            _player_inventory = GetComponent<Player_inventory_info.Player_inventory>();
        }
        private void Update() {
            _Is_crouching = GetComponent<Player_info>().player_crouching;
            _Is_in_air = !GetComponent<Player_info>().player_grounded;
            _player_stamina =  _player_stats.Current_stamina;
            _player_strength =  _player_stats.Raw_strength;
        }
        public void Handle_combo_attack(){
            attack_durability_cost = 1;
            if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed  && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){
                if(! _Is_in_air && !_Is_crouching){

                    if( _player_stamina >= (_player_inventory.current_weapon_for_left_hand.combo_attack_stamina_cost + _player_inventory.current_weapon_for_right_hand.combo_attack_stamina_cost)){

                        damage_given_left = _player_inventory.current_weapon_for_left_hand.Light_attack_damage +  _player_strength + _player_inventory.current_weapon_for_left_hand.combo_dmg_bonus;
                        damage_given_right = _player_inventory.current_weapon_for_right_hand.Light_attack_damage + _player_strength + _player_inventory.current_weapon_for_right_hand.combo_dmg_bonus;
                        _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.combo_attack_stamina_cost + _player_inventory.current_weapon_for_right_hand.combo_attack_stamina_cost);


                        //Debug.Log("Combo attack was performed");
                        //Debug.Log(_player_inventory.current_weapon_for_left_hand +" "+ _player_inventory.current_weapon_for_right_hand +" "+ _player_inventory.unarmed);
                        if(_player_inventory.current_weapon_for_left_hand ==_player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
                            Debug.Log("fist combo attack animation here ");
                        }
                        else{
                            animator.CrossFadeInFixedTime("Override.Light_attack_combo",0f,1);
                        }       
                        // Debug.Log("Damage given to enemy without armour: " + damage_given_left + damage_given_right);
                    }
                    else{
                        Debug.Log("Not enough stamina to perform combo attack - maybe do something with stamina bar light it on or somethink don't know");
                    }              
                }
            }
            else{
                Debug.Log("Can't do combo with no weapon in one hand for now - i need more animations ");
            }
        }
        public void Handle_light_attack(){
            attack_durability_cost = 1;
            if(_player_stamina < _player_inventory.current_weapon_for_left_hand.light_attack_stamina_cost || _player_stamina < _player_inventory.current_weapon_for_right_hand.light_attack_stamina_cost){
                Debug.Log("Not enough stamina to perform light attack");
            }
            
            else if(! _Is_in_air && !_Is_crouching){        
                //Debug.Log("Light ground attack should perform");
                //no weapons in either hand
                damage_given_left = _player_inventory.current_weapon_for_left_hand.Light_attack_damage +  _player_strength;
                damage_given_right = _player_inventory.current_weapon_for_right_hand.Light_attack_damage + _player_strength;
                  
                
                if(_player_inventory.current_weapon_for_left_hand ==  _player_inventory.unarmed &&  _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
                    Debug.Log("fist light attack animation here ");
                }
                //weapon in only right hand
                else if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed){
                    Start_right_light_attack();
                    
                }
                //weapon in only left hand
                else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
                    Start_left_light_attack();
                    
                }
                // weapons in both hands
                else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){

                    //Should be randomised store all atacks in data - array or list and from them start them etc - better for more animations - to
                    if(_last_light_attack == 0){
                        Start_left_light_attack();
                        _last_light_attack =1;
                    }  
                    else if (_last_light_attack == 1){
                        Start_right_light_attack();
                        _last_light_attack = 0;
                    }
                }
                //_animator.SetFloat("Attack_speed_multiplayer",1);   
                  
            }
            else if (_Is_in_air){
                Debug.Log("Light air attack was performed");
            }
        }
        public void Handle_heavy_attack(){
            attack_durability_cost = 2;
            if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed){
                Start_right_heavy_attack();
            }
                
            //Debug.Log("Strong ground attack was performed");
        }
        public void Handle_special_attack(){
            //    Debug.Log("Special attack was performed");
            Rigidbody Instance_projectile;
            Instance_projectile = Instantiate(_projectile_prefab_model,_root_for_shooting_projectile.position,_root_for_shooting_projectile.rotation) as Rigidbody;
            Instance_projectile.AddForce(_root_for_shooting_projectile.forward * _projectile_speed);        
        }
        //could change two functions below to use third one so that there would be less code overall
        public void Start_left_light_attack(){
            //_player_inventory.current_weapon_for_left_hand.GetType()
            if(_player_inventory.current_weapon_for_left_hand is Dagger){
                Dagger dagger = (Dagger)_player_inventory.current_weapon_for_left_hand;
                animator.SetFloat("Attack_speed_multiplayer",dagger.attack_speed_multiplayer);
            }
            else if(_player_inventory.current_weapon_for_left_hand is Rapier){
                Rapier rapier = (Rapier)_player_inventory.current_weapon_for_left_hand;
                animator.SetFloat("Attack_speed_multiplayer",rapier.attack_speed_multiplayer);
            }
            else{
                animator.SetFloat("Attack_speed_multiplayer",1);
            }
            animator.CrossFadeInFixedTime("Override.Light_attack_left",0f,1);
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.light_attack_stamina_cost);
        }
        public void Start_right_light_attack(){
            if(_player_inventory.current_weapon_for_right_hand is Dagger){
                Dagger dagger = (Dagger)_player_inventory.current_weapon_for_right_hand;
                animator.SetFloat("Attack_speed_multiplayer",dagger.attack_speed_multiplayer);
            }
            else if(_player_inventory.current_weapon_for_right_hand is Rapier){
                Rapier rapier = (Rapier)_player_inventory.current_weapon_for_right_hand;
                animator.SetFloat("Attack_speed_multiplayer",rapier.attack_speed_multiplayer);
            }
            else{
                animator.SetFloat("Attack_speed_multiplayer",1);
            }
            animator.CrossFadeInFixedTime("Override.Light_attack_right",0f,1);
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_right_hand.light_attack_stamina_cost);
        }
         public void Start_right_heavy_attack(){
            if(_player_inventory.current_weapon_for_right_hand is Dagger){
                Dagger dagger = (Dagger)_player_inventory.current_weapon_for_right_hand;
                animator.SetFloat("Attack_speed_multiplayer",dagger.attack_speed_multiplayer);
            }
            else if(_player_inventory.current_weapon_for_right_hand is Rapier){
                Rapier rapier = (Rapier)_player_inventory.current_weapon_for_right_hand;
                animator.SetFloat("Attack_speed_multiplayer",rapier.attack_speed_multiplayer);
            }
            else{
                animator.SetFloat("Attack_speed_multiplayer",1);
            }
            animator.CrossFadeInFixedTime("Override.Strong_attack_right",0f,1);
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_right_hand.strong_attack_stamina_cost);
        }
    }

}
