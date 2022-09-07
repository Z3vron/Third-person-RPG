using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Inventories : MonoBehaviour{
    public Interactable_objects object_inv_to_show;
    public bool added_all_items;
    public int remaining_item_amount;


    [SerializeField]  private  GameObject _moving_icon;
    [SerializeField] private Input_handler _input_handler;
    [SerializeField] private Player_inventory_info.Player_inventory _player_inventory;
    [SerializeField] private GameObject _instance_item_info_prefab_model;  
    [SerializeField] private GameObject _item_option_menu_prefab;
    [SerializeField] private GameObject _item_creation_menu_prefab;
    [SerializeField] private Rigidbody _item_dropped_prefab;
    [SerializeField] private Transform _root_for_drop_items;
    [SerializeField] private float _dropp_items_force = 200;
    [SerializeField] private Object_inventory_slots _obj_inv_slots;
    [SerializeField] private Object_inventory_slots _creation_inv_slots;
    [SerializeField] private Creation_inv _creation_inv;
    [SerializeField] private Inventory_slots _player_inventory_slots;
    [SerializeField] private GameObject _crafting_menu;
    [SerializeField] private GameObject _player_inventory_menu;
    [SerializeField] private GameObject _inventory_buttons;
    [SerializeField] private GameObject _input_field_prefab;
    
    
    
    private int _slot_number_item_info;
    private float _time_to_show_item_info_elapsed = 0.0f;
    private bool _start_time = false;
    private bool _weapon_on_mouse;
    private bool _cursor_player_inv;
  
    private Slot _slot_selected;
    private PointerEventData _help;
    bool _help2 = false;
    bool _last_pressed_right;
    private GraphicRaycaster _graphicRaycaster_inv_slots;
    private GameObject _instance_item_info;
    private GameObject _instance_item_option_menu;
    private GameObject _instance_item_creation_menu;
    private GameObject _instance_item_creation_amount;
    private TMPro.TMP_Dropdown Items_dropdown_options;
    private GameObject _dropdown_options_title;
    private bool _pressed_outside_dropdown = true;
    private bool _mouse_on_item = false;
    // create list of slots - code will be more clean - extra list but current syntax is just awful - done but still could think about changing system - now i manually add item to player/obj list end to list of slots separate
    // variables used in operations on inventories taking values from object or player inv
    // I store stack amount in slot scriptableobject could change to store in in weapon??? or in the list - still not sure  about that
    private Weapon_info.Weapon _weapon; 
    private Item_info.Item _item_selected;
    int _occupied_slots;
    List <GameObject>  _inv_slots_images = new List<GameObject>();
    List <Slot> _inv_slots = new List<Slot>();
    bool _item_derivative;

    // slots item has higher priority player inv lists update base on correspondig slots actually not i chagned it, so operations are done on lists in inventory and slots items are asign to respondned lists position items - not sure which is better
    // nope went with original :)
    //check variables and their values at hover mouse over dropslot - specially choose functions etc 
    private void Start() {
        _help = new PointerEventData(null);
        _graphicRaycaster_inv_slots = GetComponent<GraphicRaycaster>();
    }
    // watchout for choose which inv function - called on pointer enter - changing some values - be carefull with using 3 variables above their values isn't changing  by add weapons functions but they change originall values 
    //do automatic stack of same objects whilk transfering items to the object/player inventory, handle situation when transfer item to capped inv - done mouse gives more control - no stack and transfer do auto stack if possible, if inv full than it transfer as much as possible and rest stay at original inv
    public void Check_hits_inv(){
        _item_derivative = false;   
        _help.position = new Vector3( _input_handler.mouse_position.x,_input_handler.mouse_position.y,0);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster_inv_slots.Raycast(_help, results);

        foreach(var hit in results){
            // var item_slot = hit.gameObject.GetComponent<Drop_slot>();
            // //Debug.Log("hitted something");
            // if(item_slot){i
            if(hit.gameObject.TryGetComponent(out Drop_slot item_slot)){
                //started at player inv
                if(_cursor_player_inv){
                    if(item_slot.slot.isRight && !item_slot.slot.in_inventory && !item_slot.slot.in_obj_inv && _slot_selected.item is Weapon_info.Weapon ){
                        Change_right_weapon_from_inv(_slot_selected);     
                    }
                    else if(!item_slot.slot.isRight && !item_slot.slot.in_inventory && !item_slot.slot.in_obj_inv && _slot_selected.item is Weapon_info.Weapon ){
                        Change_left_weapon_from_inv(_slot_selected);
                    }
                    else if (item_slot.slot.in_inventory && item_slot.slot != _slot_selected ){
                        //must be a better way to do this
                        if(_slot_selected.item is Materials || _slot_selected.item is Potions || _slot_selected.item is Weapon_info.Weapon || item_slot.slot.item is Materials || item_slot.slot.item is Potions || item_slot.slot.item is Weapon_info.Weapon){
                            _item_derivative = true;
                        }
                        if(_slot_selected.item is Materials && item_slot.slot.item is Materials || _slot_selected.item is Potions && item_slot.slot.item is Potions || _slot_selected.item is Weapon_info.Weapon && item_slot.slot.item is Weapon_info.Weapon  || (_slot_selected.item is Item_info.Item && item_slot.slot.item is Item_info.Item && !_item_derivative)){
                            //Debug.Log("same class not pure  items");
                            Change_items_in_inv(item_slot.slot,_slot_selected,true);
                            _mouse_on_item = true;
                        }
                    }
                    else if(item_slot.slot.in_obj_inv){
                        Move_items_between_player_obj_inv(item_slot.slot,_slot_selected);
                        Choose_which_inventory(false);
                        Choose_which_inventory_type_slot(item_slot.slot);
                        _mouse_on_item = true;
                        Start_timer(item_slot.slot.slot_number);
                    }
                }
                //started at object inv
                else if(!_cursor_player_inv){
                    if(item_slot.slot.isRight && !item_slot.slot.in_inventory && !item_slot.slot.in_obj_inv  && _item_selected is Weapon_info.Weapon ){
                        Change_right_weapon_from_inv(_slot_selected);  
                    }
                    else if(!item_slot.slot.isRight && !item_slot.slot.in_inventory && !item_slot.slot.in_obj_inv && _item_selected is Weapon_info.Weapon){
                        Change_left_weapon_from_inv(_slot_selected);
                    }
                    else if(item_slot.slot.in_inventory){
                        if(_slot_selected.item is Materials || _slot_selected.item is Potions || _slot_selected.item is Weapon_info.Weapon || item_slot.slot.item is Materials || item_slot.slot.item is Potions || item_slot.slot.item is Weapon_info.Weapon)
                           _item_derivative = true;
                        if(_slot_selected.item is Materials && (item_slot.slot.item is Materials || item_slot.slot.item is null) || _slot_selected.item is Potions && (item_slot.slot.item is Potions || item_slot.slot.item is null)   || _slot_selected.item is Weapon_info.Weapon && (item_slot.slot.item is Weapon_info.Weapon || item_slot.slot.item is null)  || (_slot_selected.item is Item_info.Item && (item_slot.slot.item is Item_info.Item || item_slot.slot.item is null) && !_item_derivative)){
                            Move_items_between_player_obj_inv(item_slot.slot,_slot_selected);
                            Choose_which_inventory(true);
                            Choose_which_inventory_type_slot(item_slot.slot);
                            _mouse_on_item = true;
                            Start_timer(item_slot.slot.slot_number);
                        }
                    }
                    else if(item_slot.slot.in_obj_inv && item_slot.slot != _slot_selected){
                        Change_items_in_inv(item_slot.slot,_slot_selected,false);
                        _mouse_on_item = true;
                    }
                }
            }
        }
    }
    public void Move_items_between_player_obj_inv(Slot target_slot,Slot start_slot){
        if(!_cursor_player_inv){
            if(start_slot.item is Weapon_info.Weapon){
                _occupied_slots = _player_inventory.inventory_weapons_slots.Count;
            }
            else if(start_slot.item is Materials){
                _occupied_slots = _player_inventory.inventory_materials_slots.Count;
            }
            else if(start_slot.item is Potions){
                _occupied_slots = _player_inventory.inventory_potions_slots.Count;
            } 
            else if(start_slot.item is Item_info.Item){
                _occupied_slots = _player_inventory.inventory_items_slots.Count;
            }
        }
        else{
            _occupied_slots = object_inv_to_show.items_in_object.Count;
        }

        int max_slots_amount;
        if(_cursor_player_inv)
            max_slots_amount = _player_inventory.amount_of_items_slots;
        else
            max_slots_amount = object_inv_to_show.amount_of_item_slots;
       
        if(_occupied_slots >  target_slot.slot_number && start_slot.item == target_slot.item && target_slot.item.max_stack_amount > 1  && target_slot.stack_amount < target_slot.max_stack_amount && target_slot.stack_amount <start_slot.item.max_stack_amount  && start_slot.stack_amount <= target_slot.max_stack_amount) {
            Debug.Log("stack items inv");
            if((target_slot.stack_amount + start_slot.stack_amount) <= target_slot.max_stack_amount && (target_slot.stack_amount + start_slot.stack_amount) <= target_slot.item.max_stack_amount){
                target_slot.stack_amount += start_slot.stack_amount;
                start_slot.stack_amount = 0;
                if(_cursor_player_inv)
                    _player_inventory.Remove_item_from_player_inv(start_slot); 
                else
                    object_inv_to_show.Remove_item_from_object(start_slot.slot_number);
                Move_slots_to_left_inv(start_slot,_player_inventory.amount_of_items_slots);
            }
            else if ((target_slot.stack_amount + start_slot.stack_amount) <= target_slot.max_stack_amount && (target_slot.stack_amount + start_slot.stack_amount) > target_slot.item.max_stack_amount){
                start_slot.stack_amount -= (target_slot.item.max_stack_amount - target_slot.stack_amount);
                target_slot.stack_amount = target_slot.item.max_stack_amount;
            }
            else if ((target_slot.stack_amount + start_slot.stack_amount) > target_slot.max_stack_amount && (target_slot.stack_amount + start_slot.stack_amount) <= target_slot.item.max_stack_amount){
                start_slot.stack_amount -= (target_slot.max_stack_amount - target_slot.stack_amount);
                target_slot.stack_amount = target_slot.max_stack_amount;
            }
            else if ((target_slot.stack_amount + start_slot.stack_amount) > target_slot.max_stack_amount && (target_slot.stack_amount + start_slot.stack_amount) > target_slot.item.max_stack_amount){
                if(target_slot.item.max_stack_amount < target_slot.max_stack_amount){
                    start_slot.stack_amount -= (target_slot.item.max_stack_amount - target_slot.stack_amount);
                    target_slot.stack_amount = target_slot.item.max_stack_amount;
                }
                else{
                    start_slot.stack_amount -= (target_slot.max_stack_amount - target_slot.stack_amount);
                    target_slot.stack_amount = target_slot.max_stack_amount;
                }
            }
        }
        else if (_occupied_slots < max_slots_amount){ 
            if(_cursor_player_inv){
                object_inv_to_show.Add_item_to_object(start_slot.item,start_slot.stack_amount);
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount += start_slot.stack_amount;
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item = start_slot.item;
                _player_inventory.Remove_item_from_player_inv(start_slot);
                start_slot.stack_amount = 0;
                start_slot.item = null;
                Move_slots_to_left_inv(start_slot,max_slots_amount);
            }
            else{
                Add_item_to_player_inv_last_slot(start_slot.item,start_slot.stack_amount);
                if(added_all_items){
                    object_inv_to_show.Remove_item_from_object(_slot_selected.slot_number);
                    Move_slots_to_left_inv(start_slot,max_slots_amount);
                }
            }                          
        }
    }
    public void Change_items_in_inv(Slot target_slot,Slot start_slot,bool player_inv){
        if(_occupied_slots > target_slot.slot_number){
            //with objects(scriptableobjects) created with creating systyem on the run line below returns false for the same objects because they are copy of the original scriptable objects but they are not the same scriptable object
            //Debug.Log(target_slot.item + " " + start_slot.item + (target_slot.item == start_slot.item));
            //stack items in inventory
            if(start_slot.item.max_stack_amount > 1 && target_slot.stack_amount < target_slot.max_stack_amount && target_slot.stack_amount < start_slot.item.max_stack_amount && start_slot.stack_amount < start_slot.item.max_stack_amount  ){
                if(target_slot.item.item_id == start_slot.item.item_id){
                    //Debug.Log("stack items player inv");
                    if((target_slot.stack_amount + start_slot.stack_amount) <= target_slot.max_stack_amount ){
                        if((target_slot.stack_amount + start_slot.stack_amount) <= target_slot.item.max_stack_amount){
                            target_slot.stack_amount += start_slot.stack_amount;
                            start_slot.stack_amount = 0; 
                            if(player_inv){
                                _player_inventory.Remove_item_from_player_inv(start_slot);
                                Move_slots_to_left_inv(start_slot,_player_inventory.amount_of_items_slots);
                            }
                            else{
                                object_inv_to_show.Remove_item_from_object(start_slot.slot_number);
                                Move_slots_to_left_inv(start_slot,object_inv_to_show.amount_of_item_slots);
                            }
                        }
                        else{
                            start_slot.stack_amount -= (target_slot.item.max_stack_amount - target_slot.stack_amount); 
                            target_slot.stack_amount = target_slot.item.max_stack_amount;
                        }
                    }
                    else{
                        if(target_slot.max_stack_amount <= target_slot.item.max_stack_amount){
                            start_slot.stack_amount -= (target_slot.max_stack_amount - target_slot.stack_amount);
                            target_slot.stack_amount = target_slot.max_stack_amount; 
                        }
                        else{
                            start_slot.stack_amount -= (target_slot.item.max_stack_amount - target_slot.stack_amount); 
                            target_slot.stack_amount = target_slot.item.max_stack_amount;
                        }
                    }
                }   
            }
            //chagne two items in inventory
            else{
                //Debug.Log("Replace items in inventory");
                Item_info.Item temp_item;
                int temp_stack;
                if(player_inv){
                    if(start_slot.item is Weapon_info.Weapon){
                        temp_item = _player_inventory.inventory_weapons_slots[target_slot.slot_number];
                        _player_inventory.inventory_weapons_slots[target_slot.slot_number] = _player_inventory.inventory_weapons_slots[start_slot.slot_number];
                        _player_inventory.inventory_weapons_slots[start_slot.slot_number] = (Weapon_info.Weapon) temp_item;
                    }
                    else if(start_slot.item is Materials){
                        temp_item = _player_inventory.inventory_materials_slots[target_slot.slot_number];
                        _player_inventory.inventory_materials_slots[target_slot.slot_number] = _player_inventory.inventory_materials_slots[start_slot.slot_number];
                        _player_inventory.inventory_materials_slots[start_slot.slot_number] = (Materials)temp_item;
                    }
                    else if(start_slot.item is Potions){
                        temp_item = _player_inventory.inventory_potions_slots[target_slot.slot_number];
                        _player_inventory.inventory_potions_slots[target_slot.slot_number] = _player_inventory.inventory_potions_slots[start_slot.slot_number];
                        _player_inventory.inventory_potions_slots[start_slot.slot_number] = (Potions)temp_item;
                    }
                    else if(start_slot.item is Item_info.Item){
                        temp_item = _player_inventory.inventory_items_slots[target_slot.slot_number];
                        _player_inventory.inventory_items_slots[target_slot.slot_number] = _player_inventory.inventory_items_slots[start_slot.slot_number];
                        _player_inventory.inventory_items_slots[start_slot.slot_number] = temp_item;
                    }
                }
                else{
                    temp_item = object_inv_to_show.items_in_object[target_slot.slot_number];
                    object_inv_to_show.items_in_object[target_slot.slot_number] = object_inv_to_show.items_in_object[start_slot.slot_number];
                    object_inv_to_show.items_in_object[start_slot.slot_number] = temp_item;
                }
                //change slot_stack  
                temp_stack = target_slot.stack_amount;
                temp_item = target_slot.item;
                target_slot.stack_amount = start_slot.stack_amount;
                target_slot.item = start_slot.item;
                start_slot.stack_amount = temp_stack;
                start_slot.item = temp_item;
            }
        }
        else{
            Debug.Log("No item in the target slot");
        }
    }
    public void Move_slots_to_left_inv(Slot slot_to_start_move,int max_number_of_items){
        // for(int i=0;i<max_number_of_items;i++){
        //     Debug.Log(i +" " + _inv_slots[i].item + " " + _inv_slots[i].stack_amount);
        // }
        for(int i = slot_to_start_move.slot_number;i < max_number_of_items-1;i++){
            _inv_slots[i].item = _inv_slots[i+1].item;
            _inv_slots[i].stack_amount = _inv_slots[i+1].stack_amount;
        }
        _inv_slots[max_number_of_items-1].stack_amount = 0;
        _inv_slots[max_number_of_items-1].item = null;
    }
    public void Move_item_inv(){
        if(_occupied_slots > _slot_selected.slot_number-1){
            _weapon_on_mouse = true;
            _moving_icon.GetComponentInChildren<Image>().sprite = _slot_selected.item.Item_icon;
            _moving_icon.GetComponentInChildren<Image>().enabled = true;
            _moving_icon.GetComponent<RectTransform>().position= new Vector3( _input_handler.mouse_position.x,_input_handler.mouse_position.y,0);
        }
    }
    public void Drop_item_to_slot_inv(){
        if(_occupied_slots > _slot_selected.slot_number-1){
            // Debug.Log("mouse up");
            _moving_icon.GetComponentInChildren<Image>().sprite = null;
            _moving_icon.GetComponentInChildren<Image>().enabled = false;
            Check_hits_inv();
        }
    }
    public void Unequip_weapon_slot(bool isRight){
        //if(_player_inventory.inventory_weapons_slots.Count < _player_inventory.amount_of_items_slots){
            // _player_inventory_slots.inventory_weapons_slots[_player_inventory.inventory_weapons_slots.Count].stack_amount = 1;

            _player_inventory.Remove_weapon_from_hand_slot(isRight);
        //}
    }
    public void Assign_weapons_amount_to_slots(){
        if(object_inv_to_show !=null){
            //Debug.Log("Assing amount");
            for(int i=0;i<object_inv_to_show.items_in_object.Count;i++){
                _obj_inv_slots.obj_inv_items_slots[i].stack_amount = object_inv_to_show.obj_inv_item_amount_on_slot[i];
                _obj_inv_slots.obj_inv_items_slots[i].item = object_inv_to_show.items_in_object[i];
            }
            for(int i = object_inv_to_show.items_in_object.Count;i < object_inv_to_show.amount_of_item_slots;i++ ){
                _obj_inv_slots.obj_inv_items_slots[i].stack_amount = 0;
                _obj_inv_slots.obj_inv_items_slots[i].item = null;
            }
        }
    } 
    public void Update_inventory_object_UI(){
        if(object_inv_to_show !=null){
            for(int i=0;i<object_inv_to_show.items_in_object.Count ;i++){
                _obj_inv_slots.Add_inventory_item_icon(object_inv_to_show.items_in_object[i],i);
                _obj_inv_slots.Check_item_amount(i,_obj_inv_slots.obj_inv_items_images_slots,_obj_inv_slots.obj_inv_items_slots);
            }
            for(int i=object_inv_to_show.items_in_object.Count;i<object_inv_to_show.amount_of_item_slots;i++){
                _obj_inv_slots.Remove_inventory_item_icon(i,_obj_inv_slots.obj_inv_items_images_slots);
            }
        }
    }
    public void Update_inventory_player_UI(){
        //weapons
        List<Item_info.Item> list_of_items = _player_inventory.inventory_weapons_slots.Cast<Item_info.Item>().ToList();
        Update_UI_inventory_row(list_of_items,_player_inventory_slots.inventory_weapons_images_slots,_player_inventory_slots.inventory_weapons_slots);
        //materials
        list_of_items = _player_inventory.inventory_materials_slots.Cast<Item_info.Item>().ToList();
        Update_UI_inventory_row(list_of_items,_player_inventory_slots.inventory_materials_images_slots,_player_inventory_slots.inventory_materials_slots);
        //potions
        list_of_items = _player_inventory.inventory_potions_slots.Cast<Item_info.Item>().ToList();
        Update_UI_inventory_row(list_of_items,_player_inventory_slots.inventory_potions_images_slots,_player_inventory_slots.inventory_potions_slots);
        //items
        Update_UI_inventory_row(_player_inventory.inventory_items_slots,_player_inventory_slots.inventory_items_images_slots,_player_inventory_slots.inventory_items_slots);
    }
    private void Update_UI_inventory_row(List<Item_info.Item> list_of_items,List<GameObject> list_of_icons,List<Slot> list_of_slots){
        for(int i=0;i<list_of_items.Count;i++){
            _player_inventory_slots.Add_inventory_item_icon(list_of_items[i],i);
            _player_inventory_slots.Check_item_amount(i,list_of_icons,list_of_slots);
        }
        for(int i=list_of_items.Count;i<_player_inventory.amount_of_items_slots;i++){
            _player_inventory_slots.Remove_inventory_item_icon(i,list_of_icons);
        }
    }
    public void Update_creation_inventory_UI(){
        if(_creation_inv.gameObject.activeSelf == true){
            for(int i=0;i<_creation_inv.recipes_for_items.Count;i++){
                _creation_inv_slots.Add_inventory_item_icon(_creation_inv.recipes_for_items[i].item_to_create,i);
            }
            for(int i=_creation_inv.recipes_for_items.Count;i<_creation_inv.amount_of_items_possible_to_create;i++){
                _creation_inv_slots.Remove_inventory_item_icon(i,_creation_inv_slots.obj_inv_items_images_slots);
            }
        }
    }
    private void Update() {  
        _input_handler.Check_flags();
        if(_input_handler.use_item_inv_flag){
            Use_item_from_inv();
        }
        if(_input_handler.transfer_items_inv_flag){
            Transfer_items_to_other_inv();
        }
        if(_input_handler.drop_items_inv_flag){
            Drop_item_from_inventory();
        }
        //Debug.Log("update click  " + _input_handler.mouse_right_pressed_inv_flag );
        if(_input_handler.inventory_close_inv_flag){
            if(_instance_item_info != null)
                Destroy(_instance_item_info);
            if(_instance_item_option_menu != null)
                Destroy(_instance_item_option_menu);
            if(_instance_item_creation_menu != null)
                Destroy(_instance_item_creation_menu);
            if(_instance_item_creation_amount != null)
                Destroy(_instance_item_creation_amount);
            _pressed_outside_dropdown = true;
        }
        //option menu on only right click not left
        if(_input_handler.mouse_right_pressed_inv_flag)
            _last_pressed_right = true;
        if(_input_handler.mouse_left_pressed_inv_flag)
            _last_pressed_right = false;
       
        // when player need to choose one of the option
        // if( _instance_item_option_menu != null  && _input_handler.mouse_left_pressed_inv_flag && !_pressed_outside_dropdown){
        //     Check_if_mouse_click_on_dropdown_options();
        // }
        // //when player need to choose main button to show other option about actions to items
        // if((_instance_item_option_menu != null || _instance_item_creation_menu != null || _instance_item_creation_amount != null)  && _pressed_outside_dropdown && (_input_handler.mouse_left_pressed_inv_flag  || _input_handler.mouse_right_pressed_inv_flag)){
        //     Check_if_mouse_click_on_dropdown_title();
        // }
        //creating given number of items
        if(_instance_item_creation_amount != null && _input_handler.confirmed_action_inv_flag && _instance_item_creation_amount.GetComponent<TMP_InputField>().text != ""){
                int counter = 0;
                //Debug.Log( "Trying to create: " + (_instance_item_creation_amount.GetComponent<TMP_InputField>().text));
                while(true && counter < int.Parse(_instance_item_creation_amount.GetComponent<TMP_InputField>().text)){
                    if(! Create_item(_slot_number_item_info))
                        break;
                    counter++;
                }
                Destroy(_instance_item_creation_amount);
            }
        // if (EventSystem.current.)
        //     Debug.Log("left-click over a GUI element!");

        // if(_help2){
        //     if(Items_dropdown_options.onValueChanged.())
        //     _help2 = false;
        // }
       // Update_inventory_player_data();
        Set_object_inv_items_amount();
        Update_inventory_object_UI();
        Update_inventory_player_UI();
        Update_creation_inventory_UI();
       
         if(_start_time){
            _time_to_show_item_info_elapsed += Time.deltaTime;
            if(_time_to_show_item_info_elapsed > 0.35f){
                Show_item_stats_inv(_slot_number_item_info);
                _start_time = false;
                _time_to_show_item_info_elapsed = 0.0f;
            }
        }  
    }
    private void Transfer_items_to_other_inv(){
        if(_mouse_on_item && _obj_inv_slots.gameObject.activeSelf){
            if(_cursor_player_inv && _occupied_slots > _slot_number_item_info  ){
                //Debug.Log("transfer items from player inv to object inv");
                List<Slot> _inv_slots_backup = _inv_slots;
                Add_item_to_obj_inv_check_for_same_object(_inv_slots[_slot_number_item_info].item,_inv_slots[_slot_number_item_info].stack_amount);
                Choose_which_inventory_type_slot(_inv_slots_backup[_slot_number_item_info]);
                if(added_all_items){
                    _player_inventory.Remove_item_from_player_inv(_slot_selected);
                    Move_slots_to_left_inv(_inv_slots[_slot_number_item_info],object_inv_to_show.amount_of_item_slots);
                }
                else{
                    _inv_slots[_slot_number_item_info].stack_amount = remaining_item_amount;
                }
                Reset_timer();
                _mouse_on_item = true;
            }
            else if (!_cursor_player_inv && _occupied_slots > _slot_number_item_info){
                //Debug.Log("transfer items from obj inv to player inv");
                Add_item_to_player_inv_check_for_same_object(_inv_slots[_slot_number_item_info].item,_inv_slots[_slot_number_item_info].stack_amount);
                Choose_which_inventory_type_slot(_obj_inv_slots.obj_inv_items_slots[_slot_number_item_info]);
                if(added_all_items){
                     object_inv_to_show.Remove_item_from_object(_slot_number_item_info);
                    Move_slots_to_left_inv(_inv_slots[_slot_number_item_info],object_inv_to_show.amount_of_item_slots);
                }
                else{
                    _inv_slots[_slot_number_item_info].stack_amount = remaining_item_amount;
                }
                Reset_timer();
                _mouse_on_item = true;
            }
        }
    }
     public void Change_right_weapon_from_inv(Slot slot){
        if(_weapon_on_mouse){
            _player_inventory.Change_weapon_for_right_hand(slot,_cursor_player_inv);
            _weapon_on_mouse = false;
        }
    }
    public void Change_left_weapon_from_inv(Slot slot){
        //Debug.Log("on left weapon slot - override events? drop not working neither on poniter enter during drag in other");
        if(_weapon_on_mouse){
            _player_inventory.Change_weapon_for_left_hand(slot,_cursor_player_inv);
            _weapon_on_mouse = false;
        }
    }
    public void Choose_which_inventory(bool player_inventory){
        _cursor_player_inv = player_inventory;
    }
    public void Choose_which_inventory_type_slot(Slot slot){
        _slot_selected = slot;
        _item_selected = slot.item;
        //Debug.Log("inv");
        if(_cursor_player_inv){

            if(_item_selected is Weapon_info.Weapon){
                _occupied_slots = _player_inventory.inventory_weapons_slots.Count;
                _inv_slots_images = _player_inventory_slots.inventory_weapons_images_slots;
                _inv_slots = _player_inventory_slots.inventory_weapons_slots;
            }
            else if(_item_selected is Materials){
                _occupied_slots = _player_inventory.inventory_materials_slots.Count;
                _inv_slots_images = _player_inventory_slots.inventory_materials_images_slots;
                _inv_slots = _player_inventory_slots.inventory_materials_slots;
            }
            else if(_item_selected is Potions){
                _occupied_slots = _player_inventory.inventory_potions_slots.Count;
                _inv_slots_images = _player_inventory_slots.inventory_potions_images_slots;
                _inv_slots = _player_inventory_slots.inventory_potions_slots;
            } 
            else if(_item_selected is Item_info.Item){//.GetType().IsAssignableFrom(item_type)){
                _occupied_slots = _player_inventory.inventory_items_slots.Count;
                _inv_slots_images = _player_inventory_slots.inventory_items_images_slots;
                _inv_slots = _player_inventory_slots.inventory_items_slots;
            }
            else if(_item_selected is null){
                //_occupied_slots = 0;
            }
        }
        else{
            //Debug.Log("object");
            _occupied_slots = object_inv_to_show.items_in_object.Count;
            _inv_slots_images = _obj_inv_slots.obj_inv_items_images_slots;
            _inv_slots = _obj_inv_slots.obj_inv_items_slots;
        }
    }
    public void Start_timer( int slot){
        //Debug.Log("Mouse hover over Inv slot");
        _mouse_on_item = true;
        _slot_number_item_info = slot;
        _start_time = true;
    }
    public void Reset_timer(){
        //Debug.Log("Mouse stop hovering over Inv slot");
        //do seperate func to hadnle reset timer for weapon info ( without _mouse_on_item) left belowe to handle pointer out(this)
        _mouse_on_item = false;
        _start_time = false;
        _time_to_show_item_info_elapsed = 0.0f;
       if(_instance_item_info != null)
           Destroy(_instance_item_info);
    }
    public void Show_item_stats_inv(int slot){
        //use anchors? while playing with maximize on play window with item info tends to change its placce - either on the item slot itslef or way below it 
        if(_occupied_slots > slot && _item_selected!= null){
            _instance_item_info = Instantiate(_instance_item_info_prefab_model,_inv_slots_images[slot].transform.parent.transform.position,_inv_slots_images[slot].transform.parent.transform.rotation) as GameObject;
            _instance_item_info.transform.SetParent(gameObject.transform);
           _instance_item_info.GetComponent<RectTransform>().localPosition += new Vector3(0,-25,0);
            //_instance_item_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(_inv_slots_images[slot].transform.parent.transform.position.x/1080,_inv_slots_images[slot].transform.parent.transform.position.y/607);
            // _instance_item_info.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1);
            // _instance_item_info.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,1f);
            // _instance_item_info.GetComponent<RectTransform>().pivot = new Vector2(0.5f,1);
            if(_item_selected is Weapon_info.Weapon){
                Weapon_info.Weapon weapon = (Weapon_info.Weapon)_item_selected;
                _instance_item_info.GetComponentInChildren<Text>().text = _item_selected.Item_name +"\n DMG: \t" +  weapon.Light_attack_damage + " \t" + weapon.Strong_attack_damage + " \t" + weapon.combo_dmg_bonus +  "\n STM: \t" + weapon.light_attack_stamina_cost+ " \t" + weapon.strong_attack_stamina_cost +" \t" + weapon.combo_attack_stamina_cost;
                _instance_item_info.GetComponentInChildren<Text>().text += "\n Durability: \t" + Mathf.Round( weapon.durability*100f)/100f +"%" + "\n MT: \t" + weapon.material_toughness;
                if(weapon is Dagger){
                    _instance_item_info.GetComponent<RectTransform>().sizeDelta = new Vector2(250,150);
                    Dagger dagger = (Dagger)weapon;
                    _instance_item_info.GetComponentInChildren<Text>().text +="\n Attack speed : " + dagger.attack_speed_multiplayer;
                }
            }
            else if(_item_selected is Whetstones){
                Whetstones whetstone = (Whetstones)_item_selected;
                _instance_item_info.GetComponent<RectTransform>().sizeDelta = new Vector2(250,75);
                _instance_item_info.GetComponentInChildren<Text>().text = _item_selected.Item_name + "\n Durability: \t" + whetstone.durability + "\n Toughness: \t" + whetstone.grid_toughness;
            }
            else if(_item_selected is Materials){
                Materials material = (Materials)_item_selected;
                _instance_item_info.GetComponent<RectTransform>().sizeDelta = new Vector2(250,75);
                _instance_item_info.GetComponentInChildren<Text>().text = _item_selected.Item_name +"\n Quality: \t" + material.quality_class + "\n Size: \t" + material.size; 
            }
            else if(_item_selected is Health_potion){
                Health_potion health_potion = (Health_potion) _item_selected;
                _instance_item_info.GetComponent<RectTransform>().sizeDelta = new Vector2(250,75);
                _instance_item_info.GetComponentInChildren<Text>().text = _item_selected.Item_name +"\n Healing amount: \t" + health_potion.heal_amount + "\n Healing duration: \t" + health_potion.efect_duration ; 
            }
            else if(_item_selected is Berries_healing){
                Berries_healing berry_healing = (Berries_healing) _item_selected;
                _instance_item_info.GetComponent<RectTransform>().sizeDelta = new Vector2(250,75);
                _instance_item_info.GetComponentInChildren<Text>().text = _item_selected.Item_name +"\n Healing amount: \t" + berry_healing.health_restore_amount + "\n Eating time: /s: \t" + berry_healing.eating_time; 
            }
            //set window size base on item info size etc??
        }    
    }
    public void Show_item_creation_requirements(int slot){
        _occupied_slots = _creation_inv.recipes_for_items.Count;
        _slot_number_item_info = slot;
        if(_occupied_slots > slot){
            _item_selected = _creation_inv.recipes_for_items[slot].item_to_create;
            _inv_slots_images = _creation_inv_slots.obj_inv_items_images_slots;
            Show_item_stats_inv(slot);
            _instance_item_info.GetComponent<RectTransform>().sizeDelta += new Vector2(0,25);
            _instance_item_info.GetComponentInChildren<Text>().text += "\n Required ingredients";
            List<Item_info.Item> already_displayed_ingredients = new List<Item_info.Item>();
            foreach(Item_info.Item ingredient in _creation_inv.recipes_for_items[slot].crafting_ingredients){
                Item_info.Item result = already_displayed_ingredients.Find(
                    delegate (Item_info.Item  item_to_found) {
                        return  item_to_found == ingredient;
                    });
                if(result == null){
                    already_displayed_ingredients.Add(ingredient);
                    List <Item_info.Item> repeated_items = _creation_inv.recipes_for_items[slot].crafting_ingredients.FindAll(
                        delegate(Item_info.Item item_to_found){
                            return item_to_found == ingredient;
                        });
                    _instance_item_info.GetComponent<RectTransform>().sizeDelta += new Vector2(0,25);
                    if(repeated_items.Count > 1)
                        _instance_item_info.GetComponentInChildren<Text>().text += "\n" +ingredient.Item_name + " x" + repeated_items.Count;
                    else
                        _instance_item_info.GetComponentInChildren<Text>().text += "\n" +ingredient.Item_name;
                }
            }
        }
    }
    public void Show_item_options_menu(int slot){
        // Debug.Log("after click  " + _input_handler.mouse_right_pressed_inv_flag );
        if(_last_pressed_right){
            if(_instance_item_option_menu != null)
                Destroy(_instance_item_option_menu);
            Reset_timer();
            if(_occupied_slots > slot){
                _instance_item_option_menu = Instantiate(_item_option_menu_prefab,_inv_slots_images[slot].transform.parent.transform.position,_inv_slots_images[slot].transform.parent.transform.rotation) as GameObject;
                _instance_item_option_menu.transform.SetParent(gameObject.transform);
                _instance_item_option_menu.GetComponent<RectTransform>().localPosition += new Vector3(0,-45,0);
                Items_dropdown_options = _instance_item_option_menu.GetComponent<TMPro.TMP_Dropdown>();
                _dropdown_options_title = _instance_item_option_menu.GetComponentInChildren<TMPro.TMP_Text>().gameObject;
                if(!(_item_selected is Health_potion))
                    Remove_option_in_the_item_option_menu("Use item (E)");
                if(!(_item_selected is Weapon_info.Weapon))
                    Remove_option_in_the_item_option_menu("Repair item");
                Items_dropdown_options.onValueChanged.AddListener(delegate{
                    Item_operations_dropdown(Items_dropdown_options,slot);
                });
            }        
        }
    }
    private void Remove_option_in_the_item_option_menu(string option_name){
        for(int i =0;i<Items_dropdown_options.options.Count;i++){
            if(Items_dropdown_options.options[i].text == option_name){
                Items_dropdown_options.options.RemoveAt(i);
                break;
            }
        }
    }
    public void Item_operations_dropdown(TMPro.TMP_Dropdown sender,int slot){   
        //Items_dropdown_options.OnPointerClick()
        _dropdown_options_title.SetActive(false);
        //Debug.Log("value: " + sender.value);
        int index = sender.value;
        if(sender.options[index].text == "Use item (E)"){
            _mouse_on_item = true;
            Use_item_from_inv(); 
        }
        if(sender.options[index].text == "Transfer items (T)"){
            _mouse_on_item = true;
            Transfer_items_to_other_inv();
        }
        if(sender.options[index].text == "Drop items (O)"){
            Drop_item_from_inventory();
        }
        if(sender.options[index].text == "Split one item"){
            Split_one_item_from_inv_stack(slot);
        }
        if(sender.options[index].text == "Split items in half"){
            Split_items_in_half_inv_stack(slot);
        }
        if(sender.options[index].text == "Repair item"){
            Repair_item(slot); 
        }
        if(sender.options[index].text == "Exit"){
            
        }
        // if(sender.value == 0){
        //     Debug.Log("0");
        //     // _mouse_on_item = true;  
        //     // Use_item_from_inv();
        // }
        // if(sender.value == 1){
        //     Debug.Log("1");
        //     // _mouse_on_item = true;  
        //     // Transfer_items_to_other_inv();
        // }
        // if(sender.value == 2){
        //    Debug.Log("2");
        //    // Drop_item_from_inventory();
        // }
        // if(sender.value == 3){
        //     Debug.Log("3");
        //    // Split_one_item_from_inv_stack(slot);
        // }
        // if(sender.value == 4){
        //     Debug.Log("4");
        //     //Split_items_in_half_inv_stack(slot);
        // }
        // if(sender.value == 5){
        //     Debug.Log("5");
        //    // Repair_item(slot);
        // }
        // if(sender.value == 6){

        // }
        Destroy(_instance_item_option_menu);
        _pressed_outside_dropdown = true;
    }
    public void Use_item_from_inv(){
        if(_mouse_on_item){
            if(_cursor_player_inv && _occupied_slots > _slot_number_item_info ){
                if(_inv_slots[_slot_number_item_info].item is Health_potion){
                    Health_potion health_potion = (Health_potion) _player_inventory.inventory_potions_slots[_slot_number_item_info];
                    _player_inventory.gameObject.GetComponent<Player_info>().Start_healing_player_process(health_potion.efect_duration,health_potion.heal_amount);
                    _inv_slots[_slot_number_item_info].stack_amount -= 1;
                    if(_inv_slots[_slot_number_item_info].stack_amount == 0){
                        _player_inventory.Remove_item_from_player_inv(_inv_slots[_slot_number_item_info]);
                        Move_slots_to_left_inv(_inv_slots[_slot_number_item_info],_player_inventory.amount_of_items_slots);
                    }
                }
            }
        }
    }
    public void Drop_item_from_inventory(){
        if(!_mouse_on_item)
            return;
        Rigidbody _instance_item_dropped;
        _instance_item_dropped = Instantiate(_item_dropped_prefab,_root_for_drop_items.position,_root_for_drop_items.rotation) as Rigidbody;
        _instance_item_dropped.AddForce(_root_for_drop_items.forward * _dropp_items_force);
        _instance_item_dropped.GetComponent<Item_dropped>().item_dropped = _item_selected;
        _instance_item_dropped.GetComponent<Item_dropped>().amount_of_dropped_items =  _slot_selected.stack_amount;
        if(_cursor_player_inv)
            _player_inventory.Remove_item_from_player_inv(_slot_selected);
        else
            object_inv_to_show.Remove_item_from_object(_slot_selected.slot_number);
        Move_slots_to_left_inv(_slot_selected,_player_inventory.amount_of_items_slots);
    }
    public void Split_one_item_from_inv_stack(int slot){
        if(_slot_selected.stack_amount >1){
            if(_cursor_player_inv && _occupied_slots < _player_inventory.amount_of_items_slots){
                Add_item_to_player_inv_last_slot(_item_selected,1);
                _slot_selected.stack_amount -= 1;
            }         
            else if(!_cursor_player_inv && _occupied_slots < object_inv_to_show.amount_of_item_slots){
                object_inv_to_show.Add_item_to_object(object_inv_to_show.items_in_object[slot],1);
                _slot_selected.stack_amount -= 1;
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item = _item_selected;
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount += 1;
            }
        }
        else 
            Debug.Log("Item in one copy - cant split");
    }
    public void Split_items_in_half_inv_stack(int slot){
        if( _slot_selected.stack_amount >1){
            if(_cursor_player_inv && _occupied_slots < _player_inventory.amount_of_items_slots){
                Add_item_to_player_inv_last_slot(_item_selected,_slot_selected.stack_amount/2);
                _slot_selected.stack_amount -= _slot_selected.stack_amount/2;
            }
             else if(!_cursor_player_inv && _occupied_slots < object_inv_to_show.amount_of_item_slots){
                object_inv_to_show.Add_item_to_object(_item_selected,_slot_selected.stack_amount/2);
                
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item = _item_selected;
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount += _slot_selected.stack_amount/2;
                _slot_selected.stack_amount -= _slot_selected.stack_amount/2;
            }
        }
        else 
            Debug.Log("Item in one copy - cant split");
    }
    public void Repair_item(int slot){
        if(_cursor_player_inv && Check_for_items_to_reapir() && _item_selected is Weapon_info.Weapon){
            _player_inventory.inventory_weapons_slots[slot].Add_durability(1);
        }
    }
    public bool Check_for_items_to_reapir(){
        return Inv_whetstone_level(3);
        // foreach(Item_info.Item item in _player_inventory.inventory_items_slots){
        //     if(item is Whetstones){
        //         Whetstones whetstone = (Whetstones) item;
        //         if(whetstone.grid_toughness>=4)
        //             whetstone.Remove_durability(50);
        //              //item = (Item_info.Item) whetstone;
        //             return true;
        //     }
        // }
        // Debug.Log("Not enough resources to reapir weapon");
        // return false;
    }
    private bool Inv_whetstone_level(int whetstone_grid_level ){
        for(int i=0;i<_player_inventory.inventory_items_slots.Count;i++){
            if(_player_inventory.inventory_items_slots[i] is Whetstones){
                Whetstones whetstone = (Whetstones) _player_inventory.inventory_items_slots[i];
                if(whetstone.grid_toughness >= whetstone_grid_level){
                    whetstone.Remove_durability(50);
                    if(whetstone.durability > 0)
                        _player_inventory.inventory_items_slots[i] = (Item_info.Item) whetstone;
                    else{
                        _player_inventory_slots.inventory_items_slots[i].stack_amount -=1;
                        if(_player_inventory_slots.inventory_items_slots[i].stack_amount == 0){
                            _player_inventory.inventory_items_slots.RemoveAt(i);
                            Choose_which_inventory_type_slot(_player_inventory_slots.inventory_items_slots[i]);
                            Move_slots_to_left_inv(_slot_selected,_player_inventory.amount_of_items_slots);
                        }
                    }
                    return true;
                }
            }
        }
        Debug.Log("Not enough resources to reapir weapon");
        return false;
    }      
    public void Check_if_mouse_click_on_dropdown_title(){
        _help.position = new Vector3( _input_handler.mouse_position.x,_input_handler.mouse_position.y,0);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster_inv_slots.Raycast(_help, results);

        foreach(var hit in results){
            var Dropdown_select_button = hit.gameObject.GetComponent<TMPro.TMP_Dropdown>();
            //Debug.Log("hitted something");
            if(Dropdown_select_button){
                _help2 = true;
                _pressed_outside_dropdown = false;
                //Debug.Log("Dropdown menu clicked");
            }
            else{
                //Debug.Log("Dropdown menu not clicked");
            }
        }
        if(_instance_item_option_menu!=null && _pressed_outside_dropdown){
            Destroy(_instance_item_option_menu);
        }
        if(_instance_item_creation_menu!=null && _pressed_outside_dropdown){
            Destroy(_instance_item_creation_menu);
        }
        if(_instance_item_creation_amount != null && _pressed_outside_dropdown)
            Destroy(_instance_item_creation_amount);
    }
    public void Check_if_mouse_click_on_dropdown_options(){
        _help.position = new Vector3( _input_handler.mouse_position.x,_input_handler.mouse_position.y,0);
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster_inv_slots.Raycast(_help, results);

        foreach(var hit in results){
            var Dropdown_select_button = hit.gameObject.GetComponent<TMPro.TextMeshPro>();
            //Debug.Log("hitted something");
            if(Dropdown_select_button){
                //Debug.Log("one of options clicked");
               // _pressed_outside_dropdown = false;
            }
            else{
                //Debug.Log("none of options clicked");
            }
        }
        // if(!_pressed_outside_dropdown){
        //     Destroy(_instance_item_option_menu);
        // }
    }
    public void Show_item_creation_menu(int slot){
        if(_last_pressed_right){
            if(_instance_item_creation_menu != null)
                Destroy(_instance_item_creation_menu);
            if(_instance_item_creation_amount != null)
                Destroy(_instance_item_creation_amount);
            Reset_timer();
            if(_creation_inv.recipes_for_items[slot] != null){
                _instance_item_creation_menu = Instantiate(_item_creation_menu_prefab,_creation_inv_slots.obj_inv_items_images_slots[slot].transform.parent.transform.position,_creation_inv_slots.obj_inv_items_images_slots[slot].transform.parent.transform.rotation) as GameObject;
                _instance_item_creation_menu.transform.SetParent(gameObject.transform);
                _instance_item_creation_menu.GetComponent<RectTransform>().localPosition += new Vector3(0,-45,0);
                Items_dropdown_options = _instance_item_creation_menu.GetComponent<TMPro.TMP_Dropdown>();
                _dropdown_options_title = _instance_item_creation_menu.GetComponentInChildren<TMPro.TMP_Text>().gameObject;
                Items_dropdown_options.onValueChanged.AddListener(delegate{
                    Item_creation_dropdown(Items_dropdown_options,slot);
                });
            }        
        }
    }
    public void Item_creation_dropdown(TMPro.TMP_Dropdown sender,int slot){   
        //Items_dropdown_options.OnPointerClick()
        _dropdown_options_title.SetActive(false);
        //Debug.Log("value: " + sender.value);
        if(sender.value == 0){
            //create one item
            Create_item(slot);
        }
        if(sender.value == 1){
            //create all possible copies of selected item
            while(true){
                if(! Create_item(slot))
                    break;
            }
        }
        if(sender.value == 2){
            //create given number amount of item
            _instance_item_creation_amount = Instantiate(_input_field_prefab,_creation_inv_slots.obj_inv_items_images_slots[slot].transform.parent.transform.position,_creation_inv_slots.obj_inv_items_images_slots[slot].transform.parent.transform.rotation) as GameObject;
            _instance_item_creation_amount.transform.SetParent(gameObject.transform);
            _instance_item_creation_amount.GetComponent<RectTransform>().localPosition += new Vector3(0,-45,0);
        }
        if(sender.value == 3){
            //exit

        }
        Destroy(_instance_item_creation_menu);
        _pressed_outside_dropdown = true;
    }
    public bool Create_item(int slot){
        if(Check_for_ingredients_to_create(slot)){
            //Debug.Log("creating item: " + _creation_inv.recipes_for_items[slot].item_to_create);
            Item_info.Item new_item = ScriptableObject.Instantiate(_creation_inv.recipes_for_items[slot].item_to_create);
            Add_item_to_player_inv_check_for_same_object(new_item,1);
            // Whetstones new_whetstone;
            // //Debug.Log("Creating item: " + _creation_inv.recipes_for_items[slot].item_to_create);
            // new_whetstone = (Whetstones)ScriptableObject.CreateInstance(typeof(Whetstones));
            // new_whetstone = (Whetstones) _creation_inv.recipes_for_items[slot].item_to_create;
            // _player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.Count].item = new_whetstone; //(Item_info.Item)ScriptableObject.CreateInstance(typeof(_creation_inv.items_possible_to_create[slot]));
            // _player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.Count].stack_amount = 1;
            // _player_inventory.inventory_items_slots.Add(new_whetstone);
            return true;
        }
        else{
            return false;
        }
    } 
    public bool Check_for_ingredients_to_create(int recipe_number){
        List<Item_info.Item>  requierd_ingredients = new List<Item_info.Item>( _creation_inv.recipes_for_items[recipe_number].crafting_ingredients);
        List<Item_info.Item> items_used_to_create_item = new List<Item_info.Item>();
        foreach(Materials material in _player_inventory.inventory_materials_slots){
            int help = _player_inventory_slots.inventory_materials_slots[_player_inventory.inventory_materials_slots.IndexOf(material)].stack_amount;
            while(help>0){
                if(requierd_ingredients.Remove(material)){
                    items_used_to_create_item.Add(material);
                    help --;
                }
                else
                    break;
            }
        }
        foreach(Item_info.Item item in _player_inventory.inventory_items_slots){
            int help = _player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.IndexOf(item)].stack_amount;
            while(help>0){
                if(requierd_ingredients.Remove(item)){
                    items_used_to_create_item.Add(item);
                    help --;
                }
                else
                    break;
            }
        }
        if(requierd_ingredients.Count == 0){
        //Debug.Log("All ingredients are in inventory");
            foreach(Item_info.Item item in items_used_to_create_item){
                if(_player_inventory.inventory_materials_slots.Contains(item)){
                    _player_inventory_slots.inventory_materials_slots[_player_inventory.inventory_materials_slots.IndexOf((Materials)item)].stack_amount -= 1;
                    if(_player_inventory_slots.inventory_materials_slots[_player_inventory.inventory_materials_slots.IndexOf((Materials)item)].stack_amount == 0){
                        _inv_slots =  _player_inventory_slots.inventory_materials_slots;
                        Move_slots_to_left_inv(_player_inventory_slots.inventory_materials_slots[_player_inventory.inventory_materials_slots.IndexOf((Materials)item)],_player_inventory.amount_of_items_slots);
                        _player_inventory.inventory_materials_slots.Remove((Materials)item);
                    }
                }
                else if(_player_inventory.inventory_items_slots.Contains(item)){
                    _player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.IndexOf(item)].stack_amount -= 1;
                    if(_player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.IndexOf(item)].stack_amount == 0){
                        _inv_slots =  _player_inventory_slots.inventory_items_slots;
                        Move_slots_to_left_inv(_player_inventory_slots.inventory_items_slots[_player_inventory.inventory_items_slots.IndexOf(item)],_player_inventory.amount_of_items_slots);
                        _player_inventory.inventory_items_slots.Remove(item);
                    }
                }
            }
            return true;
        }
        else{
            Debug.Log("Something is missing");
            return false;
        }
    }
    public void Set_object_inv_items_amount(){
        if(object_inv_to_show != null){
            for(int i=0; i<object_inv_to_show.items_in_object.Count;i++){
                object_inv_to_show.obj_inv_item_amount_on_slot[i] = _obj_inv_slots.obj_inv_items_slots[i].stack_amount;
                object_inv_to_show.items_in_object[i] = _obj_inv_slots.obj_inv_items_slots[i].item;
            }
        }
    }
    //do more functions that return private fields etc - think which variables should be public and which private
    public  Inventory_slots Get_player_inv_slots() =>  _player_inventory_slots;
    // two pairs of ad items functions - definitely could combine checking func to one - not for now - i am done with this part of inv for the moment
    //maybe functions could return number of left items - 0 if all items were transfered and some number if inv was full - it would save doing extra variables in other scripts - not sure if good idea - could look into it
    public void Add_item_to_obj_inv_check_for_same_object(Item_info.Item item,int stack_amount){
        added_all_items = false;
        
        _occupied_slots = object_inv_to_show.items_in_object.Count;
        _inv_slots = _obj_inv_slots.obj_inv_items_slots;
        
    
        for(int i=0;i<_occupied_slots;i++){
            if(_inv_slots[i].item == item && _inv_slots[i].stack_amount < _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount < _inv_slots[i].item.max_stack_amount ){
                //stack items
                if(_inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].item.max_stack_amount){
                    _inv_slots[i].stack_amount += stack_amount;
                    added_all_items = true;
                    goto Object_found2;
                }
                else if (_inv_slots[i].stack_amount + stack_amount > _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].item.max_stack_amount){
                    stack_amount -= (_inv_slots[i].max_stack_amount - _inv_slots[i].stack_amount);
                    _inv_slots[i].stack_amount = _inv_slots[i].max_stack_amount;
                }
                else if (_inv_slots[i].stack_amount + stack_amount > _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount > _inv_slots[i].item.max_stack_amount){
                    if(_inv_slots[i].max_stack_amount < _inv_slots[i].item.max_stack_amount){
                        stack_amount -= (_inv_slots[i].max_stack_amount - _inv_slots[i].stack_amount);
                        _inv_slots[i].stack_amount = _inv_slots[i].max_stack_amount;
                    }
                    else{
                        stack_amount -= (_inv_slots[i].item.max_stack_amount - _inv_slots[i].stack_amount);
                        _inv_slots[i].stack_amount = _inv_slots[i].item.max_stack_amount;
                    }
                }
                else if (_inv_slots[i].stack_amount + stack_amount < _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount > _inv_slots[i].item.max_stack_amount){
                    stack_amount -= (_inv_slots[i].item.max_stack_amount - _inv_slots[i].stack_amount);
                    _inv_slots[i].stack_amount = _inv_slots[i].item.max_stack_amount;
                }
            }
        }
        // no same object in inventory or totally capped - add on last free slot
        Add_item_to_obj_inv_last_slot(item,stack_amount);
        Object_found2:
            Debug.Log("");
    }
    public void Add_item_to_player_inv_check_for_same_object(Item_info.Item item,int stack_amount){
        added_all_items = false;
        
        if(item is Weapon_info.Weapon){
            _occupied_slots = _player_inventory.inventory_weapons_slots.Count;
            _inv_slots = _player_inventory_slots.inventory_weapons_slots;
        }
        else if(item  is Materials ){
            _occupied_slots = _player_inventory.inventory_materials_slots.Count;
            _inv_slots = _player_inventory_slots.inventory_materials_slots;
        }
        else if(item is Potions){
            _occupied_slots = _player_inventory.inventory_potions_slots.Count;
            _inv_slots = _player_inventory_slots.inventory_potions_slots;
        }
        else{
            _occupied_slots = _player_inventory.inventory_items_slots.Count;
            _inv_slots = _player_inventory_slots.inventory_items_slots;
        }
        for(int i=0;i<_occupied_slots;i++){
            if(_inv_slots[i].item.item_id == item.item_id && _inv_slots[i].stack_amount < _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount < _inv_slots[i].item.max_stack_amount ){
                //stack items
                if(_inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].item.max_stack_amount){
                    _inv_slots[i].stack_amount += stack_amount;
                    added_all_items = true;
                    goto Object_found2;
                }
                else if (_inv_slots[i].stack_amount + stack_amount > _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount <= _inv_slots[i].item.max_stack_amount){
                    stack_amount -= (_inv_slots[i].max_stack_amount - _inv_slots[i].stack_amount);
                    _inv_slots[i].stack_amount = _inv_slots[i].max_stack_amount;
                }
                else if (_inv_slots[i].stack_amount + stack_amount > _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount > _inv_slots[i].item.max_stack_amount){
                    if(_inv_slots[i].max_stack_amount < _inv_slots[i].item.max_stack_amount){
                        stack_amount -= (_inv_slots[i].max_stack_amount - _inv_slots[i].stack_amount);
                        _inv_slots[i].stack_amount = _inv_slots[i].max_stack_amount;
                    }
                    else{
                        stack_amount -= (_inv_slots[i].item.max_stack_amount - _inv_slots[i].stack_amount);
                        _inv_slots[i].stack_amount = _inv_slots[i].item.max_stack_amount;
                    }
                }
                else if (_inv_slots[i].stack_amount + stack_amount < _inv_slots[i].max_stack_amount && _inv_slots[i].stack_amount + stack_amount > _inv_slots[i].item.max_stack_amount){
                    stack_amount -= (_inv_slots[i].item.max_stack_amount - _inv_slots[i].stack_amount);
                    _inv_slots[i].stack_amount = _inv_slots[i].item.max_stack_amount;
                }
            }
        }
        // no same object in inventory or totally capped - add on last free slot
        Add_item_to_player_inv_last_slot(item,stack_amount);
        Object_found2:
            Debug.Log("");
    }
    public void Add_item_to_player_inv_last_slot(Item_info.Item item,int stack_amount){
        added_all_items = false;
        _item_derivative = false;
        //do check for overcapping slot use general variables like _occupied slots
        // add on last free slot
        if(item is Weapon_info.Weapon || item is Materials || item is Potions)
            _item_derivative = true;
    
        if(item is Weapon_info.Weapon && _player_inventory_slots.inventory_weapons_slots[5].item ==null){
            Add_item_to_player_inv_category_last_slot(item,stack_amount,_player_inventory_slots.inventory_weapons_slots,_player_inventory.inventory_weapons_slots.Count);
        }
            
        else if(item is Materials && _player_inventory_slots.inventory_materials_slots[5].item == null){
            Add_item_to_player_inv_category_last_slot(item,stack_amount,_player_inventory_slots.inventory_materials_slots,_player_inventory.inventory_materials_slots.Count);
        }
        else if(item is Potions && _player_inventory_slots.inventory_potions_slots[5].item == null){
            Add_item_to_player_inv_category_last_slot(item,stack_amount,_player_inventory_slots.inventory_potions_slots,_player_inventory.inventory_potions_slots.Count);
        }
        else if(item is Item_info.Item && _player_inventory_slots.inventory_items_slots[5].item == null && !_item_derivative){
            Add_item_to_player_inv_category_last_slot(item,stack_amount,_player_inventory_slots.inventory_items_slots,_player_inventory.inventory_items_slots.Count);
        }
        else{
            Debug.Log("No space in items type slots");
           _player_inventory.GetComponent<Player_Movemnet.Movement>().dropped_item_new_amount = stack_amount;
           remaining_item_amount = stack_amount;
        }
    }
    private void Add_item_to_player_inv_category_last_slot(Item_info.Item item,int stack_amount,List<Slot> list_of_slots,int number_of_items_in_inv){
        _player_inventory.Add_item_to_player_inv_list(item);
        list_of_slots[number_of_items_in_inv].item = item;
        if(list_of_slots[number_of_items_in_inv].max_stack_amount >= stack_amount && list_of_slots[number_of_items_in_inv].item.max_stack_amount >= stack_amount){
            list_of_slots[number_of_items_in_inv].stack_amount = stack_amount;
            added_all_items = true;
        }
        else{
            //slot max is capping
            if(list_of_slots[number_of_items_in_inv].max_stack_amount < stack_amount){
                stack_amount -= list_of_slots[number_of_items_in_inv].max_stack_amount - list_of_slots[number_of_items_in_inv].stack_amount;
                list_of_slots[number_of_items_in_inv].stack_amount = list_of_slots[number_of_items_in_inv].max_stack_amount;
            }
            //item max is capping
            else{
                stack_amount -= list_of_slots[number_of_items_in_inv].item.max_stack_amount;
                list_of_slots[number_of_items_in_inv].stack_amount = list_of_slots[number_of_items_in_inv].item.max_stack_amount;
            }
            Add_item_to_player_inv_last_slot(item,stack_amount);
        }
    }
     public void Add_item_to_obj_inv_last_slot(Item_info.Item item,int stack_amount){
        added_all_items = false;
        //do check for overcapping slot use general variables like _occupied slots
        // add on last free slot
        if(item is Weapon_info.Weapon || item is Materials || item is Potions)
            _item_derivative = true;
    
        if(item is Item_info.Item && _obj_inv_slots.obj_inv_items_slots[5].item == null ){
            object_inv_to_show.Add_item_to_object(item,stack_amount);
            _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item = item;
            if(_obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].max_stack_amount >= stack_amount &&  _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item.max_stack_amount >= stack_amount){
                _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount = stack_amount;
                added_all_items= true;
            }
            else{
                //slot max is capping
                if( _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].max_stack_amount < stack_amount){
                    stack_amount -=  _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].max_stack_amount;
                    _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount =  _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].max_stack_amount;
                }
                //item max is capping
                else{
                    stack_amount -=  _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item.max_stack_amount;
                    _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].stack_amount =  _obj_inv_slots.obj_inv_items_slots[object_inv_to_show.items_in_object.Count-1].item.max_stack_amount;
                }
               
                Add_item_to_player_inv_last_slot(item,stack_amount);
            }
        }
        else{
            Debug.Log("No space in items type slots");
           _player_inventory.GetComponent<Player_Movemnet.Movement>().dropped_item_new_amount = stack_amount;
           remaining_item_amount = stack_amount;
        }
    }
    //buttons to toggle between crafting and player inv
    public void Show_crafting_menu(){
        _crafting_menu.SetActive(true);
        _player_inventory_menu.SetActive(false);
        if(_instance_item_info != null)
            Destroy(_instance_item_info);
        if(_instance_item_option_menu != null)
            Destroy(_instance_item_option_menu);
       
    }
    public void Show_player_inventory(){
        _crafting_menu.SetActive(false);
        _player_inventory_menu.SetActive(true);
        
        if(_instance_item_info != null)
            Destroy(_instance_item_info);
        if(_instance_item_creation_menu != null)
            Destroy(_instance_item_creation_menu);
        if(_instance_item_creation_amount != null)
            Destroy(_instance_item_creation_amount);
    }
    public void Show_change_inv_buttons(){
        _inventory_buttons.SetActive(true);
    }
    public void Hide_change_inv_buttons(){
        _inventory_buttons.SetActive(false);
    }
    public void Hide_player_inventory(){
        _player_inventory_menu.SetActive(false);
    }
    public void Hide_crafting_menu(){
        _crafting_menu.SetActive(false);
    }
}