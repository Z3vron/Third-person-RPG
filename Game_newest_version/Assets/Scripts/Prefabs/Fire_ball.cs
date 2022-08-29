using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_ball : MonoBehaviour
{
    [SerializeField]
    private float _time_to_destroy_projectile = 1.5f;
    private void Start() {
        Destroy(gameObject,_time_to_destroy_projectile);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            other.GetComponent<Enemy_manager>().instance_enemy_stats.Take_damage(50);
            Debug.Log("Hitted enemy");
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
    // private void OnCollisionEnter(Collision other) {
    //     if(other.gameObject.tag == "Enemy"){
    //         Destroy(other.gameObject);
           
    //     }
    //     Destroy(gameObject);
    // }
}
