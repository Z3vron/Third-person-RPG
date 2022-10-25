using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item_info{
    public class Item : ScriptableObject{
        //display info in Inspector
        [Header("Item information")]
        // Sprite vs Image, image - canvas - more functionality - used for UI - difference?
        public Sprite Item_icon;
        public string Item_name;
        public int max_stack_amount; 
        public float Item_weight; 
        [Tooltip("Item id used to recognise if the elements are the same - could be stackable - Weapons 0-20, Armour 20-40, Potions 40-60, Items 60-80,Materials 80-100")]
        public string item_id;
    }
}

