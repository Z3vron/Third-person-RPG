using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava_damage : MonoBehaviour
{   
    public float lava_dmg = 1.2f;
    //dropping items into lava gives errores that's why I used trygetcomponent and not getcomponent - it's better approach - could/should change it everywhere
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Your stepped  in lava");
       if(other.TryGetComponent(out Player_info player_info)){
        player_info.player_stats.Take_damage(lava_dmg * 5);
       }
    }
    private void OnTriggerStay(Collider other) {
        //Debug.Log("Your are in lava");
        if(other.TryGetComponent(out Player_info player_info)){
            player_info.player_stats.Take_damage(lava_dmg);
        }    
    }
}