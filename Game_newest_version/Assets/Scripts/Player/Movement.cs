/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterControlle_r _Controler;
    [SerializeField]
    private float Speed = 5.0f;
    [SerializeField]
    private float Jump_height = 4.0f;
    private bool jumping = false;
    private Vector2 Input_vector;
    private PlayerInput Player_input;
    private InputAction Jump_action;
    private void Awake() {
        Player_input = GetComponent<PlayerInput>();
        Jump_action = Player_input.actions["Jump"];
    }
    private void Start(){
        _Controler = GetComponent<CharacterControlle_r>();
       // context = GetComponent<InputAction.CallbackContext>();
    }

    // Update is called once per frame
    void Update(){
       
    }
    /*
    public void Move (InputAction.CallbackContext context){
        if(context.performed){
            Debug.Log("Go" + context.phase);
            Input_vector = context.ReadValue<Vector2>();
            Vector3 Move = new Vector3(Input_vector.x,0,Input_vector.y);
            _Controler.Move(Move * Speed);
        } 
    }
    public void Jump(InputAction.CallbackContext context){
        
        if(context.performed){
            Debug.Log( "Jump" + context.phase);
            Vector3 Move = new Vector3(0,Jump_height,0);
            _Controler.Move(Move);
            jumping = true;
        }
       

    }
    
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

namespace Player_Movemnet{

   
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        //figure out wayy to use pointers and addresses
        public int dropped_item_new_amount;
        
       

        public bool player_grounded = false;
        public bool player_crouching = false;
        
      
        // public bool Door_touched = false; // variable used to control door in another script but I decided to handle it here  - not sure if good habbit but another script would be too simple
        public bool Trap_active = false; 
        //Should do Tooltip to every variable
        [Tooltip("Speed of character in m/s for walking")]
        [SerializeField] private float _walk_speed = 3.0f;
        #region Stamina costs
        [SerializeField] private float  _sprinting_stamina_cost = 1.5f;
        [SerializeField] private float  _jump_stamina_cost = 3.0f;
        [SerializeField] private float _dash_stamina_cost = 15;
        #endregion
        [SerializeField] private float _sprint_speed = 9.0f;
        [SerializeField] private float _crouch_speed = 1.0f;
        [SerializeField] private float _player_speed_change_rate = 10.0f;
        [SerializeField] private float _player_rotation_speed = 7.0f;
        [SerializeField] private float _jump_height = 1.5f;
        [SerializeField] private float _gravity_force = -12f;
        [SerializeField] private float _Time_between_jumps = 0.6f;
        [SerializeField] private float _Time_between_landing_next_jump;     
        [SerializeField] private float _Target_angle;        
        [SerializeField] private bool _Player_hit_ceiling = false;        
        [SerializeField] private float _Grounded_check_radious  = 0.19f;
        [SerializeField] private float _Grounded_help = -0.09f;
        [SerializeField] private float _Ceiling_help = -1.65f;
        [SerializeField] private LayerMask _ground_layer;
        [SerializeField] private float _Player_speed;
        [SerializeField] private float _dash_speed;
        [SerializeField] private float _Player_current_horizontal_speed;
        [SerializeField] private float _Player_target_speed;
        [SerializeField] private RaycastHit _hit;
        [SerializeField] private bool _active_action;
        [SerializeField] private float _Distance_to_interact = 0.9f;
        [SerializeField] private float _Touch_force = 80f;
        [SerializeField] private float _Door_side=0;
        [SerializeField] private GameObject _interact_pop_up;
        [SerializeField] private GameObject _load_strong_attack_fillbar;
        [SerializeField] private LayerMask _interact_layer;
        [SerializeField] private GameObject _object_inventory;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private   Transform _root_for_shot_raycast;
        [SerializeField] private GameObject _free_look_camera;
        [SerializeField] private GameObject _lock_on_camera;
        [SerializeField] private CinemachineTargetGroup _target_group;
        [SerializeField] private LayerMask _environment_layer;

        
        private  GameObject _object_to_interact;
        private CharacterController _controller;
        private Input_handler _input_handler;
        private Vector3 _Player_velocity = new Vector3(0,0,0);
        private Transform _main_camera;
        private Vector2 _input_vector;
        private Animator _animator;
        private Vector3 _Current_position;
        private Vector3 _Last_position;
        private Vector3 _character_speed;
        private Vector3 _Move;
        private Attack.Player_attack  _handle_attacks;
        private Player_statistics _player_statistics;
        private Player_info _player_info;
        private Player_inventory_info.Player_inventory _player_inventory;
        private bool _text_added = false;
        private string _back_up_interact_text;
        private float _strong_attack_time_elapsed = 0.0f;
        private int _enemies_counter = -1;
        
        // on trigger enter depended on button pressed
        private bool _in_area_to_interact_chest = false;
         private bool _in_area_to_interact_bush = false;
        private bool _in_area_to_interact_door = false;
        private bool _in_area_to_interact_dropped_items = false;

        private GameObject _closest_enemy;
        public  List<GameObject> enemies_lock_on = new List<GameObject>();
        
        
        // to the same for text
        // number of dropped items doesn't change
        [SerializeField] private List<Collider> _trigger_colliders = new List<Collider>();
        [SerializeField] private Collider _collider_to_interact;

        private void Start(){
            _controller = GetComponent<CharacterController>();
            _animator = gameObject.GetComponentInChildren<Animator>();
            _main_camera = Camera.main.transform;
            _Time_between_landing_next_jump = _Time_between_jumps;
            _handle_attacks = GetComponent<Attack.Player_attack>();
            _player_info = GetComponent<Player_info>();
            _player_statistics =  _player_info.player_stats;
            _player_inventory = GetComponent<Player_inventory_info.Player_inventory>();
            _input_handler = GetComponent<Input_handler>();
            _back_up_interact_text = _interact_pop_up.GetComponentInChildren<Text>().text;


             _Current_position = transform.position;
             _Last_position = transform.position;
            //_interact_pop_up = GameObject.FindObjectOfType<Canvas>();


            //read on this shit layermask from raycast return number but from variable set in inspector UnityEngine.layermask
            _environment_layer = LayerMask.NameToLayer("Environment");
        }
        //Physics should be done here - chagne TIme.deltaTime for Time.FixeddeltaTime
        private void FixedUpdate() {
             _Current_position = transform.position;
            _character_speed = (_Current_position - _Last_position)/Time.fixedDeltaTime;
            _Last_position = _Current_position;

            //camera shakes really badly
            // if(!_Active_action && !_player_inventory.inventory_open)
            // Move_player();
        }
        void Update(){
            _input_handler.Check_flags();
            if(_player_info.locked_on_enemy){
                Handle_state_while_lock_on_enemy();
            }
            _active_action = _player_info.active_animation;
            Gravity();
            Ground_check();
            Handle_jump_on_top_of_enemy();
            Ceiling_check();
            if(!_active_action){
                Equip_items();
                if(!_player_inventory.inventory_open){
                    Lock_on_enemy();
                    Jump();
                    Crouch();
                    Move_player();
                    if(!_player_info.locked_on_enemy)
                        Rotate_player();
                    Attack();
                    Interact();
                }
            }
        }
        private void LateUpdate() {
            Set_input_flags_false();
        }
        void  Equip_items(){
            if(_input_handler.inventory_flag)
                _player_inventory.Handle_inventory();
            if(_input_handler.inventory_close_inv_flag)
                _object_inventory.SetActive(false);
            if(_input_handler.left_weapon_flag)
                _player_inventory.Check_left_weapon();
            if(_input_handler.right_weapon_flag)
                _player_inventory.Check_right_weapon();
        }
        private void OnTriggerEnter(Collider other){
            // _trigger_colliders.Add(other);
            if(other.CompareTag("Trap")){
            //other.gameObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * 100f);
                Debug.Log("Trap activated");
                Trap_active = true;
            }
            else if(other.CompareTag("Door_pull")){
               // Debug.Log("Door pull setting");
                _Door_side = -1;
            }
            else if(other.CompareTag("Door_push")){
                _Door_side = 1;
                //Debug.Log("Door push setting");
            }
            else{
                //Debug.Log(" random trigger enter");
            }
        }
        private void OnTriggerStay(Collider other){
            //Debug.Log(other);
            if(other.GetComponent<Item_dropped>() ||other.gameObject.tag == "Chest"|| other.gameObject.tag == "Door" || other.gameObject.tag == "Bush"){
                Vector3 interactable_object_dir = other.transform.position - gameObject.transform.position;
                interactable_object_dir.y = 0;
                interactable_object_dir.Normalize();
                float angle = Vector3.Angle(gameObject.transform.forward, interactable_object_dir);
                if(angle < 45 && angle > -45){
                    if(!_trigger_colliders.Contains(other))
                        _trigger_colliders.Add(other);
                    //dropped items are first because their radious is smaller so they need to be detected before other interactable objects with bigger radious
                   
                }
                else{
                    if(_trigger_colliders.Contains(other))
                        _trigger_colliders.Remove(other);
                    _in_area_to_interact_chest = false;
                    _in_area_to_interact_door = false;
                    _in_area_to_interact_dropped_items = false;
                    _in_area_to_interact_bush = false;
                    //Debug.Log("Item out of reach");
                    Reset_turn_off_interact_pop_up();
                }
            }
            float distance = Mathf.Infinity;
            foreach(var interact_collider in _trigger_colliders){
                if(Vector3.Distance(gameObject.transform.position,interact_collider.gameObject.transform.position) < distance){
                    distance = Vector3.Distance(gameObject.transform.position,interact_collider.gameObject.transform.position);
                    _collider_to_interact = interact_collider;
                    Reset_turn_off_interact_pop_up();
                }
            }
            if(_collider_to_interact == null)
                return ;
            if(_collider_to_interact.GetComponent<Item_dropped>()){
                //Debug.Log("Dropped item in area");
                _in_area_to_interact_dropped_items = true;
                _interact_pop_up.SetActive(true);
                
                if(!_text_added){
                    _interact_pop_up.GetComponentInChildren<Text>().text += _collider_to_interact.GetComponent<Item_dropped>().interactable_text;
                    _text_added = true;
                }               
            }
            else if(_collider_to_interact.gameObject.tag == "Chest"){
                _in_area_to_interact_chest = true;
                _interact_pop_up.SetActive(true);
                if(!_text_added){
                    _interact_pop_up.GetComponentInChildren<Text>().text += _collider_to_interact.GetComponent<Interactable_objects>().interactable_text;
                    _text_added = true;
                }
            }
            else if(_collider_to_interact.gameObject.tag == "Bush"){
                _in_area_to_interact_bush = true;
                _interact_pop_up.SetActive(true);
                if(!_text_added){
                    _interact_pop_up.GetComponentInChildren<Text>().text += _collider_to_interact.GetComponent<Bush_contents>().interactable_text;
                    _text_added = true;
                }
            }
            else if(_collider_to_interact.gameObject.tag == "Door"){
                //Debug.Log("Opening Door");
                // Door_touched = true;
                _in_area_to_interact_door = true;
                _interact_pop_up.SetActive(true);
                if(!_text_added){
                    _interact_pop_up.GetComponentInChildren<Text>().text += _collider_to_interact.GetComponent<Interactable_objects>().interactable_text;
                    _text_added = true;
                }
            }
            
        }
        private void OnTriggerExit(Collider other){
            //_trigger_colliders.RemoveAt(0);
            if(other.CompareTag("Trap")){
                Debug.Log("Trap deactivated");
                Trap_active = false;
            }
            if(other.GetComponent<Item_dropped>() || other.gameObject.tag == "Chest" || other.gameObject.tag == "Door"){
                if(_trigger_colliders.Contains(other))
                    _trigger_colliders.Remove(other);
                if(_trigger_colliders.Count == 0)
                    _collider_to_interact = null;
                _in_area_to_interact_chest = false;
                _in_area_to_interact_door = false;
                _in_area_to_interact_dropped_items = false;
                //Debug.Log("Item out of reach");
                Reset_turn_off_interact_pop_up();
            }
        }
        private void Interact(){
            if(_input_handler.interact_flag && _trigger_colliders.Count > 0){
                if(_in_area_to_interact_dropped_items){
                    _canvas.GetComponent<Inventories>().Add_item_to_player_inv_check_for_same_object(_collider_to_interact.GetComponent<Item_dropped>().item_dropped,_collider_to_interact.GetComponent<Item_dropped>().amount_of_dropped_items);
                    
                    if( _canvas.GetComponent<Inventories>().added_all_items){
                        Destroy(_collider_to_interact.gameObject);
                        if(_trigger_colliders.Contains(_collider_to_interact))
                            _trigger_colliders.Remove(_collider_to_interact);
                        if(_trigger_colliders.Count == 0)
                            _collider_to_interact = null;
                        _in_area_to_interact_dropped_items = false;
                        Reset_turn_off_interact_pop_up();
                    }
                    else{
                        _collider_to_interact.GetComponent<Item_dropped>().amount_of_dropped_items = dropped_item_new_amount;
                    }        
                }
                else if(_in_area_to_interact_chest){
                    _object_to_interact =_collider_to_interact.gameObject;   
                    _object_to_interact.GetComponent<AudioSource>().Play();
                    //StartCoroutine(Coloring_object());
                    Function_timer.Create(() => Set_chest_color(Color.red),2);
                    Function_timer.Create(() => Set_chest_color(Color.white),8);
                    _object_inventory.SetActive(true);
                    _canvas.GetComponent<Inventories>().object_inv_to_show = _object_to_interact.GetComponent<Interactable_objects>();
                    _canvas.GetComponent<Inventories>().Assign_weapons_amount_to_slots();
                    _player_inventory.Handle_inventory();
                }
                else if(_in_area_to_interact_bush){
                    _animator.CrossFade("Gathering",0);
                   _canvas.GetComponent<Inventories>().Add_item_to_player_inv_check_for_same_object(_collider_to_interact.GetComponent<Bush_contents>().berry_in_bush,_collider_to_interact.GetComponent<Bush_contents>().amount_of_berry); 
                    if(_canvas.GetComponent<Inventories>().added_all_items){
                        _collider_to_interact.GetComponent<Bush_contents>().amount_of_berry = 0;
                    }
                    else{
                        _collider_to_interact.GetComponent<Bush_contents>().amount_of_berry = dropped_item_new_amount;
                    }
                    
                }
                else if(_in_area_to_interact_door){
                    _collider_to_interact.GetComponent<Rigidbody>().AddForce(_Door_side*_root_for_shot_raycast.forward * _Touch_force * _Player_speed,ForceMode.Acceleration);
                }
            }
                // old version with raycasts
                /*
            // use raycast or ontrigger enter
            if(Physics.Raycast(_root_for_shot_raycast.position, _root_for_shot_raycast.forward, out _Hit, _Distance_to_interact,_interact_layer)){
                Debug.DrawRay(_root_for_shot_raycast.position, _root_for_shot_raycast.forward,Color.blue,_Distance_to_interact);
                   _interact_pop_up.SetActive(true);
                  
                    if(!_text_added){
                        _interact_pop_up.GetComponentInChildren<Text>().text += _Hit.collider.GetComponent<Interactable_objects>().interactable_text;
                        _text_added = true;
                    }
                    
              
                if(_input_handler.interact_flag){
                   
                    //Debug.Log("Raycast hit something");
                    if(_Hit.collider.gameObject.tag == "Chest"){
                        
                        
                        //Debug.Log("Opening chest");
                     
                        //_object_to_interact = GameObject.FindWithTag("Chest"); // gets random object in the scene
                        /*
                        //gets closest object
                        GameObject[] Gos;
                        Gos = GameObject.FindGameObjectsWithTag("Chest");
                        GameObject Closest = null;
                        float Distance_to_object = Mathf.Infinity;
                        Vector3 Player_position = transform.position;
                        foreach(GameObject Founded_object in Gos ){
                            Vector3 Diffrence = Founded_object.transform.position - Player_position;
                            float Current_distance = Diffrence.sqrMagnitude;
                            if (Current_distance < Distance_to_object){
                                Closest = Founded_object;
                                Distance_to_object = Current_distance;
                            }
                        }
                        _object_to_interact = Closest;
                        
                        _object_to_interact = _Hit.collider.gameObject;
                        _object_to_interact.GetComponent<AudioSource>().Play();
                        StartCoroutine(Coloring_object());

                        _object_inventory.SetActive(true);
                        
                        _canvas.GetComponent<Inventories>().object_inv_to_show = _object_to_interact.GetComponent<Interactable_objects>();
                        _canvas.GetComponent<Inventories>().Assign_weapons_amount_to_slots();
                        _player_inventory.Handle_inventory();
            
                        // if(_object_to_interact.GetComponent<Interactable_objects>().weapon_to_pickup != null){
                        //     _player_inventory.Add_weapon_to_list(_object_to_interact.GetComponent<Interactable_objects>().weapon_to_pickup);
                        //     Debug.Log("Added " +_object_to_interact.GetComponent<Interactable_objects>().weapon_to_pickup.Item_name  +" to inventory");
                        //     _object_to_interact.GetComponent<Interactable_objects>().weapon_to_pickup = null;
                        // }
                            

                    }
                    else if(_Hit.collider.gameObject.tag == "Door"){
                        Debug.Log("Opening Door");
                       // Door_touched = true;
                        _Hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(_Door_side*_root_for_shot_raycast.forward * _Touch_force * _Player_speed,ForceMode.Acceleration);

                        
                    }
                    
                
                }                
            }
            else{
                _interact_pop_up.SetActive(false);
                _text_added = false;
                _interact_pop_up.GetComponentInChildren<Text>().text = _back_up_interact_text;
            }
            */
        }
        private void Set_chest_color(Color color){
            _object_to_interact.GetComponent<Renderer>().material.color = color;
        }
        private void Reset_turn_off_interact_pop_up(){
            _interact_pop_up.SetActive(false);
            _text_added = false;
            _interact_pop_up.GetComponentInChildren<Text>().text = _back_up_interact_text;
        }
        private IEnumerator Coloring_object(){
            _object_to_interact.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(4.5f);
            _object_to_interact.GetComponent<Renderer>().material.color = Color.white;
        }
        private void Attack(){
                if(_input_handler.attack_light_flag){
                    //Debug.Log("Light attack button pressed");
                    _handle_attacks.Handle_light_attack();
                }
                else if(_input_handler.attack_combo_flag){
                    _handle_attacks.Handle_combo_attack();
                }
                else if(_input_handler.attack_strong_started_flag){
                    _strong_attack_time_elapsed +=Time.deltaTime;                        
                    if(_strong_attack_time_elapsed >= 0.2f && !_load_strong_attack_fillbar.activeSelf)
                        _load_strong_attack_fillbar.SetActive(true);
                    //this works ok but isn't really accurate way of showing strong attack hold progress - maybe some algorithm or somethnig don't know 
                    _load_strong_attack_fillbar.GetComponent<Slider>().value += Time.deltaTime/2;
                }
                if(_input_handler.attack_strong_canceled_flag){
                    _load_strong_attack_fillbar.GetComponent<Slider>().value = 0;
                    _load_strong_attack_fillbar.SetActive(false);
                    _strong_attack_time_elapsed = 0.0f;
                }
                else if(_input_handler.attack_strong_performed_flag){
                    _handle_attacks.Handle_heavy_attack();
                    _load_strong_attack_fillbar.GetComponent<Slider>().value = 0;
                    _load_strong_attack_fillbar.SetActive(false);
                    _strong_attack_time_elapsed = 0.0f;
                }
                else if(_input_handler.attack_special_flag){
                    _handle_attacks.Handle_special_attack();
                }
        }
        private void Move_player(){
            _input_vector = _input_handler.walk_input;
            if(_player_info.locked_on_enemy && _animator.GetBool("Active_animation") == false){
                //Debug.Log(_animator.GetBool("Active_animation") + "combat movement");
                _animator.SetBool("Combat_movement",true);
                if(_input_vector == Vector2.zero){
                    _animator.CrossFade("Base Layer.Lock on movement.Lock on Idle",0,0);
                }
                if(_input_vector.x > 0 && _input_vector.y == 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Right",0,0);
                else if(_input_vector.x < 0 && _input_vector.y == 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Left",0,0);
                else if(_input_vector.y > 0 && _input_vector.x == 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Forward",0,0);
                else if(_input_vector.y > 0 && _input_vector.x > 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Forward-Right",0,0);
                else if(_input_vector.y > 0 && _input_vector.x < 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Forward-Left",0,0);
                else if(_input_vector.y < 0 && _input_vector.x == 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Backward",0,0);
                else if(_input_vector.y < 0 && _input_vector.x > 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Backward-Right",0,0);
                else if(_input_vector.y < 0 && _input_vector.x < 0)
                    _animator.CrossFade("Base Layer.Lock on movement.Unarmed-Strafe-Backward-Left",0,0);
            }
            else{
                _animator.SetBool("Combat_movement",false);
           
            //   if(player_grounded){
                    //_controller.velocity to work properly requires only one _controller.move - tried to combine gravity,jump and mve into one but 
                    //Debug.Log("speed: " + _controller.velocity); 
                   // _Player_current_horizontal_speed = new Vector3 (_controller.velocity.x,0.0f,_controller.velocity.z).magnitude;    
                    _Player_current_horizontal_speed = new Vector3(_character_speed.x,0f,_character_speed.z).magnitude;
                   
                    if(player_crouching){
                        _Player_target_speed = _crouch_speed;
                    }
                    else if(_input_handler.sprint_flag && _player_statistics.Current_stamina >0){
                        _player_statistics.Take_stamina(_sprinting_stamina_cost * Time.deltaTime);
                        _Player_target_speed = _sprint_speed;
                        _animator.SetFloat("Speed_percent",0.99f);//, 0.1f,Time.deltaTime); // last 2 arguments to smooth transision - now useless but great when i change 0.99f to player speed - fix the bug
                    }
                    else{
                        _Player_target_speed = _walk_speed;
                        _animator.SetFloat("Speed_percent",0.3f);//, 0.1f,Time.deltaTime);
                    } 
                    if(_input_vector == Vector2.zero){
                        _Player_target_speed = 1f;  // doesn't change _controller.Move becasue INput vector is 0 but allows to make depanded on speed functions etc while player is in  one place
                        _animator.SetFloat("Speed_percent",-1f);//, 0.1f,Time.deltaTime);
                    }
                    //technically it works but value is inconsistent and so/through this player speed is also inconsistent and camera shakes 
                    //Debug.Log("speed: " + _Player_current_horizontal_speed);
                    // if(_Player_current_horizontal_speed < _Player_target_speed - 0.1f || _Player_current_horizontal_speed > _Player_target_speed + 0.1f ){
                    //         _Player_speed = Mathf.Lerp(_Player_current_horizontal_speed,_Player_target_speed,Time.deltaTime * _player_speed_change_rate);
                    //         _Player_speed = Mathf.Round(_Player_speed * 1000f) / 1000f;
                    // }
                    else
                        _Player_speed = _Player_target_speed;
                
                    
                    _Move = new Vector3(_input_vector.x,0,_input_vector.y);
                    _Move  = _main_camera.forward * _Move.z + _main_camera.right * _Move.x;
                    _Move.y = 0f; 
                    _controller.Move(_Move * Time.deltaTime * _Player_speed);
            //  }
            }
        }
        private void Gravity(){
            _Player_velocity.y += _gravity_force * Time.deltaTime;
            if (player_grounded && _Player_velocity.y < 0)
                _Player_velocity.y = 0f;
        
            _controller.Move(_Player_velocity * Time.deltaTime);
        }
        private void Jump(){
            if(!player_grounded && _Player_hit_ceiling){
               // Debug.Log("Player hit ceiling" + _controller.velocity );
                //_controller.move = Vector3.zero;
                // _Player_velocity.y =_gravity_force*2 * Time.deltaTime;
                //_Player_velocity.y = - _Player_velocity.y;
                // _controller.Move(_Player_velocity * Time.deltaTime);
            }
            if(player_grounded && !player_crouching){
                if(_Time_between_landing_next_jump >= 0.0f)
                    _Time_between_landing_next_jump -= Time.deltaTime;

                if (_input_handler.jump_flag && _Time_between_landing_next_jump <=0.0f && _player_statistics.Current_stamina > _jump_stamina_cost){
                    //_Player_velocity.y += Mathf.Sqrt(_jump_height * -2.0f * _gravity_force); // in theory should take care speed to jump higher - very high jumpo at stairs - not sure how it works
                    // _Player_velocity = _Move;
                    _player_statistics.Take_stamina(_jump_stamina_cost * Time.deltaTime);
                    //Debug.Log("Taken stamina: " + _jump_stamina_cost * Time.deltaTime);
                    _Player_velocity.y = Mathf.Sqrt(_jump_height * -2.0f * _gravity_force);
                    _controller.Move(_Player_velocity * Time.deltaTime);
                }
            // Debug.Log("X: " + move.x +"Y: " + _Player_velocity.y +"Z: " + move.z );
            // _controller.Move(new Vector3(move.x,_Player_velocity.y,move.z) * Time.deltaTime * _Player_speed);
            }
            else{
                _Time_between_landing_next_jump = _Time_between_jumps;
            }
        }
        private void Crouch(){
            if(_input_handler.crouch_flag&& player_grounded){
                if(player_crouching){
                    player_crouching = false;
                    _animator.CrossFadeInFixedTime("Base Layer.Idle",0.25f,0);
                    //Debug.Log("end crouching");
                }
                else{
                    player_crouching = true;
                    _animator.CrossFadeInFixedTime("Base Layer.Crouching",0.25f,0);
                    //Debug.Log("start crouching");
                }  
            }
            if(player_crouching){
                _controller.height = 1f;
                _controller.center = (new Vector3(0,0.5f,0));
            }
            else{
                _controller.height = 1.78f;
                _controller.center = (new Vector3(0,0.9f,0));
            }
        }        
        private void Ground_check(){
            // player_grounded = _controller.isGrounded; // buggy as fuck
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y - _Grounded_help,transform.position.z );
            player_grounded = Physics.CheckSphere(Sphere_position,_Grounded_check_radious,_ground_layer,QueryTriggerInteraction.Ignore);
            //Debug.Log("Grounded: " + player_grounded);
        }
        private void Handle_jump_on_top_of_enemy(){
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y + 2 * _Grounded_help,transform.position.z);
            if(Physics.CheckSphere(Sphere_position,_Grounded_check_radious,128,QueryTriggerInteraction.Ignore)){
                //Debug.Log("Push player from the top of the enemy");
                //Vector3 move = new Vector3()
                _controller.Move( Vector3.back * Time.deltaTime * 4f);
            }
        }   
        private void Ceiling_check(){
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y - _Ceiling_help ,transform.position.z );
            _Player_hit_ceiling = Physics.CheckSphere(Sphere_position,_Grounded_check_radious,_ground_layer,QueryTriggerInteraction.Ignore);
        }
        private void Rotate_player(){
            if(_input_vector != Vector2.zero){
                _Target_angle = Mathf.Atan2(_input_vector.x,_input_vector.y) * Mathf.Rad2Deg + _main_camera.eulerAngles.y;
                Quaternion Rotation = Quaternion.Euler(0f,_Target_angle,0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Rotation,Time.deltaTime * _player_rotation_speed);
            }
        }
        private void Lock_on_enemy(){
            if(_input_handler.lock_on_flag && !_player_info.locked_on_enemy && enemies_lock_on.Count > 0){
                float distance_to_closest_enemy = Mathf.Infinity;
                foreach( var enemy in enemies_lock_on){
                    float viewable_anlge = Vector3.Angle(enemy.transform.position - transform.position,_root_for_shot_raycast.forward);
                    if(viewable_anlge > -50 && viewable_anlge < 50 && Is_enemy_to_lock_on_visible(enemy)){
                         if(Vector3.Distance(gameObject.transform.position,enemy.transform.position) < distance_to_closest_enemy){
                            distance_to_closest_enemy = Vector3.Distance(gameObject.transform.position,enemy.transform.position);
                            _closest_enemy = enemy.gameObject;
                        }    
                    }
                }
                // Collider[] enemies = Physics.OverlapSphere(transform.position,10);
                // float distance_to_closest_enemy = Mathf.Infinity;
                // foreach(var enemy in enemies){
                //     float viewable_anlge = Vector3.Angle(enemy.transform.position - transform.position,_root_for_shot_raycast.forward);
                //     if(enemy.CompareTag("Enemy") && viewable_anlge > -50 && viewable_anlge < 50 ){
                //         enemies_lock_on.Add(enemy.gameObject);
                //         if(Vector3.Distance(gameObject.transform.position,enemy.transform.position) < distance_to_closest_enemy){
                //             distance_to_closest_enemy = Vector3.Distance(gameObject.transform.position,enemy.transform.position);
                //             _closest_enemy = enemy.gameObject;
                //         }    
                //     } 
                // }
                if(_closest_enemy != null){
                    _target_group.AddMember(_closest_enemy.transform,1,2);
                    _player_info.locked_on_enemy = true;
                    _lock_on_camera.SetActive(true);
                }
            }
            else if(_input_handler.lock_on_flag && _player_info.locked_on_enemy)
                Release_lock_on_enemy();
        }
        public void Release_lock_on_enemy(){
            _player_info.locked_on_enemy = false;
            _free_look_camera.SetActive(true);
            _target_group.RemoveMember(_closest_enemy.transform);
            _closest_enemy = null;
        }
        private void Switch_enemy_locked_on(int index){
            _target_group.RemoveMember(_closest_enemy.transform);
            _closest_enemy = enemies_lock_on[index];
            _target_group.AddMember(_closest_enemy.transform,1,2);
        }
        private void Handle_state_while_lock_on_enemy(){
            Handle_dashes();
           // Debug.Log(_animator.GetBool("Active_animation") + "Dashes movement");
            if(_input_handler.switch_flag && enemies_lock_on.Count > 1){
                if(enemies_lock_on.Count == _enemies_counter+1){
                    _enemies_counter = -1;
                }
                _enemies_counter++;
                if( _closest_enemy != enemies_lock_on[_enemies_counter]){
                    do{
                        if(Is_enemy_to_lock_on_visible(enemies_lock_on[_enemies_counter]) &&  _closest_enemy != enemies_lock_on[_enemies_counter]){
                            Switch_enemy_locked_on(_enemies_counter);
                            break;
                        }
                        else if(_enemies_counter + 1 < enemies_lock_on.Count){
                            _enemies_counter++;
                        }
                        else{
                            break;
                        }
                        
                    } while (true);
                }
                else if( _closest_enemy == enemies_lock_on[_enemies_counter]){
                    do{
                        if(Is_enemy_to_lock_on_visible(enemies_lock_on[_enemies_counter+1])){
                            _enemies_counter++;
                            Switch_enemy_locked_on(_enemies_counter);
                            break;
                        }
                        else if(_enemies_counter + 2 < enemies_lock_on.Count){
                            _enemies_counter++;
                        }
                        else{
                            break;
                        }
                        
                    } while (true);
                }
            }
            //transform.LookAt(_closest_enemy.transform);
            //float viewable_anlge = Vector3.Angle(enemy.transform.position - transform.position,_root_for_shot_raycast.forward);
            Vector3 direction = _closest_enemy.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0;
            Quaternion target_rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation,target_rotation,_player_rotation_speed * Time.deltaTime);
        }
        private void Handle_dashes(){
            if(_input_handler.dash_flag && _player_statistics.Current_stamina > _dash_stamina_cost){
                //Debug.Log(_Move * _dash_speed);
                if(_input_handler.walk_input.x < 0){
                    _animator.SetBool("Active_animation",true);
                    _animator.CrossFade("Base Layer.Dash_left",0f,0);
                }
                else if(_input_handler.walk_input.x > 0){
                    _animator.SetBool("Active_animation",true);
                    _animator.CrossFade("Base Layer.Dash_right",0f,0);
                }
                    
                else
                    return;
                _player_statistics.Take_stamina(_dash_stamina_cost);
            }
        }
        public bool Is_enemy_to_lock_on_visible(GameObject potential_enemy_to_lock_on){
            if(Physics.Linecast(transform.position,potential_enemy_to_lock_on.transform.position, out _hit)){
                //Debug.DrawLine(transform.position,_closest_enemy.transform.position);
                if(_hit.transform.gameObject.layer == _environment_layer )
                    return false;
                else
                    return true;
            }
            else
                return true;                
        }
        private void OnDrawGizmosSelected(){
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (player_grounded)
                Gizmos.color = transparentGreen;
            else 
                Gizmos.color = transparentRed;
                
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _Grounded_help, transform.position.z), _Grounded_check_radious);
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _Ceiling_help, transform.position.z), _Grounded_check_radious);
        }
        private void Set_input_flags_false(){
            //Player action map
            _input_handler.sprint_flag = false;
            _input_handler.crouch_flag = false;
            _input_handler.jump_flag = false;
            _input_handler.left_weapon_flag = false;
            _input_handler.right_weapon_flag = false;
            _input_handler.first_potion_flag = false;
            _input_handler.second_potion_flag = false;
            _input_handler.third_potion_flag = false;
            _input_handler.fourth_potion_flag = false;
            _input_handler.interact_flag = false;
            _input_handler.attack_light_flag = false;
            _input_handler.attack_strong_performed_flag = false;
            _input_handler.attack_special_flag = false;
            _input_handler.attack_combo_flag = false;
            _input_handler.inventory_flag = false;
            _input_handler.attack_strong_canceled_flag = false;
            _input_handler.lock_on_flag = false;
            _input_handler.switch_flag = false;
            _input_handler.dash_flag = false;

            //Inventory action map
            _input_handler.inventory_close_inv_flag = false;
            _input_handler.mouse_right_pressed_inv_flag = false;
            _input_handler.mouse_left_pressed_inv_flag = false;
            _input_handler.transfer_items_inv_flag = false;
            _input_handler.drop_items_inv_flag = false;
            _input_handler.use_item_inv_flag = false;
            _input_handler.confirmed_action_inv_flag = false;
            _input_handler.left_weapon_inv_flag = false;
            _input_handler.right_weapn_inv_flag = false;
        }
    }
}
