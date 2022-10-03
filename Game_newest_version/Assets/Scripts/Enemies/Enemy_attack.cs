using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy actions/Enemy attack")]
public class Enemy_attack : Enemy_action{
    [Tooltip("Higher score = bigger chance that this attack will occure")]
    public int attack_chance_score = 3;
    [Tooltip("Time after this attack to start next  action")]
    public float recovery_time = 2f;
    public float minimum_attack_angle = -35;
    public float maximum_attack_angle = 35;
    public float minimum_distance_to_attack = 0;
    public float maximum_distance_to_attack = 3;
    public float damage = 20;

    [Header("External requirements to use this attack")]
    public bool require_left_weapon;
    public bool require_right_weapon;

}
