using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon_info{
    //makes new type type listed in assets/create submenu
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class Weapon : Item_info.Item{
        public GameObject Weapon_model;
        public float durability;
        public float material_toughness;
        public bool unarmed;
        public int weapon_lvl;
        [Header("Poison efect")]
        public float poison_damage;
        public float poison_duration;
        public bool isPoisoned = false;

        [Header("Damages values")]
        public float Light_attack_damage;
        public float Strong_attack_damage;
        public float combo_dmg_bonus;

        [Header("Stamina costs")]
        public float light_attack_stamina_cost;
        public float strong_attack_stamina_cost;
        public float combo_attack_stamina_cost;
       
        public void Remove_durability(float durability_value_to_remove){
            durability -= durability_value_to_remove/material_toughness;
            //to catch some strange small numbers unsure of their origin in some situations
            if(durability<0.0001)
                durability = 0;
        }
        public void Add_durability(float durability_value_to_add){
            if(durability + durability_value_to_add/material_toughness < 100)
                durability += durability_value_to_add/material_toughness;
            else
                durability = 100;
        }
        public void Reset_durability(){
            durability = 100;
        }
   }
}

