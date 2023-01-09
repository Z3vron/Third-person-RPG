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
       
        #region Player flags
            [field: SerializeField, Header("Player movement flags")]
            //[Range(1,6)]
            //int,float value
           // array[index++ % array.Length] // will go until last element in array and then go again from the first element
           //[field: SerializeField] public bool player_grounded {get; private set;}
           //header field needs to be serializefield or it won't show in inspector with properties, this means for first property that it doesn't need own serializefield field declaration 
            public bool player_grounded {get; private set;} = false;
            [field: SerializeField]  public bool player_crouching {get; private set;}  = false;
            [field: SerializeField] public bool player_hit_ceiling {get; private set;} = false;
        #endregion
        // public bool Door_touched = false; // variable used to control door in another script but I decided to handle it here  - not sure if good habbit but another script would be too simple
        public bool Trap_active = false; 
        [Tooltip("Force with which player is pushing and pulling doors")]
        [SerializeField] private float _touch_force = 80f;
        [Tooltip("Variable that holds information about which side of doors player is facing(-1 when player will need to pull doors and 1 when he will need to push them")]
        [SerializeField] private float _door_side=0;
        #region Stamina costs
            [Header("Stamina costs")]
            [SerializeField] private float  _sprinting_stamina_cost = 1.5f;
            [SerializeField] private float  _jump_stamina_cost = 3.0f;
            [SerializeField] private float _dash_stamina_cost = 15;
        #endregion
        #region Movement speed values
            [Header("Movement speed values")]
            [Tooltip("Speed of character in m/s while walking")]
            [SerializeField] private float _walk_speed = 3.0f;
            [Tooltip("Speed of character in m/s while sprinting")]
            [SerializeField] private float _sprint_speed = 9.0f;
            [Tooltip("Speed of character in m/s while crouching")]
            [SerializeField] private float _crouch_speed = 1.0f;
            [Tooltip("Speed of character in m/s while changing between speed multiplayers")]
            [SerializeField] private float _player_speed_change_rate = 7.0f;
            [Tooltip("Speed of character in m/s while rotating towards direction camera is facing")]
            [SerializeField] private float _player_rotation_speed = 7.0f;
            [Tooltip("Speed of character while dashing in any direction")]
            [SerializeField] private float _dash_speed;

            //not sure about 3 variables below i think that i could remove atleast one but there is still a problem with getting actual player speed - i tried few diffrent methods but i am not satisfied with any of them
            [Tooltip("Present player speed")]
            [SerializeField] private float _player_speed;
            [Tooltip("Targeted player speed")]
            [SerializeField] private float _player_target_speed;
        #endregion
        #region Jump/Gravity/ ground and ceiling check values
            [Header("Gravity/Jump ground and ceiling check values")]
            [Tooltip("Gravity force that pulls player down")]
            [SerializeField] private float _gravity_force = -12f;
            [Tooltip("Height that player will achieve while jumping")]
            [SerializeField] private float _jump_height = 1.5f;
            [Tooltip("Time that needs to pass before player will be able to jump again")]
            [SerializeField] private float _time_between_jumps = 0.6f;
            [Tooltip("Timer that counts time elapsed from the last landing ")]
            [SerializeField] private float _timer_between_landing_next_jump;     
            [Tooltip("Determine radius of the sphere that checks if player is grounded")] 
            [SerializeField] private float _grounded_check_radious  = 0.19f;
            [Tooltip("Changes transform.y of the sphere that checks if player is grounded")]
            [SerializeField] private float _grounded_help = -0.09f;
            [Tooltip("Changes transform.y of the sphere that checks if player hit ceiling")]
            [SerializeField] private float _ceiling_help = -1.65f;
        #endregion        
        #region UI elements references
            [Header("UI elements references")]
            [Tooltip("UI element that shows user to press E to interact with given object(object name is adjusted) shows when in distance to interact")]
            [SerializeField] private Interact_pop_up _interact_pop_up;
            [Tooltip("UI elemnt that shows status(level) of loading strong attack")]
            [SerializeField] private GameObject _load_strong_attack_fillbar;
            [Tooltip("UI element that shows contents of the object to interact - for now chest only")]
            [SerializeField] private GameObject _object_inventory;
        #endregion
        #region Other scripts and components references
            [Header("Other scripts references")]
            [Tooltip("Script that handles operations on inventories")]
            [SerializeField] private Inventories _inventories;
            private Input_handler _input_handler;
            private Player_attack_handler  _handle_attacks;
            private Player_statistics _player_statistics;
            private Player_info _player_info;
            private Player_inventory_info.Player_inventory _player_inventory;

            private CharacterController _controller;
            private Animator _animator;
            private Transform _main_camera;
        #endregion
        #region Layer masks
            [Header("Layer masks")]
            [Tooltip("Used to detect if player is grounded or if player hit ceiling")]
            [SerializeField] private LayerMask _ground_ceiling_layer;
            [Tooltip("Used to decide if object in the front of player in given radius and distance is interactable")]
            [SerializeField] private LayerMask _interact_layer;
            [Tooltip("Used to detect if there is something(some object) in a way between player and possible enemy to lock on")]
            [SerializeField] private LayerMask _environment_layer;
        #endregion
        #region Cameras
            [Header("Cameras")]
            [Tooltip("Reference to free looking camera - one used while walking on the level")]
            [SerializeField] private GameObject _free_look_camera;
            [Tooltip("Reference to lock on camera - one used while attacking enemy(combat stance/mode)")]
            [SerializeField] private GameObject _lock_on_camera;
            [Tooltip("Reference to cinemachine target group - usadjust lock on camera so that both lock on enemy and player will be in view")]
            [SerializeField] private CinemachineTargetGroup _target_group;
        #endregion
        
        [Tooltip("Used to store direction of player - could use gameobject.transform?")]
        [SerializeField] private  Transform _root_for_shot_raycast;
        [Tooltip("Stores input values(0-1) for walking(movement) input")]
        private Vector2 _input_vector;
        [Tooltip("Stores player speed - used to move character controller")]
        [SerializeField] private Vector3 _Move = new Vector3(0,0,0);
        private float _strong_attack_time_elapsed = 0.0f;
        #region Lock on variables
            public  List<GameObject> enemies_lock_on = new List<GameObject>();
            private int _enemies_counter = -1;
            private GameObject _closest_enemy;
        #endregion
        #region Interact with objects variables
            [Header("Interact with objects variables")]
            private  GameObject _object_to_interact;
            private bool _text_added = false;
            // [Tooltip("Distance to interactable object that within player must be to interact with given object - not using right now changed from raycast to triggers")]
            //[SerializeField] private float _distance_to_interact = 0.9f;

            private bool _in_area_to_interact_chest = false;
            private bool _in_area_to_interact_bush = false;
            private bool _in_area_to_interact_door = false;
            private bool _in_area_to_interact_dropped_items = false;

            public List<Collider> trigger_colliders = new List<Collider>();
            [Tooltip("Collider with which player will interact")]
            [SerializeField] private Collider _collider_to_interact;
            private Collider _previous_collider_to_interact;
        #endregion

        private void Start(){
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _main_camera = Camera.main.transform;

            _handle_attacks = GetComponent<Player_attack_handler>();
            _player_info = GetComponent<Player_info>();
            _player_statistics =  _player_info.player_stats;
            _player_inventory = GetComponent<Player_inventory_info.Player_inventory>();
            _input_handler = GetComponent<Input_handler>();

            _player_speed = 1;
            _timer_between_landing_next_jump = _time_between_jumps;

            Player_info.Player_death +=  Disable_movement;
            //read on this shit, layermask from raycast return number but from variable set in inspector UnityEngine.layermask
            _environment_layer = LayerMask.NameToLayer("Environment");
        }
        //Physics should be done here - chagne TIme.deltaTime for Time.FixeddeltaTime
        private void FixedUpdate() {
            //camera shakes really badly
            // if(!_Active_action && !_player_inventory.inventory_open)
            // Move_player();
        }
        void Update(){
            _input_handler.Check_flags();
            if(_player_info.locked_on_enemy && _closest_enemy != null){
                Handle_state_while_lock_on_enemy();
            }
            Gravity();
            Ground_check();
            Ceiling_check();
            if(!_animator.GetBool("Active_animation")){
                Equip_items();
                if(!_player_inventory.inventory_open){
                    Lock_on_enemy();
                    Jump();
                    Crouch();
                    Move_player();
                    if(!_player_info.locked_on_enemy)
                        Rotate_player();
                    Parry();
                    Block();
                    Attack();
                    Interact();
                }
                Handle_jump_on_top_of_enemy();
                Move_controller();
            }
        }
        private void LateUpdate() {
            Set_input_flags_false();
        }
        private void  Equip_items(){
            if(_input_handler.inventory_flag && player_grounded)
                _player_inventory.Handle_inventory();
            if(_input_handler.inventory_close_inv_flag)
                _object_inventory.SetActive(false);
            if(_input_handler.left_weapon_flag)
                _player_inventory.Check_left_weapon();
            if(_input_handler.right_weapon_flag)
                _player_inventory.Check_right_weapon();
            if(_input_handler.first_potion_flag)
                _player_inventory.Use_potion_from_quick_slot(0);
            if(_input_handler.second_potion_flag)
                _player_inventory.Use_potion_from_quick_slot(1);
            if(_input_handler.third_potion_flag)
                _player_inventory.Use_potion_from_quick_slot(2);
            if(_input_handler.fourth_potion_flag)
                _player_inventory.Use_potion_from_quick_slot(2);
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
                _door_side = -1;
            }
            else if(other.CompareTag("Door_push")){
                _door_side = 1;
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
                    //Debug.Log("in");
                    if(!trigger_colliders.Contains(other)){
                        if(other.TryGetComponent(out Bush_contents bush_content)){
                            if(bush_content.amount_of_berry == 0)
                                return;
                        }
                        trigger_colliders.Add(other);
                    }                   
                }
                else{
                    if(_collider_to_interact == other){
                       // Debug.Log("remove");
                        _collider_to_interact = null;
                        _previous_collider_to_interact = null;
                    }
                    if(trigger_colliders.Contains(other)){
                        trigger_colliders.Remove(other);
                        _in_area_to_interact_chest = false;
                        _in_area_to_interact_door = false;
                        _in_area_to_interact_dropped_items = false;
                        _in_area_to_interact_bush = false;
                        //Debug.Log("LOL");
                        _interact_pop_up.Reset_interact_pop_up();
                    }
                }
            }
            float distance = Mathf.Infinity;
            foreach(var interact_collider in trigger_colliders){
                //Debug.Log("interact collider: " + interact_collider);
                if(Vector3.Distance(gameObject.transform.position,interact_collider.gameObject.transform.position) < distance){
                    distance = Vector3.Distance(gameObject.transform.position,interact_collider.gameObject.transform.position);
                    _collider_to_interact = interact_collider;
                    
                        
                    
                } 
            }
            if(_interact_pop_up.gameObject.activeSelf && _collider_to_interact != _previous_collider_to_interact){
                       // Debug.Log("change collider to interact" + _collider_to_interact + " " + _previous_collider_to_interact);
                        _interact_pop_up.Reset_interact_pop_up();
                    }
            if(_collider_to_interact == null || _collider_to_interact == _previous_collider_to_interact){
                //Debug.Log("Dont repeat actions");
                return ;
            }
                //Debug.Log(_collider_to_interact);
            if(!_interact_pop_up.gameObject.activeSelf)
                _interact_pop_up.gameObject.SetActive(true);
            if(_collider_to_interact.GetComponent<Item_dropped>()){
               // Debug.Log("Dropped item in area");
                _in_area_to_interact_dropped_items = true;
                _interact_pop_up.Set_interact_pop_up(_collider_to_interact.GetComponent<Item_dropped>().interactable_text);        
            }
            else if(_collider_to_interact.gameObject.tag == "Chest"){
                _in_area_to_interact_chest = true;
               _interact_pop_up.Set_interact_pop_up(_collider_to_interact.GetComponent<Interactable_objects>().interactable_text);
            }
            else if(_collider_to_interact.gameObject.tag == "Bush"){
                if(_collider_to_interact.GetComponent<Bush_contents>().amount_of_berry == 0){
                    _interact_pop_up.gameObject.SetActive(false);
                    return ;
                }
                _in_area_to_interact_bush = true;
                _interact_pop_up.Set_interact_pop_up(_collider_to_interact.GetComponent<Bush_contents>().interactable_text);
            }
            else if(_collider_to_interact.gameObject.tag == "Door"){
                //Debug.Log("Opening Door");
                // Door_touched = true;
                _in_area_to_interact_door = true;
                _interact_pop_up.Set_interact_pop_up(_collider_to_interact.GetComponent<Interactable_objects>().interactable_text);
            }
            _previous_collider_to_interact = _collider_to_interact;
        }
        private void OnTriggerExit(Collider other){
            //_trigger_colliders.RemoveAt(0);
            if(other.CompareTag("Trap")){
                Debug.Log("Trap deactivated");
                Trap_active = false;
            }
            if(other.GetComponent<Item_dropped>() || other.gameObject.tag == "Chest" || other.gameObject.tag == "Door" || other.gameObject.tag == "Bush"){
                if(trigger_colliders.Contains(other))
                    trigger_colliders.Remove(other);
                if(trigger_colliders.Count == 0){
                    _collider_to_interact = null;
                    _previous_collider_to_interact = null;
                }
                    
                _in_area_to_interact_chest = false;
                _in_area_to_interact_door = false;
                _in_area_to_interact_dropped_items = false;
                //Debug.Log("Item out of reach");
                _interact_pop_up.Reset_interact_pop_up();
            }
        }
        private void Interact(){
            if(_input_handler.interact_flag && trigger_colliders.Count > 0){
                if(_in_area_to_interact_dropped_items){
                    _inventories.Add_item_to_player_inv_check_for_same_object(_collider_to_interact.GetComponent<Item_dropped>().item_dropped,_collider_to_interact.GetComponent<Item_dropped>().amount_of_dropped_items);
                    
                    if(_inventories.added_all_items){
                        _inventories.Show_items_add_rem_from_inv_window_pop_up(true,_collider_to_interact.GetComponent<Item_dropped>().item_dropped,_collider_to_interact.GetComponent<Item_dropped>().amount_of_dropped_items);
                        Destroy(_collider_to_interact.gameObject);
                        if(trigger_colliders.Contains(_collider_to_interact))
                            trigger_colliders.Remove(_collider_to_interact);
                        if(trigger_colliders.Count == 0){
                            _collider_to_interact = null;
                            _previous_collider_to_interact = null;
                        }
                            
                        _in_area_to_interact_dropped_items = false;
                        _interact_pop_up.Reset_interact_pop_up();
                    }
                    else{
                        _inventories.Show_items_add_rem_from_inv_window_pop_up(true,_collider_to_interact.GetComponent<Item_dropped>().item_dropped,dropped_item_new_amount);
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
                    
                    _inventories.object_inv_to_show = _object_to_interact.GetComponent<Interactable_objects>();
                    _inventories.Get_object_inv_slots().Update_inventory_object_UI(_inventories.object_inv_to_show);
                    _inventories.Assign_weapons_amount_to_slots();
                    _player_inventory.Handle_inventory();
                }
                else if(_in_area_to_interact_bush){
                    _animator.CrossFade("Gathering",0);
                   _inventories.Add_item_to_player_inv_check_for_same_object(_collider_to_interact.GetComponent<Bush_contents>().berry_in_bush,_collider_to_interact.GetComponent<Bush_contents>().amount_of_berry); 
                    if(_inventories.added_all_items){
                        _inventories.Show_items_add_rem_from_inv_window_pop_up(true,_collider_to_interact.GetComponent<Bush_contents>().berry_in_bush,_collider_to_interact.GetComponent<Bush_contents>().amount_of_berry);
                        _collider_to_interact.GetComponent<Bush_contents>().amount_of_berry = 0;
                        _interact_pop_up.Reset_interact_pop_up();
                        if(trigger_colliders.Contains(_collider_to_interact))
                            trigger_colliders.Remove(_collider_to_interact);
                        if(trigger_colliders.Count == 0){
                            _collider_to_interact = null;
                            _previous_collider_to_interact = null;
                        }
                    }
                    else{
                        _inventories.Show_items_add_rem_from_inv_window_pop_up(true,_collider_to_interact.GetComponent<Bush_contents>().berry_in_bush,dropped_item_new_amount);
                        _collider_to_interact.GetComponent<Bush_contents>().amount_of_berry = dropped_item_new_amount;
                    }
                }
                else if(_in_area_to_interact_door){
                    _collider_to_interact.GetComponent<Rigidbody>().AddForce(_door_side*_root_for_shot_raycast.forward * _touch_force * _player_speed,ForceMode.Acceleration);
                }
            }
                // old version with raycasts
                /*
            // use raycast or ontrigger enter
            if(Physics.Raycast(_root_for_shot_raycast.position, _root_for_shot_raycast.forward, out _Hit, _distance_to_interact,_interact_layer)){
                Debug.DrawRay(_root_for_shot_raycast.position, _root_for_shot_raycast.forward,Color.blue,_distance_to_interact);
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
                        
                        _inventories.object_inv_to_show = _object_to_interact.GetComponent<Interactable_objects>();
                        _inventories.Assign_weapons_amount_to_slots();
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
                        _Hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(_door_side*_root_for_shot_raycast.forward * _touch_force * _Player_speed,ForceMode.Acceleration);
                        
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
        private IEnumerator Coloring_object(){
            _object_to_interact.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(4.5f);
            _object_to_interact.GetComponent<Renderer>().material.color = Color.white;
        }
        private void Attack(){
                if(_input_handler.attack_light_flag){
                    //Debug.Log("Light attack button pressed");
                    _handle_attacks.Handle_light_attack(_input_handler.sprint_flag && _input_vector != Vector2.zero && !_animator.GetBool("Combat_movement"));
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
                else if(_input_handler.attack_air_flag){
                    _handle_attacks.Handle_air_special_attack();
                }
        }
        private void Block(){
            if(_input_handler.block_attacks_flag)
                _handle_attacks.Start_blocking_attacks();
            else
                _handle_attacks.Stop_blocking_attacks();
        }
        private void Parry(){
            if(_input_handler.parry_attack_flag){
                _handle_attacks.Parry_attack();
            }
        }
        private void Move_player(){
            _input_vector = _input_handler.walk_input;
            if(_player_info.locked_on_enemy && ! _animator.GetBool("Active_animation") ){
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
                //if(player_grounded){
                    //_controller.velocity to work properly requires only one _controller.move - tried to combine gravity,jump and mve into one but 
                    _player_speed = new Vector3 (_controller.velocity.x,0.0f,_controller.velocity.z).magnitude;    
                    if(player_crouching){
                        _player_target_speed = _crouch_speed;
                    }
                    else if(_input_handler.sprint_flag && _player_statistics.Current_stamina >0){
                        _player_statistics.Take_stamina(_sprinting_stamina_cost * Time.deltaTime);
                        _player_target_speed = _sprint_speed;
                        _animator.SetFloat("Speed_percent",_player_speed, 0.1f,Time.deltaTime); // last 2 arguments to smooth transision - now useless but great when i change 0.99f to player speed - fix the bug
                    }
                    else{
                        _player_target_speed = _walk_speed;
                        _animator.SetFloat("Speed_percent",_player_speed, 0.1f,Time.deltaTime);
                    } 
                    if(_input_vector == Vector2.zero){
                        _player_target_speed = 0f;  // doesn't change _controller.Move becasue INput vector is 0 but allows to make depanded on speed functions etc while player is in  one place
                        _animator.SetFloat("Speed_percent",-1f);//, 0.1f,Time.deltaTime);
                    }
                    if(_player_speed < _player_target_speed - 0.3f || _player_speed > _player_target_speed + 0.3f ){
                        // lERP TYPE of Exponential ease toward a target  because input and output are the same so no time.delta time with linear there could/should be time.delta time included
                        _player_speed = Mathf.Lerp(_player_speed,_player_target_speed,_player_speed_change_rate);
                        _player_speed = Mathf.Round(_player_speed * 1000f) / 1000f;
                    }
                    else{
                        _player_speed = _player_target_speed;
                    }
                    _Move = new Vector3(_input_vector.x,_Move.y,_input_vector.y);
                    _Move  = Vector3.ProjectOnPlane(_main_camera.forward, Vector3.up).normalized * _Move.z + Vector3.ProjectOnPlane(_main_camera.right, Vector3.up).normalized * _Move.x +  new Vector3(0,1,0) * _Move.y ;
                //}
            }
        }
        private void Gravity(){
            _Move.y  += _gravity_force * Time.deltaTime;
            if (player_grounded &&  _Move.y < 0){
                _Move.y = 0f;
            }
        }
        private void Jump(){
            if(player_grounded && !player_crouching){
                if(_timer_between_landing_next_jump >= 0.0f)
                    _timer_between_landing_next_jump -= Time.deltaTime;
                if (_input_handler.jump_flag && _timer_between_landing_next_jump <=0.0f && _player_statistics.Current_stamina > _jump_stamina_cost){
                    _player_statistics.Take_stamina(_jump_stamina_cost);
                    _Move.y = Mathf.Sqrt(_jump_height * -2.0f * _gravity_force);
                }
            }
            else{
                _timer_between_landing_next_jump = _time_between_jumps;
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
        private void Move_controller(){
            if(_animator.GetBool("Movement_driven_by_animation"))
                return ;
            Vector3 move_player;
            if(player_grounded)
                move_player = new Vector3(_Move.x * _player_speed,_Move.y,_Move.z * _player_speed);
            else
                move_player = new Vector3(_Move.x * _player_speed/2,_Move.y,_Move.z * _player_speed/2);  
            _controller.Move(move_player * Time.deltaTime);
        }        
        private void Ground_check(){
            // player_grounded = _controller.isGrounded; // buggy as fuck
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y - _grounded_help,transform.position.z );
            player_grounded = Physics.CheckSphere(Sphere_position,_grounded_check_radious,_ground_ceiling_layer,QueryTriggerInteraction.Ignore);
            //Debug.Log("Grounded: " + player_grounded);
        }
        private void Handle_jump_on_top_of_enemy(){
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y + 2 * _grounded_help,transform.position.z);
            if(Physics.CheckSphere(Sphere_position,_grounded_check_radious,128,QueryTriggerInteraction.Ignore)){
                _Move.z = -4;
                _player_speed = 1;
            }
        }   
        private void Ceiling_check(){
            Vector3 Sphere_position = new Vector3(transform.position.x,transform.position.y - _ceiling_help ,transform.position.z );
            player_hit_ceiling = Physics.CheckSphere(Sphere_position,_grounded_check_radious,_ground_ceiling_layer,QueryTriggerInteraction.Ignore);
            if(!player_grounded && player_hit_ceiling){
                if(_Move.y > 0)
                    _Move.y = -0.4f;
            }
        }
        private void Rotate_player(){
            if(_input_vector != Vector2.zero){
                float _target_angle;  
                _target_angle = Mathf.Atan2(_input_vector.x,_input_vector.y) * Mathf.Rad2Deg + _main_camera.eulerAngles.y;
                Quaternion Rotation = Quaternion.Euler(0f,_target_angle,0f);
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
                if(_closest_enemy != null){
                    _target_group.AddMember(_closest_enemy.transform,1,2);
                    _player_info.locked_on_enemy = true;
                    _lock_on_camera.SetActive(true);
                    _Move = Vector3.zero;
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
                if(_input_handler.walk_input.x < 0){
                    _animator.CrossFade("Base Layer.Dash_left",0f,0);
                }
                else if(_input_handler.walk_input.x > 0){
                    _animator.CrossFade("Base Layer.Dash_right",0f,0);
                }   
                else
                    return;
                _player_statistics.Take_stamina(_dash_stamina_cost);
            }
        }
        public bool Is_enemy_to_lock_on_visible(GameObject potential_enemy_to_lock_on){
            RaycastHit _hit;
            if(Physics.Linecast(_root_for_shot_raycast.position,potential_enemy_to_lock_on.GetComponent<Enemy_manager>().raycast_root_transform.position, out _hit)){
                Debug.DrawLine(_root_for_shot_raycast.position,potential_enemy_to_lock_on.GetComponent<Enemy_manager>().raycast_root_transform.position);
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
                
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _grounded_help, transform.position.z), _grounded_check_radious);
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _ceiling_help, transform.position.z), _grounded_check_radious);
        }
        private void Set_input_flags_false(){
            #region  Player action map 
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
                _input_handler.block_attacks_flag = false;
                _input_handler.parry_attack_flag = false;
                _input_handler.attack_light_flag = false;
                _input_handler.attack_strong_performed_flag = false;
                _input_handler.attack_special_flag = false;
                _input_handler.attack_air_flag = false;
                _input_handler.attack_combo_flag = false;
                _input_handler.inventory_flag = false;
                _input_handler.attack_strong_canceled_flag = false;
                _input_handler.lock_on_flag = false;
                _input_handler.switch_flag = false;
                _input_handler.dash_flag = false;
            #endregion
            #region  Inventory action map
                _input_handler.inventory_close_inv_flag = false;
                _input_handler.mouse_right_pressed_inv_flag = false;
                _input_handler.mouse_left_pressed_inv_flag = false;
                _input_handler.transfer_items_inv_flag = false;
                _input_handler.drop_items_inv_flag = false;
                _input_handler.use_item_inv_flag = false;
                _input_handler.confirmed_action_inv_flag = false;
                _input_handler.left_weapon_inv_flag = false;
                _input_handler.right_weapn_inv_flag = false;
                _input_handler.first_potion_inv_flag = false;
                _input_handler.second_potion_inv_flag = false;
                _input_handler.third_potion_inv_flag = false;
                _input_handler.fourth_potion_inv_flag = false;
            #endregion
        }
        private void Disable_movement(){
            this.enabled = false;
        }
        public void Stop_player(){
            _Move = Vector3.zero;
        }
    }
}