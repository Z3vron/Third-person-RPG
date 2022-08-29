using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Armor_info{
    //makes new type type listed in assets/create submenu
    [CreateAssetMenu(menuName = "Items/Armor Item")]
    public class Armour : Item_info.Item{
        public GameObject Chest_model;
        public GameObject Helmet_model;
        public GameObject Legs_model;

        public float Chest_armour;
        public float Helmet_armour;
        public float Legs_armour;

        


   }
}

