using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Character Statistics/Enemy Statistics")]
public class Enemy_statistics : Character_statistics{

    public string character_name;
    public int exp_reward;
    public Enemy_HUD enemy_HUD_to_this_stats;
    public static event Action<float,Enemy_HUD> Changed_enemy_hp_UI;
    public override void Invoke_change_character_hp_UI_event(){
      Changed_enemy_hp_UI?.Invoke(Current_health/Max_health,enemy_HUD_to_this_stats);
   }
}
