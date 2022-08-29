using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character Statistics/Player Statistics")]
public class Player_statistics : Character_statistics{
   public int current_exp;
   public int exp_to_next_level = 100;
   private void Start() {
    current_exp = 0;
    exp_to_next_level = 100;
   }
}
