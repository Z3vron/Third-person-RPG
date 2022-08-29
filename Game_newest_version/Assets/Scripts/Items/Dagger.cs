using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item/Dagger")]
public class Dagger : Weapon_info.Weapon{
    [Header("Special weapon class values")]
    public float attack_speed_multiplayer;
}
