using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item/Rapier")]
public class Rapier : Weapon_info.Weapon{
    [Header("Special rapier class values")]
    public float attack_speed_multiplayer;
    public float critical_damage_bonus;

}



