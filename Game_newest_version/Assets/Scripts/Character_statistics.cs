using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(menuName = "Character Statistics")]
public class Character_statistics : ScriptableObject{

    [Tooltip("takes armour_effectiveness % less damage")]
    public float armour_effectiveness = 0;
    public float Max_health;
    public float Current_health;
    public float Regenerate_healh_rate;
    public float Regenerate_stamina_rate;
    public float Raw_strength;
    public float Max_stamina;
    public float Current_stamina;
    public bool Taken_dmg = false;
    public bool Taken_stamina = false;
    public bool isDead = false;
    public int level = 0;

    

    public void Set_defaults_stats(float M_hp,float M_s,float R_hp,float R_s,float Strength,int base_level,int current_exp_base_0,int exp_for_level_1){
        Max_health = M_hp;
        Max_stamina = M_s;
        Regenerate_healh_rate = R_hp;
        Regenerate_stamina_rate = R_s;
        Raw_strength = Strength;

        Current_health = Max_health;
        Current_stamina = Max_stamina;
        //current_exp= current_exp_base_0;
        level = base_level;
        //exp_to_next_level = exp_for_level_1;
    }
    private void Update() {
        
    }

    public void Health_regen(){
        if(Current_health < Max_health && Current_health >0){
            Current_health += Regenerate_healh_rate;
        }
    }
    public void Stamina_regen(){
        if(Current_stamina < Max_stamina){
            Current_stamina += Regenerate_stamina_rate;
        }
    }
    public void Take_damage(float damage){
        if(Current_health > 0)
            Current_health -= ( damage * (100 -armour_effectiveness)/100);
        Taken_dmg = true;
    }
    public void Take_damage_bypass_armour(float damage){
        if(Current_health > 0)
            Current_health -= damage;
        Taken_dmg = true;
    }
    public void Take_stamina(float stamina){
        if(Current_stamina > 0){
            Current_stamina -= stamina;
        }
        if(Current_stamina < 0)
            Current_stamina = 0;    
        Taken_stamina = true;
    }
    public void Restore_health(float  health_restore_amount){
        if(Current_health + health_restore_amount < Max_health)
            Current_health += health_restore_amount;
        else
            Current_health = Max_health;
    }
}
