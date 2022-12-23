using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Game_manager : MonoBehaviour{
   public static Game_manager Instance {get; private set;}
   public Player_info player_info;
   public Player_inventory_info.Player_inventory player_inventory;
   public Player_Movemnet.Movement player_movement;

   private void Awake() {
    Instance = this;
   }
}
