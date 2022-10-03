using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy_manager : MonoBehaviour
{
    public Armor_info.Armour enemy_Armour;
    public Enemy_statistics enemy_stats;
    public Enemy_statistics instance_enemy_stats;
    public float detection_radius = 10;
    public float distance_to_attack = 1;
    public float minimum_detection_angle = -30;
    public float maximum_detection_angle = 30;
    public float enemy_rotation_speed = 4f;
    public  LayerMask characters_layer_mask;
    public GameObject targeted_character;
    
    //some variables might be put into enemy statistics scriptible object script like health_taken_bar_multiplayer or health/stamina delay/timer? Think about it
   
    [SerializeField] private float _rotation_speed = 22;
    [SerializeField] private float health_regen_delay = 10;
    [SerializeField] private float stamina_regen_delay = 3;
    [SerializeField] private float _enemy_speed = 15;
    public bool performing_action = false;
    [SerializeField] private GameObject _over_head_info_display_prefab;
    [SerializeField] private Camera _camera_to_rotate_to;
    public GameObject player;
    [SerializeField] private Canvas _world_canvas;
    [SerializeField] private Enemy_attack[] _enemy_attacks;
    [SerializeField] private Enemy_weapon_slot_manager _enemy_weapon_slot_manager;
    public Enemy_attack current_attack;

    private float _health_regen_timer;
    private float _stamina_regen_timer;
    public float current_recovery_time = 0.0f;
    private GameObject _instance_enemy_HUD;
    public NavMeshAgent nav_mesh_agent;
    public Animator animator;

    private float _poison_timer =0;
    private float _poison_time=0;
    private float _poison_damage = 0;
     [SerializeField] private bool _isPoisoned = false;

    private void Awake() {
        nav_mesh_agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _enemy_weapon_slot_manager = GetComponent<Enemy_weapon_slot_manager>();
    }
    private void Start() {
        //enemy_stats.Set_defaults_stats(300,100,0.1f,0.01f,20,2,0,0);
        instance_enemy_stats = (Enemy_statistics)ScriptableObject.Instantiate(enemy_stats);
        _instance_enemy_HUD = Instantiate(_over_head_info_display_prefab,gameObject.transform.position,gameObject.transform.rotation) as GameObject;
        _instance_enemy_HUD.transform.SetParent(_world_canvas.transform);
        _instance_enemy_HUD.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = instance_enemy_stats.character_name + " lvl. " + instance_enemy_stats.level;
        instance_enemy_stats.armour_effectiveness = enemy_Armour.Chest_armour + enemy_Armour.Helmet_armour + enemy_Armour.Legs_armour;
    }
    //Put in the scriptable object?? - Characters_statistics.cs
    void Update(){
        if(instance_enemy_stats.isDead)
            return;       
        Handle_HUD_enemies_lock_on_list();
        Handle_stamina_health_regen();
        Handle_death();
        Handle_recovery_time();

        if(_isPoisoned){
            _poison_timer += Time.deltaTime;
            instance_enemy_stats.Take_damage_bypass_armour(_poison_damage * Time.deltaTime);
            if(_poison_timer > _poison_time){
                _isPoisoned = false;
                _instance_enemy_HUD.GetComponent<Enemy_HUD>().End_poisoned();
                _poison_timer = 0;
                _poison_damage = 0;
            }
        }
    }
    private void Handle_HUD_enemies_lock_on_list(){
        if(_instance_enemy_HUD != null){
            _instance_enemy_HUD.transform.GetComponent<RectTransform>().position =  new Vector3( gameObject.transform.position.x,gameObject.transform.position.y + GetComponent<CapsuleCollider>().height + 0.3f,gameObject.transform.position.z);  
            _instance_enemy_HUD.transform.localRotation = Quaternion.RotateTowards(_instance_enemy_HUD.transform.rotation,_camera_to_rotate_to.transform.rotation,180);
            if(player != null){
                if(Vector3.Distance(transform.position,player.transform.position) > 10){
                    _instance_enemy_HUD.SetActive(false);
                    if(player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Contains(gameObject))
                        player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Remove(gameObject);
                }  
                else{
                    _instance_enemy_HUD.SetActive(true);
                    if(!player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Contains(gameObject))
                        player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Add(gameObject);
                }
            }
        }
       
    }
    private void Handle_stamina_health_regen(){
        if(instance_enemy_stats.Taken_dmg){
            _health_regen_timer = 0;
            instance_enemy_stats.Taken_dmg = false;
            _instance_enemy_HUD.GetComponent<Enemy_HUD>().Set_health_bar_fill_amount(instance_enemy_stats.Current_health/instance_enemy_stats.Max_health);
        }
        _health_regen_timer += Time.deltaTime;
        if(_health_regen_timer >= health_regen_delay){
            instance_enemy_stats.Health_regen(); 
           _instance_enemy_HUD.GetComponent<Enemy_HUD>().Set_health_bar_fill_amount(instance_enemy_stats.Current_health/instance_enemy_stats.Max_health);          
        }
        if(instance_enemy_stats.Taken_stamina){
            _stamina_regen_timer = 0;
            instance_enemy_stats.Taken_stamina = false;
        }
        _stamina_regen_timer += Time.deltaTime;
        if(_stamina_regen_timer >= stamina_regen_delay){
            instance_enemy_stats.Stamina_regen();          
        }
    }
    private void Handle_death(){
         if(instance_enemy_stats.Current_health <= 0){
            animator.CrossFade("Death",0f,0);
            instance_enemy_stats.isDead = true;
            player.GetComponent<Player_info>().player_stats.current_exp += instance_enemy_stats.exp_reward;
            if(player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Contains(gameObject))
                player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Remove(gameObject);
            if(player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Count == 0 && player.GetComponent<Player_info>().locked_on_enemy)
                player.GetComponent<Player_Movemnet.Movement>().Release_lock_on_enemy();
            Destroy(gameObject,2.167f);
            Destroy(_instance_enemy_HUD);
            Destroy(instance_enemy_stats,2.167f);
        }
    }
    private void Handle_recovery_time(){
        if(current_recovery_time > 0){
            current_recovery_time -= Time.deltaTime;
        }
        if(performing_action && current_recovery_time <=0){
            performing_action = false;
        }
    }
    public void Poisoning(float poison_time, float poison_damage){
        //Debug.Log("enemy poisoned");
        _isPoisoned = true;
        _poison_time = poison_time;
        _poison_damage = poison_damage;
        _instance_enemy_HUD.GetComponent<Enemy_HUD>().Start_poisoned(_poison_time);
    }
    public void Get_new_attack(){
        Vector3 target_direction = targeted_character.transform.position - transform.position;
        float viewable_angle = Vector3.Angle(target_direction,transform.forward);
        int sum_attack_chance_score = 0;

        foreach(Enemy_attack enemy_attack in _enemy_attacks){
            if(Vector3.Distance(transform.position,targeted_character.transform.position) >= enemy_attack.minimum_distance_to_attack && Vector3.Distance(transform.position,targeted_character.transform.position) <= enemy_attack.maximum_distance_to_attack && viewable_angle >= enemy_attack.minimum_attack_angle && viewable_angle <= enemy_attack.maximum_attack_angle){
                sum_attack_chance_score += enemy_attack.attack_chance_score;
            }
        }
        int random_val = Random.Range(0,sum_attack_chance_score);
        int temp_val = 0;
        // order of the attacks in the _enemy_attacks array has impact on how often each attack occure - change method
        foreach(Enemy_attack enemy_attack in _enemy_attacks){
            if(Vector3.Distance(transform.position,targeted_character.transform.position) >= enemy_attack.minimum_distance_to_attack && Vector3.Distance(transform.position,targeted_character.transform.position) <= enemy_attack.maximum_distance_to_attack && viewable_angle >= enemy_attack.minimum_attack_angle && viewable_angle <= enemy_attack.maximum_attack_angle){
                if(current_attack != null)
                    return ;
                temp_val += enemy_attack.attack_chance_score;
                if(temp_val > random_val){
                    // checks for the attacks requirements about weapons
                    if(enemy_attack.require_left_weapon){
                        if(_enemy_weapon_slot_manager.left_hand_weapon != null)
                            current_attack = enemy_attack;
                    }
                    if(enemy_attack.require_right_weapon){
                        if(_enemy_weapon_slot_manager.right_hand_weapon != null)
                           current_attack = enemy_attack; 
                    }
                    if(enemy_attack.require_left_weapon && enemy_attack.require_right_weapon){
                        if(_enemy_weapon_slot_manager.left_hand_weapon != null && _enemy_weapon_slot_manager.right_hand_weapon != null)
                            current_attack = enemy_attack;  
                    }
                    
                }
                    
            }
        }
    }
}
