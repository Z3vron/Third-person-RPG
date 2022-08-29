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
    }
}

