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
    void Start(){
        Destroy(gameObject,_time_to_destroy_projectile);
    }
}
