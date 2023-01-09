using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Character Statistics/Player Statistics")]
public class Player_statistics : Character_statistics{
   public int current_exp;
   public int exp_to_next_level = 100;
   
   public static event Action<float> Changed_player_hp_UI;
   public static event Action<float> Changed_player_stamina_UI;
   public static event Action<int,int,int> Changed_player_lvl_UI;
   private void Start() {
    current_exp = 0;
    exp_to_next_level = 100;
   }
   public void Add_exp(int exp_to_add){
      current_exp += exp_to_add;
      if(current_exp >= exp_to_next_level){
         level += 1;
         exp_to_next_level *= 2;
      }
      Invoke_change_player_lvl_UI_event();
   }
   public override void Invoke_change_character_hp_UI_event(){
      Changed_player_hp_UI?.Invoke(Current_health/Max_health);
   }
   public override void Invoke_change_player_stamina_UI_event(){
      Changed_player_stamina_UI?.Invoke(Current_stamina/Max_stamina);
   }
   public void Invoke_change_player_lvl_UI_event(){
      Changed_player_lvl_UI?.Invoke(level,current_exp,exp_to_next_level);
   }
}
