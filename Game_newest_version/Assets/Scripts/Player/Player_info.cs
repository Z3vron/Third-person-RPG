using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_info : MonoBehaviour{
    public bool active_animation = false;
    public bool player_invulnerability;
    public bool player_grounded;
    public bool player_crouching;
    public bool locked_on_enemy = false;
    public Player_statistics player_stats;
    [SerializeField] private UI_elements.UI_bars _player_UI_info;
    [SerializeField] private float _health_regen_delay = 5;
    [SerializeField] private float _stamina_regen_delay = 3;
    private float health_regen_timer;
    private float stamina_regen_timer;
    private Animator _animator;

    private bool _healing_process;
    private float _healing_duration_in_sec;
    private float _healing_amount_per_sec;
    private float _healing_timer = 0.0f;
    private AudioSource _audio_source;
    private void Start() {
        player_stats.Set_defaults_stats(300,100,0.1f,0.01f,20,1,0,100);
        player_stats.level = 1;
        player_stats.exp_to_next_level = 100;
        player_stats.current_exp = 0;
        _animator = GetComponentInChildren<Animator>();
        _audio_source = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){
        Update_UI();
        player_grounded = GetComponent<Player_Movemnet.Movement>().player_grounded;
        player_crouching = GetComponent<Player_Movemnet.Movement>().player_crouching;
        active_animation = _animator.GetBool("Active_animation");
        player_invulnerability = _animator.GetBool("Invulnerability");
        if(player_stats.Taken_dmg){
            health_regen_timer = 0;
            player_stats.Taken_dmg = false;
        }
        health_regen_timer += Time.deltaTime;
        if(health_regen_timer >= _health_regen_delay){
            player_stats.Health_regen();           
        }
        if(player_stats.Taken_stamina){
            stamina_regen_timer = 0;
            player_stats.Taken_stamina = false;
        }
        stamina_regen_timer += Time.deltaTime;
        if(stamina_regen_timer >= _stamina_regen_delay){
            player_stats.Stamina_regen();          
        }
        if(player_stats.current_exp >= player_stats.exp_to_next_level){
            player_stats.level += 1;
            player_stats.exp_to_next_level = player_stats.exp_to_next_level * player_stats.level;
        }
        Handle_healing_player();
        Death_check();
    }
    void Update_UI(){
        _player_UI_info.Set_current_health(player_stats.Current_health/player_stats.Max_health);
        _player_UI_info.Set_current_stamina(player_stats.Current_stamina/player_stats.Max_stamina);
        _player_UI_info.Set_exp_level(player_stats.current_exp,player_stats.exp_to_next_level,player_stats.level);
    }
    void Death_check(){
        if(player_stats.Current_health <=0){
            //play death animation
           Destroy(gameObject,1.5f);
           player_stats.isDead = true;
        }
    }
   
    public void Start_healing_player_process(float duration, float amount_per_second){
        _audio_source.PlayDelayed(0.5f);
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
}
