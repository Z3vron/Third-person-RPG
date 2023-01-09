using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_info : MonoBehaviour{

     #region Player flags
        [field: SerializeField, Header("Player flags")]
        public bool active_animation {get; private set;} = false;
        [field: SerializeField] public bool player_invulnerability {get; private set;} = false;
        [field: SerializeField] public bool player_parrying {get; private set;} = false;
        [field: SerializeField] public bool player_grounded {get; private set;} = false;    
        [field: SerializeField] public bool player_crouching {get; private set;} = false;
        [field: SerializeField] public bool locked_on_enemy {get; set;} = false;
    #endregion
    #region Other scripts and components references
        [field: SerializeField, Header("Other scripts and components references")]
        public Player_statistics player_stats {get; private set;}
       // [SerializeField] private UI_elements.UI_bars _player_UI_info;
        private Animator _animator;
        private AudioSource _audio_source;
        public AudioClip eat_food;
        public AudioClip drink_potion;
    #endregion
    
    [SerializeField] private float _health_regen_delay = 5;
    [SerializeField] private float _stamina_regen_delay = 3;
    private float health_regen_timer;
    private float stamina_regen_timer;
    

    private bool _healing_process;
    private float _healing_duration_in_sec;
    private float _healing_amount_per_sec;
    private float _healing_timer = 0.0f;
   

    public static event Action Player_death;// same as below - Action is delegate that returns void, event ensure that given delegate/action could be invoke from this script only
    //public delegate void Player_death
    // public static event Player_death player_death
    private void Start() {
        Player_death += Stop_healing_and_stamina_regen;
        player_stats.Set_defaults_stats(300,100,4f,8f,20,1,0,100);
        player_stats.level = 1;
        player_stats.exp_to_next_level = 100;
        player_stats.current_exp = 0;
        _animator = GetComponentInChildren<Animator>();
        _audio_source = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){
        //Update_UI();
        player_grounded = GetComponent<Player_Movemnet.Movement>().player_grounded;
        player_crouching = GetComponent<Player_Movemnet.Movement>().player_crouching;
        active_animation = _animator.GetBool("Active_animation");
        player_invulnerability = _animator.GetBool("Invulnerability");
        player_parrying = _animator.GetBool("Parry");
        if(player_stats.Taken_dmg){
            if(player_stats.Current_health <= 0){
                Player_death?.Invoke();// if Player_death is diffrent from null then Invoke else dont do anything
            }
            health_regen_timer = 0;
            player_stats.Taken_dmg = false;
        }
        health_regen_timer += Time.deltaTime;
        if(health_regen_timer >= _health_regen_delay){
            player_stats.Health_regen(Time.deltaTime);           
        }
        if(player_stats.Taken_stamina){
            stamina_regen_timer = 0;
            player_stats.Taken_stamina = false;
        }
        stamina_regen_timer += Time.deltaTime;
        if(stamina_regen_timer >= _stamina_regen_delay){
            player_stats.Stamina_regen(Time.deltaTime);          
        }
        if(player_stats.current_exp >= player_stats.exp_to_next_level){
            player_stats.level += 1;
            player_stats.exp_to_next_level = player_stats.exp_to_next_level * player_stats.level;
        }
        Handle_healing_player();  
    }
    // void Update_UI(){
    //     _player_UI_info.Set_current_health(player_stats.Current_health/player_stats.Max_health);
    //     _player_UI_info.Set_current_stamina(player_stats.Current_stamina/player_stats.Max_stamina);
    //     _player_UI_info.Set_exp_level(player_stats.current_exp,player_stats.exp_to_next_level,player_stats.level);
    // }
    public void Play_audio_from_player(AudioClip audioclip,float delay){
        Function_timer.Create(() =>  _audio_source.PlayOneShot(audioclip),delay);
    }
    public void Start_healing_player_process(float duration, float amount_per_second){
        _healing_process = true;
        _healing_amount_per_sec = amount_per_second;
        _healing_duration_in_sec = duration;
    }
    private void Handle_healing_player(){
        if(_healing_process && _healing_timer < _healing_duration_in_sec){
            _healing_timer += Time.deltaTime;
            player_stats.Restore_health(_healing_amount_per_sec * Time.deltaTime);
        }
        if(_healing_timer >= _healing_duration_in_sec){
            _healing_process = false;
            _healing_timer = 0;
        }
    }    
    private void Stop_healing_and_stamina_regen(){
        this.enabled = false;
    }
}
