using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_attack_handler : MonoBehaviour{
    public Animator animator;
    #region Info for weapon_damage_collider
        [Header("Info for weapon_damage_collider")]
        public float damage_given_left = 0;
        public float damage_given_right = 0;
        public float attack_durability_cost = 0;
    #endregion
    #region Variables about shooting projectile
        [Header("Variables about shooting projectile")]
        [SerializeField] private Rigidbody _projectile_prefab_model;
        [SerializeField] private Transform _root_for_shooting_projectile;
        [SerializeField] private float _projectile_speed = 2000f;
    #endregion
    #region Player stats/info
        private float _player_strength;
        private float _player_stamina;
        private bool _is_in_air;
        private bool _is_crouching;
        private Player_statistics _player_stats;
    #endregion
    [SerializeField] private GameObject _block_collider;
    private bool _last_light_attack_right = false;
    private bool _last_heavy_attack_right = false;
    private Player_inventory_info.Player_inventory _player_inventory;
    #region  Lists of possible player attacks(used for playing given animation)
        [Header("Lists of possible player attacks")]
        [SerializeField] private List<Player_attacks> _player_list_light_attacks;
        [SerializeField] private List<Player_attacks> _player_list_heavy_attacks;
        [SerializeField] private List<Player_attacks> _player_list_combo_attacks;
        [SerializeField] private List<Player_attacks> _player_list_air_special_attacks;
    #endregion

    // !! Before Instantiate cant get to the data but after it i can just use get component as example below, cant assing objects to the overall prefab but to the instantiate in ine scene yes.
    // change flags that scripts get from prefabs to script on pfebbas to get values by themselves - think about it - probably change quite few things - especially arming player  with weapons - pivots left and right lots of 
    // unnecessary code and values etc
    // MOve attaqck animations to weapons so that each weapon could use diffrent attack animations
    private void Start() {
        animator = GetComponentInChildren<Animator>();
        _player_stats = GetComponent<Player_info>().player_stats;
        _player_inventory = GetComponent<Player_inventory_info.Player_inventory>();
    }
    private void Update() {
        _is_crouching = GetComponent<Player_info>().player_crouching;
        _is_in_air = !GetComponent<Player_info>().player_grounded;
        _player_stamina =  _player_stats.Current_stamina;
        _player_strength =  _player_stats.Raw_strength;
        if(animator.GetBool("Blocking") && _player_stamina < _player_inventory.current_weapon_for_left_hand.blocking_stamina_cost * Time.deltaTime)
            animator.SetBool("Blocking",false);  
        if(animator.GetBool("Blocking"))
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.blocking_stamina_cost * Time.deltaTime);
    }
    public void Handle_combo_attack(){
        attack_durability_cost = 1;
        if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed  && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){
            if(! _is_in_air && !_is_crouching){

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
    public void Handle_light_attack(bool is_player_running){
        attack_durability_cost = 1;
        if(! _is_in_air && !_is_crouching){      
            //no weapons in either hand
            if(_player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed)
                Debug.Log("Fist light attack here");
            //weapon in only right hand
            if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed || _player_inventory.current_weapon_for_left_hand is Shield ){
                Start_right_light_attack(is_player_running);
            }
            //weapon in only left hand
            else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
                Start_left_light_attack(is_player_running);
            }
            // weapons in both hands
            else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){
                if(_last_light_attack_right){
                    Start_left_light_attack(is_player_running);
                    _last_light_attack_right= false;
                }  
                else if (!_last_light_attack_right){
                    Start_right_light_attack(is_player_running);
                    _last_light_attack_right = true;
                }
            }
        }
        else if (_is_in_air){
            //animator.CrossFadeInFixedTime("Override.Air_attack_01",0f,1);
            Debug.Log("Light air attack was performed");
        }
    }
    public void Handle_heavy_attack(){
        if(! _is_in_air && !_is_crouching){      
            attack_durability_cost = 2;
            if(_player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed)
                    Debug.Log("Fist heavy attack here");
            if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed){
                Start_right_heavy_attack();
            }
            else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
                Start_left_heavy_attack();
            }
            else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){
                if(_last_heavy_attack_right){
                    Start_left_heavy_attack();
                    _last_heavy_attack_right = false;
                }  
                else if (!_last_heavy_attack_right){
                    Start_right_heavy_attack();
                    _last_heavy_attack_right = true;
                }
            }
        }
    }
    public void Handle_special_attack(){
        //    Debug.Log("Special attack was performed");
        Rigidbody Instance_projectile;
        Instance_projectile = Instantiate(_projectile_prefab_model,_root_for_shooting_projectile.position,_root_for_shooting_projectile.rotation) as Rigidbody;
        Instance_projectile.AddForce(_root_for_shooting_projectile.forward * _projectile_speed);        
    }
    //could change two functions below to use third one so that there would be less code overall
    public void Handle_air_special_attack(){
        if(_is_in_air || GetComponent<Player_info>().player_crouching || _player_inventory.current_weapon_for_left_hand == _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed )
                return ;
        
        if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand == _player_inventory.unarmed){
            if(_player_stats.Current_stamina < _player_inventory.current_weapon_for_left_hand.air_attack_stamina_cost)
                return ;
            damage_given_left = _player_inventory.current_weapon_for_left_hand.air_attack_damage + _player_strength;
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.air_attack_stamina_cost);
        } 
        else if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_left_hand == _player_inventory.unarmed){
            if(_player_stats.Current_stamina < _player_inventory.current_weapon_for_right_hand.air_attack_stamina_cost)
                return ;
            damage_given_right = _player_inventory.current_weapon_for_right_hand.air_attack_damage + _player_strength;
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_right_hand.air_attack_stamina_cost);
        }
        else if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && _player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed){
            if(_player_stats.Current_stamina < _player_inventory.current_weapon_for_left_hand.air_attack_stamina_cost + _player_inventory.current_weapon_for_right_hand.air_attack_stamina_cost)
                return ;
            damage_given_left = _player_inventory.current_weapon_for_left_hand.air_attack_damage + _player_strength;
            damage_given_right = _player_inventory.current_weapon_for_right_hand.air_attack_damage + _player_strength;
            _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.air_attack_stamina_cost + _player_inventory.current_weapon_for_right_hand.air_attack_stamina_cost);
        }
        animator.CrossFadeInFixedTime("Override.Air_attack_01",0f,1);
    }
    
    public void Start_left_light_attack(bool is_player_running){
        if(_player_stamina < _player_inventory.current_weapon_for_left_hand.light_attack_stamina_cost)
            return;

        damage_given_left = _player_inventory.current_weapon_for_left_hand.Light_attack_damage +  _player_strength;
        if(is_player_running)
            damage_given_left *= 1.5f;
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
        if(is_player_running){
            animator.CrossFadeInFixedTime("Running_attack_left",0f,1);
        }
        else{
            int attack_num;
            while(true){
                attack_num = Random.Range(0,_player_list_light_attacks.Count);
                if(_player_list_light_attacks[attack_num].require_left_weapon)
                    break;
            }
            animator.CrossFadeInFixedTime(_player_list_light_attacks[attack_num].attack_name,0f,1);
        }
        _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.light_attack_stamina_cost);
    }
    public void Start_right_light_attack(bool is_player_running){
        if(_player_stamina < _player_inventory.current_weapon_for_right_hand.light_attack_stamina_cost)
            return;
        damage_given_right = _player_inventory.current_weapon_for_right_hand.Light_attack_damage + _player_strength;
        if(is_player_running)
            damage_given_right *= 1.5f;
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
        if(is_player_running){
            animator.CrossFadeInFixedTime("Running_attack_right",0f,1);
        }
        else{
            int attack_num;
            while(true){
                attack_num = Random.Range(0,_player_list_light_attacks.Count);
                if(_player_list_light_attacks[attack_num].require_right_weapon)
                    break;
            }
        animator.CrossFadeInFixedTime(_player_list_light_attacks[attack_num].attack_name,0f,1);
        }
       
        _player_stats.Take_stamina(_player_inventory.current_weapon_for_right_hand.light_attack_stamina_cost);
    }
    public void Start_right_heavy_attack(){
        if(_player_stamina < _player_inventory.current_weapon_for_right_hand.strong_attack_stamina_cost)
            return;
        damage_given_right = _player_inventory.current_weapon_for_right_hand.Strong_attack_damage + _player_strength;
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
        int attack_num;
        while(true){
            attack_num = Random.Range(0,_player_list_heavy_attacks.Count);
            if(_player_list_heavy_attacks[attack_num].require_right_weapon)
                break;
        }
        animator.CrossFadeInFixedTime(_player_list_heavy_attacks[attack_num].attack_name,0f,1);
        _player_stats.Take_stamina(_player_inventory.current_weapon_for_right_hand.strong_attack_stamina_cost);
    }
    public void Start_left_heavy_attack(){
        if(_player_stamina < _player_inventory.current_weapon_for_left_hand.strong_attack_stamina_cost)
            return;
        damage_given_left = _player_inventory.current_weapon_for_left_hand.Strong_attack_damage + _player_strength;
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
        int attack_num;
        while(true){
            attack_num = Random.Range(0,_player_list_heavy_attacks.Count);
            if(_player_list_heavy_attacks[attack_num].require_left_weapon)
                break;
        }
        animator.CrossFadeInFixedTime(_player_list_heavy_attacks[attack_num].attack_name,0f,1);
        _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.strong_attack_stamina_cost);
    }
    public void Start_blocking_attacks(){
        if(_player_inventory.current_weapon_for_left_hand != _player_inventory.unarmed && !animator.GetBool("Blocking") ){
            animator.SetBool("Blocking",true);
            animator.CrossFadeInFixedTime("Blocking_with_shield_begin",0f,2);
            _player_inventory.current_blocking_weapon = _player_inventory.current_weapon_for_left_hand;
        }
        else if(_player_inventory.current_weapon_for_right_hand != _player_inventory.unarmed && !animator.GetBool("Blocking") ){
            animator.SetBool("Blocking",true);
            animator.CrossFadeInFixedTime("Blocking_with_swords_begin",0f,2);
            _player_inventory.current_blocking_weapon = _player_inventory.current_weapon_for_right_hand;
        }
    }
    public void Stop_blocking_attacks(){
        if(animator.GetBool("Blocking")){
            animator.SetBool("Blocking",false);
        }
    }
    public void Parry_attack(){
        animator.CrossFadeInFixedTime("Shield_parry",0f,1);
        _player_stats.Take_stamina(_player_inventory.current_weapon_for_left_hand.parry_stamina_cost);
    }
    
}
