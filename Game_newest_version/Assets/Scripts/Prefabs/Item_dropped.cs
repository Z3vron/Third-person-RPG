using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_dropped : MonoBehaviour
{
    
    public string interactable_text = "pick up items";

    //public Weapon_info.Weapon weapon_dropped;
    public Item_info.Item item_dropped;
    public int amount_of_dropped_items;
    
    [SerializeField]
    private float _time_to_destroy_projectile = 45;
    private Player_Movemnet.Movement movement_to_destroy_reference_in_list_of_object_to_interact;
    void Start(){
        Destroy(gameObject,_time_to_destroy_projectile);
        // if(movement_to_destroy_reference_in_list_of_object_to_interact.trigger_colliders.Contains(gameObject.GetComponent<Collider>()))
        //     Function_timer.Create(movement_to_destroy_reference_in_list_of_object_to_interact.trigger_colliders.Remove(gameObject.GetComponent<Collider>()),_time_to_destroy_projectile));
    }
}
