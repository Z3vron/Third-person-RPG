using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player_attack")]
public class Player_attacks : ScriptableObject{
    public string attack_name;

    [Header("External requirements to use this attack")]
    public bool require_left_weapon;
    public bool require_right_weapon;
    public bool require_one_of_two_weapon;
}
