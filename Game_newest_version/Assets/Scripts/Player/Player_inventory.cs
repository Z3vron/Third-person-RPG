using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
//ctr + k , ctr + 0(zero) collapse(fold) all functions/regions/classes etc
//ctr + shift+[ collapse(fold) active(where is mouse course) function/region/class etc

namespace Player_inventory_info{
    public class Player_inventory : MonoBehaviour{
        //for tests
        #region Items to add and their amounts at start to player inv(for tests)
            [Header("Items to add and their amounts at start to player inv(for tests)")]
            [SerializeField] private Item_info.Item _item_to_add_1;
            [SerializeField] private int _item_to_add_1_amount;
            [SerializeField] private Item_info.Item _item_to_add_2;
            [SerializeField] private int _item_to_add_2_amount;
            [SerializeField] private Item_info.Item _item_to_add_3;
            [SerializeField] private int _item_to_add_3_amount;
        #endregion
        #region Weapons and potions in quick slots
            [Header("Weapons and potions in quick slots")]       
            public Weapon_info.Weapon current_weapon_for_right_hand;
            public Weapon_info.Weapon backup_weapon_right;
            public Weapon_info.Weapon current_weapon_for_left_hand;
            public Weapon_info.Weapon backup_weapon_left;
            public Weapon_info.Weapon unarmed;
            public List<Slot> quick_slots_potions = new List<Slot>();
            public Potions empty_potion;
        #endregion
        public Armor_info.Armour player_armour;
        #region Items in player inventory divided by item category
            [Header("Lists of items in player inventory")]
            public List<Weapon_info.Weapon> inventory_weapons_slots = new List<Weapon_info.Weapon>();
            public List<Potions> inventory_potions_slots = new List<Potions>();
            public List<Materials> inventory_materials_slots = new List<Materials>();
            public List<Item_info.Item> inventory_items_slots = new List<Item_info.Item>();
        #endregion
        [Tooltip("Holds information if player inventory or crafting inventory  is open")]
        public bool inventory_open = false;
        [Tooltip("Amount of items in 1 row in player inventory applies to all rows")]
        public int amount_of_items_slots = 6;
        #region Other scripts and components references
            [Header("Other scripts and components references")]
            [SerializeField] private Inventories _inventories;
            [SerializeField] private CinemachineFreeLook _cinemachine_camera;
            private Input_handler _input_handler;
            private Weapon_slot_manager.Weapon_manager weapon_slot_manager;
            private Player_Movemnet.Movement _player_movement;
            private Animator _animator;
        #endregion
        #region Variables used to store camera values to restore them after freezing camera for browsing inventory
        private float _cinemachine_camera_x_sensitivity;
        private float _cinemachine_camera_y_sensitivity;
        #endregion
        #region Poison weapons variables    
            [Tooltip("Timer how long left weapon will be poison")]
            private float _poison_timer_left_weapon = 0;
            private float _poison_timer_right_weapon = 0;
            private float _poison_time_left_weapon;
            private float _poison_time_right_weapon;
        #endregion
        private void Awake() {
            weapon_slot_manager = GetComponent<Weapon_slot_manager.Weapon_manager>();
            _input_handler = GetComponent<Input_handler>();
            _player_movement = GetComponent<Player_Movemnet.Movement>();
            _animator = GetComponentInChildren<Animator>();
        }
        private void Start() {
            //just for testing
            _inventories.Add_item_to_player_inv_last_slot(_item_to_add_1,_item_to_add_1_amount);
            _inventories.Add_item_to_player_inv_last_slot(_item_to_add_2,_item_to_add_2_amount);
            _inventories.Add_item_to_player_inv_last_slot(_item_to_add_3,_item_to_add_3_amount);           
            
            //definition here elsewhere value is changed 
            //load fists at the start to left hand
            weapon_slot_manager.Load_weapon_to_slot(unarmed,false);
            current_weapon_for_left_hand = unarmed;
            //load fists at the start to right hand
            weapon_slot_manager.Load_weapon_to_slot(unarmed,true);
            current_weapon_for_right_hand = unarmed;
    
           backup_weapon_left = unarmed;
           backup_weapon_right = unarmed;
           
            for(int i=0;i<4;i++){
                quick_slots_potions[i].item = null;
                quick_slots_potions[i].stack_amount = 0;
                _inventories.player_slots.Update_quick_slot_potions_icon(empty_potion,i);
           }
        }
        private void Update() {
            if(current_weapon_for_left_hand.durability == 0){
                backup_weapon_left = current_weapon_for_left_hand;
                current_weapon_for_left_hand = unarmed;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_left_hand,false);
            }
            if(current_weapon_for_right_hand.durability == 0){
                Debug.Log("Weapon broken");
                backup_weapon_right = current_weapon_for_right_hand;
                current_weapon_for_right_hand = unarmed;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_right_hand,true);
            }
            if(current_weapon_for_right_hand.isPoisoned){
                _poison_timer_right_weapon += Time.deltaTime;
                if(_poison_timer_right_weapon >= _poison_time_right_weapon){
                    current_weapon_for_right_hand.isPoisoned = false;
                    current_weapon_for_right_hand.poison_damage = 0;
                    current_weapon_for_right_hand.poison_duration = 0;
                    _poison_timer_right_weapon = 0;
                }
            }
            if(current_weapon_for_left_hand.isPoisoned){
                _poison_timer_left_weapon += Time.deltaTime;
                if(_poison_timer_left_weapon >= _poison_time_left_weapon){
                    current_weapon_for_left_hand.isPoisoned = false;
                    current_weapon_for_left_hand.poison_damage = 0;
                    current_weapon_for_left_hand.poison_duration = 0;
                    _poison_timer_left_weapon = 0;
                }
            }
        }
        public void Check_left_weapon(){
            if(current_weapon_for_left_hand != unarmed){
                backup_weapon_left = current_weapon_for_left_hand;
                current_weapon_for_left_hand = unarmed;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_left_hand,false);
            }
            else if(current_weapon_for_left_hand == unarmed &&  backup_weapon_left.durability > 0){
                current_weapon_for_left_hand = backup_weapon_left;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_left_hand,false);
                backup_weapon_left = unarmed;
            }
        }
        public void Check_right_weapon(){
            //toggles between fists and weapon chosen for a hand slot
            if(current_weapon_for_right_hand != unarmed){
                backup_weapon_right = current_weapon_for_right_hand;
                current_weapon_for_right_hand = unarmed;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_right_hand,true);
            }
            else if(current_weapon_for_right_hand == unarmed && backup_weapon_right.durability > 0){
                current_weapon_for_right_hand = backup_weapon_right;
                weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_right_hand,true);
                backup_weapon_right = unarmed;
            }
        }
        //this two functions might be combined/ to one 
        public void Change_weapon_for_left_hand(Slot weapon_slot_inv,bool slot_in_playyer_inv){
            Weapon_info.Weapon temp_weapon;
            temp_weapon = (Weapon_info.Weapon)weapon_slot_inv.item;
            while(true){
                if(temp_weapon.durability > 0){ 
                    if(current_weapon_for_left_hand == unarmed && backup_weapon_left == unarmed){
                        current_weapon_for_left_hand = (Weapon_info.Weapon)weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }  
                    else if(current_weapon_for_left_hand != unarmed && inventory_weapons_slots.Count < amount_of_items_slots){
                        Add_item_to_player_inv_list(current_weapon_for_left_hand);
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].item = current_weapon_for_left_hand;
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].stack_amount = 1;
                        current_weapon_for_left_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }
                    else if(current_weapon_for_left_hand != unarmed && inventory_weapons_slots.Count == amount_of_items_slots && weapon_slot_inv.stack_amount == 1){
                        temp_weapon = current_weapon_for_left_hand;
                        if(slot_in_playyer_inv)
                            inventory_weapons_slots[weapon_slot_inv.slot_number] = current_weapon_for_left_hand;
                        else
                            _inventories.object_inv_to_show.items_in_object[weapon_slot_inv.slot_number] = current_weapon_for_left_hand;
                        current_weapon_for_left_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.item = temp_weapon;
                        
                    } 
                    else if(backup_weapon_left != unarmed && inventory_weapons_slots.Count < amount_of_items_slots){
                        Add_item_to_player_inv_list(backup_weapon_left);
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].item = backup_weapon_left;
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].stack_amount = 1;
                        backup_weapon_left = unarmed;
                        current_weapon_for_left_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }
                    else if(backup_weapon_left != unarmed && inventory_weapons_slots.Count == amount_of_items_slots && weapon_slot_inv.stack_amount == 1){
                        if(slot_in_playyer_inv)
                            inventory_weapons_slots[weapon_slot_inv.slot_number] = backup_weapon_left;
                        else
                            _inventories.object_inv_to_show.items_in_object[weapon_slot_inv.slot_number] = backup_weapon_left;
                        current_weapon_for_left_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.item = backup_weapon_left;
                        backup_weapon_left = unarmed;
                    }
                    else{
                        Debug.Log("capped inv");
                        break;
                    }
                    //load weapon to right hand
                    weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_left_hand,false);
                    break;                
                }
                else{
                    Debug.Log("Weapon has 0 durability");
                }
            }
            _inventories.Get_player_inv_slots().Update_inventory_player_UI(this);
            _inventories.Get_object_inv_slots().Update_inventory_object_UI(_inventories.object_inv_to_show);
        }
        public void Change_weapon_for_right_hand(Slot weapon_slot_inv,bool slot_in_playyer_inv){
            Weapon_info.Weapon temp_weapon;
            temp_weapon = (Weapon_info.Weapon)weapon_slot_inv.item;
            while(true){
                if(temp_weapon.durability > 0){ 
                    if(current_weapon_for_right_hand == unarmed && backup_weapon_right == unarmed){
                        current_weapon_for_right_hand = (Weapon_info.Weapon)weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }  
                    else if(current_weapon_for_right_hand != unarmed && inventory_weapons_slots.Count < amount_of_items_slots){
                        Add_item_to_player_inv_list(current_weapon_for_right_hand);
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].item = current_weapon_for_right_hand;
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].stack_amount = 1;
                        current_weapon_for_right_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                                
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }
                    else if(current_weapon_for_right_hand != unarmed && inventory_weapons_slots.Count == amount_of_items_slots && weapon_slot_inv.stack_amount == 1){
                        temp_weapon = current_weapon_for_right_hand;
                        if(slot_in_playyer_inv)
                            inventory_weapons_slots[weapon_slot_inv.slot_number] = current_weapon_for_right_hand;
                        else
                            _inventories.object_inv_to_show.items_in_object[weapon_slot_inv.slot_number] = current_weapon_for_right_hand;
                        current_weapon_for_right_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.item = temp_weapon;
                        
                    } 
                    else if(backup_weapon_right != unarmed && inventory_weapons_slots.Count < amount_of_items_slots){
                        Add_item_to_player_inv_list(backup_weapon_right);
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].item = backup_weapon_right;
                        _inventories.Get_player_inv_slots().inventory_weapons_slots[inventory_weapons_slots.Count-1].stack_amount = 1;
                        backup_weapon_right = unarmed;
                        current_weapon_for_right_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.stack_amount -= 1;
                        if(weapon_slot_inv.stack_amount == 0){
                            // Debug.Log("removing weapon - no copy left in slot");
                            if(slot_in_playyer_inv)
                                inventory_weapons_slots.RemoveAt(weapon_slot_inv.slot_number);
                            else
                                _inventories.object_inv_to_show.Remove_item_from_object(weapon_slot_inv.slot_number);
                            _inventories.Move_slots_to_left_inv(weapon_slot_inv,amount_of_items_slots);
                        }
                    }
                    else if(backup_weapon_right != unarmed && inventory_weapons_slots.Count == amount_of_items_slots && weapon_slot_inv.stack_amount == 1){
                        if(slot_in_playyer_inv)
                            inventory_weapons_slots[weapon_slot_inv.slot_number] = backup_weapon_right;
                        else
                            _inventories.object_inv_to_show.items_in_object[weapon_slot_inv.slot_number] = backup_weapon_right;
                        current_weapon_for_right_hand = (Weapon_info.Weapon) weapon_slot_inv.item;
                        weapon_slot_inv.item = backup_weapon_right;
                        backup_weapon_right = unarmed;
                    }
                    else{
                        Debug.Log("capped inv");
                        break;
                    }
                    //load weapon to right hand
                    weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_right_hand,true);
                    break;                
                }
                else{
                    Debug.Log("Weapon has 0 durability");
                }
            } 
            _inventories.Get_player_inv_slots().Update_inventory_player_UI(this);
            _inventories.Get_object_inv_slots().Update_inventory_object_UI(_inventories.object_inv_to_show);
        }
        public void Change_potion_in_slot(Slot potion_slot_inv,int quick_slot_number,bool slot_in_playyer_inv){
            Potions temp_potion;
            if(quick_slots_potions[quick_slot_number].item == null){
                quick_slots_potions[quick_slot_number].item = potion_slot_inv.item;
                if(quick_slots_potions[quick_slot_number].max_stack_amount >= potion_slot_inv.stack_amount){
                    quick_slots_potions[quick_slot_number].stack_amount = potion_slot_inv.stack_amount;
                    if(slot_in_playyer_inv)
                        Remove_item_from_player_inv(potion_slot_inv);
                    else
                        _inventories.object_inv_to_show.Remove_item_from_object(potion_slot_inv.slot_number);
                    _inventories.Move_slots_to_left_inv(potion_slot_inv,amount_of_items_slots);
                }
                else{
                    quick_slots_potions[quick_slot_number].stack_amount = quick_slots_potions[quick_slot_number].max_stack_amount;
                    potion_slot_inv.stack_amount -= quick_slots_potions[quick_slot_number].max_stack_amount;
                }
            }
            else if(quick_slots_potions[quick_slot_number].item.item_id == potion_slot_inv.item.item_id){
                if(quick_slots_potions[quick_slot_number].stack_amount + potion_slot_inv.stack_amount <= quick_slots_potions[quick_slot_number].max_stack_amount){
                    quick_slots_potions[quick_slot_number].stack_amount += potion_slot_inv.stack_amount;
                    if(slot_in_playyer_inv)
                        Remove_item_from_player_inv(potion_slot_inv);
                    else
                        _inventories.object_inv_to_show.Remove_item_from_object(potion_slot_inv.slot_number);
                    potion_slot_inv.stack_amount = 0;
                    potion_slot_inv.item = null;
                    _inventories.Move_slots_to_left_inv(potion_slot_inv,amount_of_items_slots);
                }
                else{
                    potion_slot_inv.stack_amount -= (quick_slots_potions[quick_slot_number].max_stack_amount - quick_slots_potions[quick_slot_number].stack_amount);
                    quick_slots_potions[quick_slot_number].stack_amount = quick_slots_potions[quick_slot_number].max_stack_amount;
                }
            }
            else if(quick_slots_potions[quick_slot_number].item != null && inventory_potions_slots.Count < amount_of_items_slots){
                
                _inventories.Add_item_to_player_inv_check_for_same_object(quick_slots_potions[quick_slot_number].item,quick_slots_potions[quick_slot_number].stack_amount);
               //Add_item_to_player_inv_list(quick_slots_potions[slot_number].item);
                quick_slots_potions[quick_slot_number].item = potion_slot_inv.item;
                //same as above
                if(quick_slots_potions[quick_slot_number].max_stack_amount >= potion_slot_inv.stack_amount){
                    quick_slots_potions[quick_slot_number].stack_amount = potion_slot_inv.stack_amount;
                    if(slot_in_playyer_inv)
                        Remove_item_from_player_inv(potion_slot_inv);
                    else
                        _inventories.object_inv_to_show.Remove_item_from_object(potion_slot_inv.slot_number);
                    potion_slot_inv.stack_amount = 0;
                    potion_slot_inv.item = null;
                    _inventories.Move_slots_to_left_inv(potion_slot_inv,amount_of_items_slots);
                }
                else{
                    quick_slots_potions[quick_slot_number].stack_amount = quick_slots_potions[quick_slot_number].max_stack_amount;
                    potion_slot_inv.stack_amount -= quick_slots_potions[quick_slot_number].max_stack_amount;
                }
            }
            else if(quick_slots_potions[quick_slot_number].item != null && inventory_potions_slots.Count == amount_of_items_slots){ 
                temp_potion = (Potions)quick_slots_potions[quick_slot_number].item;
                int temp_stack_amount = quick_slots_potions[quick_slot_number].stack_amount;
                quick_slots_potions[quick_slot_number].item = potion_slot_inv.item;
                if(quick_slots_potions[quick_slot_number].max_stack_amount >= potion_slot_inv.stack_amount){
                    quick_slots_potions[quick_slot_number].stack_amount = potion_slot_inv.stack_amount;
                    if(slot_in_playyer_inv)
                        Remove_item_from_player_inv(potion_slot_inv);
                    else
                        _inventories.object_inv_to_show.Remove_item_from_object(potion_slot_inv.slot_number);
                    potion_slot_inv.stack_amount = 0;
                    potion_slot_inv.item = null;
                    _inventories.Move_slots_to_left_inv(potion_slot_inv,amount_of_items_slots);
                    //Add_item_to_player_inv_list(temp_potion);
                    _inventories.Add_item_to_player_inv_check_for_same_object(temp_potion,temp_stack_amount);
                }
                else{
                    _inventories.remaining_item_amount = 0;
                    _inventories.Add_item_to_player_inv_check_for_same_object(temp_potion,temp_stack_amount);
                    if(_inventories.remaining_item_amount == 0){
                        quick_slots_potions[quick_slot_number].stack_amount = quick_slots_potions[quick_slot_number].max_stack_amount;
                        potion_slot_inv.stack_amount -= quick_slots_potions[quick_slot_number].max_stack_amount;
                    }
                    else{
                        quick_slots_potions[quick_slot_number].stack_amount -= _inventories.remaining_item_amount; 
                    }
                }
            }
        }
        public void Use_potion_from_quick_slot(int slot_number){
            if(quick_slots_potions[slot_number].item == null)
                return ;
            else{
                quick_slots_potions[slot_number].stack_amount -= 1;
                Health_potion health_potion = (Health_potion) quick_slots_potions[slot_number].item;
                GetComponent<Player_info>().Start_healing_player_process(health_potion.efect_duration,health_potion.heal_amount);
                if(quick_slots_potions[slot_number].stack_amount == 0){
                    quick_slots_potions[slot_number].item = null;
                    _inventories.player_slots.Update_quick_slot_potions_icon(empty_potion,slot_number);
                }
            }
        }
        public void Poison_weapon(Poison_potion poison_potion,bool right_weapon){
            if(right_weapon){
                if( current_weapon_for_right_hand.isPoisoned)
                    _poison_timer_right_weapon = 0;
                current_weapon_for_right_hand.isPoisoned = true;
                current_weapon_for_right_hand.poison_damage = poison_potion.poison_damage;
                current_weapon_for_right_hand.poison_duration = poison_potion.poison_duration;
                _poison_time_right_weapon = poison_potion.efect_duration;
            }
            else{
                if( current_weapon_for_left_hand.isPoisoned)
                    _poison_timer_left_weapon = 0;
                current_weapon_for_left_hand.isPoisoned = true;
                current_weapon_for_left_hand.poison_damage = poison_potion.poison_damage;
                current_weapon_for_left_hand.poison_duration = poison_potion.poison_duration;
                _poison_time_left_weapon = poison_potion.efect_duration;
            }
        }
        public void Handle_inventory(){
            if(inventory_open){
                //close inventory
                _input_handler.Switch_action_map_to_player();  
                _inventories.Hide_change_inv_buttons();
                _inventories.Hide_player_inventory();
                _inventories.Hide_crafting_menu();
                Cursor.lockState = CursorLockMode.Locked;
                inventory_open = false;
                _cinemachine_camera.m_XAxis.m_MaxSpeed = _cinemachine_camera_x_sensitivity;
                _cinemachine_camera.m_YAxis.m_MaxSpeed = _cinemachine_camera_y_sensitivity;
            }
            else{
                _animator.SetFloat("Speed_percent",-1f);
                _player_movement.Stop_player();
                _input_handler.Switch_action_map_to_inv();
                //open inventory
                _inventories.Show_player_inventory();
                _inventories.Show_change_inv_buttons();
                _inventories.Get_player_inv_slots().Update_inventory_player_UI(this);
                Cursor.lockState = CursorLockMode.Confined;
                inventory_open = true;
                _cinemachine_camera_x_sensitivity = _cinemachine_camera.m_XAxis.m_MaxSpeed;
                _cinemachine_camera_y_sensitivity = _cinemachine_camera.m_YAxis.m_MaxSpeed;
                _cinemachine_camera.m_XAxis.m_MaxSpeed = 0.0f;
                _cinemachine_camera.m_YAxis.m_MaxSpeed = 0.0f;
            }
        }
        public void Remove_weapon_from_hand_slot(bool isRight){
             if(isRight){
                if(current_weapon_for_right_hand != unarmed)
                    _inventories.Add_item_to_player_inv_check_for_same_object(current_weapon_for_right_hand,1);
                else if(backup_weapon_right != unarmed)
                    _inventories.Add_item_to_player_inv_check_for_same_object(backup_weapon_right,1);
                if(_inventories.added_all_items){
                    current_weapon_for_right_hand = backup_weapon_right = unarmed;
                    weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_right_hand,true);
                }
            }
            else{
                if(current_weapon_for_left_hand != unarmed)
                    _inventories.Add_item_to_player_inv_check_for_same_object(current_weapon_for_left_hand,1);
                else if(backup_weapon_left != unarmed)
                    _inventories.Add_item_to_player_inv_check_for_same_object(backup_weapon_left,1);
                if(_inventories.added_all_items){
                    current_weapon_for_left_hand = backup_weapon_left = unarmed;
                    weapon_slot_manager.Load_weapon_to_slot(current_weapon_for_left_hand,false);
                }
               
            }
        }
        public void Remove_potion_from_quick_slot(int slot_number){
            if(quick_slots_potions[slot_number].item == null)
                return ;
            else{
                _inventories.Add_item_to_player_inv_check_for_same_object(quick_slots_potions[slot_number].item,quick_slots_potions[slot_number].stack_amount);
                if(_inventories.added_all_items){
                    quick_slots_potions[slot_number].item = null;
                    quick_slots_potions[slot_number].stack_amount = 0;
                    _inventories.player_slots.Update_quick_slot_potions_icon(empty_potion,slot_number);
                }
            }
        }
        public void Remove_item_from_player_inv(Slot slot){
            if(slot.item is Weapon_info.Weapon)
                inventory_weapons_slots.RemoveAt(slot.slot_number);
            else if(slot.item is Potions)
                inventory_potions_slots.RemoveAt(slot.slot_number);
            else if(slot.item is Materials){
                inventory_materials_slots.RemoveAt(slot.slot_number);
            }  
            else if(slot.item is Item_info.Item){
                inventory_items_slots.RemoveAt(slot.slot_number);
            }
        }
        public void Add_item_to_player_inv_list(Item_info.Item item){
            if(item is Weapon_info.Weapon && inventory_weapons_slots.Count < 6){
                inventory_weapons_slots.Add((Weapon_info.Weapon)item);
            }
            else if(item is Potions && inventory_potions_slots.Count < 6){
                inventory_potions_slots.Add((Potions) item);
            }   
            else if(item is Materials && inventory_materials_slots.Count < 6){
                inventory_materials_slots.Add((Materials) item);
            }
            else if(item is Item_info.Item && inventory_items_slots.Count < 6){
                inventory_items_slots.Add(item);
            }   
        }
    }
}