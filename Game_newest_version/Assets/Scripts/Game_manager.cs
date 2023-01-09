using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Game_manager : MonoBehaviour{
   public static Game_manager Instance {get; private set;}
   [field: SerializeField] public Player_info player_info {get; private set;}
   [field: SerializeField] public Player_inventory_info.Player_inventory player_inventory {get; private set;}
   [field: SerializeField] public Player_Movemnet.Movement player_movement {get; private set;}

   private void Awake() {
    Instance = this;
   }
}
