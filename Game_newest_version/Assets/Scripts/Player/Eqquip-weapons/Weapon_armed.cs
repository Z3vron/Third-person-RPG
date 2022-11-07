using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon_holder{
    public class Weapon_armed : MonoBehaviour{

        //public float weapon_scale_factor;
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
                //weapon.transform.localScale = new Vector3(1/100,1/100,1/100);
                //Debug.Log(weapon.transform.localScale + " " + weapon.transform.lossyScale);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                //weapon.transform.localScale = Vector3.one;
                //weapon.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
                current_weapon = Weapon_to_equip;
                current_Weapon_model_instantiated = weapon;

                //set scale so that weapon will look good on diffrent models both player and enemy
                //current_Weapon_model_instantiated.transform.localScale = new Vector3(current_Weapon_model_instantiated.transform.localScale.x/weapon_scale_factor,current_Weapon_model_instantiated.transform.localScale.y/weapon_scale_factor,current_Weapon_model_instantiated.transform.localScale.z/weapon_scale_factor);
            }
        }
        public void Unequip_weapon(){
            if(current_weapon != null){
                Destroy(current_Weapon_model_instantiated);
            }
        }
    }
}