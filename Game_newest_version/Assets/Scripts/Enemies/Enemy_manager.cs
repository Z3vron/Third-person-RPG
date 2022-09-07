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
    public Enemy_attack current_attack;

    private float _health_regen_timer;
    private float _stamina_regen_timer;
    public float current_recovery_time = 0.0f;
    private GameObject _instance_enemy_HUD;
    public NavMeshAgent nav_mesh_agent;
    public Animator animator;

    private void Awake() {
        nav_mesh_agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
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
        Handle_HUD_enemies_lock_on_list();
        Handle_stamina_health_regen();
        Handle_death();
        Handle_current_action();
        Handle_recovery_time();
    }
    private void Handle_HUD_enemies_lock_on_list(){
        _instance_enemy_HUD.transform.GetComponent<RectTransform>().position =  new Vector3( gameObject.transform.position.x,gameObject.transform.position.y + GetComponent<BoxCollider>().size.y + GetComponent<BoxCollider>().center.y/2,gameObject.transform.position.z);  
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
            player.GetComponent<Player_info>().player_stats.current_exp += instance_enemy_stats.exp_reward;
            if(player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Contains(gameObject))
                player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Remove(gameObject);
            if(player.GetComponent<Player_Movemnet.Movement>().enemies_lock_on.Count == 0 && player.GetComponent<Player_Movemnet.Movement>().locked_on_enemy)
                player.GetComponent<Player_Movemnet.Movement>().Release_lock_on_enemy();
            Destroy(gameObject);
            Destroy(_instance_enemy_HUD);
            Destroy(instance_enemy_stats);
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
    private void Handle_current_action(){
        // if(_targeted_character == null){
        //     Check_for_target();
        // }
        // else if(Vector3.Distance(transform.position,_targeted_character.transform.position) > _distance_to_attack){
        //     Move_to_target();
        // }
        // else if(_nav_mesh_agent.remainingDistance <= _distance_to_attack){
        //     Attack_target();
        // }
    }
     private void Check_for_target(){
        //_animator.SetFloat("Vertical",0);
        // Collider[] hit_colliders = Physics.OverlapSphere(gameObject.transform.position,detection_radius,_characters_layer_mask);
        // foreach( var character_collider in hit_colliders){
        //     Vector3 target_direction = character_collider.transform.position - gameObject.transform.position;
        //     float viewable_angle = Vector3.Angle(target_direction,transform.forward);
        //     if(viewable_angle > _minimum_detection_angle && viewable_angle < _maximum_detection_angle && _player.GetComponent<Player_Movemnet.Movement>().Is_enemy_to_lock_on_visible(gameObject)){ // Is_enemy_to_lock_on_visible uses insidee tranform, the original script is attached to player so transform is player object not enemy object from which i call function
        //         if(character_collider.CompareTag("Enemy") && character_collider.gameObject != gameObject){
        //             //Debug.Log("Found another enemy");
        //             //_targeted_character = character_collider.gameObject;
        //         }
        //         else if(character_collider.CompareTag("Player")){
        //             //Debug.Log("Found player");
        //             _targeted_character = character_collider.gameObject;
        //         }
        //     }
        // }
    }
    private void Move_to_target(){
        // if(_performing_action)
        //     return ;
        // if(Vector3.Distance(transform.position,_player.transform.position) > 25){
        //     _targeted_character = null;
        //     _nav_mesh_agent.isStopped = true;
        //     _animator.SetFloat("Vertical",0);//,0.1f,Time.fixedDeltaTime);//with damping animation is still going 
        // }
        // else{
        //    // Debug.Log("Moving to target");
        //     _nav_mesh_agent.SetDestination(_targeted_character.transform.position);
        //     if(_nav_mesh_agent.isStopped){
        //         _nav_mesh_agent.isStopped = false;
        //     }
        //     _animator.SetFloat("Vertical",1,0.1f,Time.fixedDeltaTime);
        // }
        // else if(_nav_mesh_agent.remainingDistance <= _distance_to_attack){
        //     //nav_mesh_agent.nextPosition = transform.position;
        //     _animator.SetFloat("Vertical",0,0.1f,Time.fixedDeltaTime);
        //     _nav_mesh_agent.velocity = Vector3.zero;
        //     _nav_mesh_agent.isStopped = true;
        //     //Debug.Log("Start  attacking");
        //     transform.LookAt(_player.transform);//could use Quaternion.lerp / Quaternion.Slerp - could control rotation speed
        //     //Debug.Log(Vector3.Distance(transform.position,_targeted_character.transform.position));
        // }
        
    }
    private void Attack_target(){
        // //stop nav mesh agent
        // _animator.SetFloat("Vertical",0,0.1f,Time.fixedDeltaTime);
        // _nav_mesh_agent.velocity = Vector3.zero;
        // _nav_mesh_agent.isStopped = true;
        // //Debug.Log("Start  attacking");
        // transform.LookAt(_player.transform);//could use Quaternion.lerp / Quaternion.Slerp - allow to control  enemy rotation speed
        // if(_performing_action)
        //     return ;
        // if(_current_attack == null){
        //     Get_new_attack();
        // }
        // else{
        //     _performing_action = true;
        //     //recovery_timer includes time for the attack animation so each attack recovery timer should be bigger or just change where i put this
        //     _current_recovery_time = _current_attack.recovery_time;
        //     _animator.SetBool(_current_attack.animation_name,true);
        //     _current_attack = null;
        // }
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
                if(temp_val > random_val)
                    current_attack = enemy_attack;
            }
        }
    }
}
