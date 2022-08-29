using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon_holder{
    public class Weapon_armed : MonoBehaviour{

        public Weapon_info.Weapon current_weapon;
        [SerializeField] private  Transform _hand;
        public bool left_hand = false;
        public bool right_hand=false;
        public GameObject  current_Weapon_model_instantiated;

        public void Equip_weapon(Weapon_info.Weapon Weapon_to_equip){
            if(Weapon_to_equip == null){
                Debug.Log("No weapon to equip");
                return;
            }
            else{
                Unequip_weapon();
            //    //Debug.Log("Instantiate weapon");
                GameObject weapon = Instantiate(Weapon_to_equip.Weapon_model,_hand) as GameObject;
                if(weapon.GetComponent<Activate_pivots.Activate_pivots>() != null){
                    weapon.GetComponent<Activate_pivots.Activate_pivots>().isRight = right_hand;
                    //Debug.Log("weapon armed: " + weapon.GetComponent<Activate_pivots.Activate_pivots>().Left_Pivot.GetComponent<Hand_slot_activator>().activated);
                }   
                //weapon.transform.SetParent(_hand);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                weapon.transform.localScale = Vector3.one;
                current_weapon = Weapon_to_equip;
                current_Weapon_model_instantiated = weapon;
            }
        }
        public void Unequip_weapon(){
            if(current_weapon != null){
                Destroy(current_Weapon_model_instantiated);
            }
        }



    }
}


