using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_over : MonoBehaviour{

    public float restart_delay = 10f;

    private Animator _animator;
    private float _restart_timer;

    private void Start() {
        _animator = GetComponent<Animator>();
        Player_info.Player_death += Show_death_UI;
        Player_info.Player_death += Restart_level_begin;
    }
    private void Show_death_UI(){
        _animator.SetTrigger("GameOver");
    }
    private void Restart_level_begin(){
        Function_timer.Create( Restart_level,5); 
    }
    private void Restart_level(){
        SceneManager.LoadScene("Village");
    }
    // private void Update(){

    //     if(player.player_stats.Current_health <=0){
    //         _animator.SetTrigger("GameOver");
    //         // Destroy(Player.gameObject);
    //         _restart_timer += Time.deltaTime;

    //         if(_restart_timer >= restart_delay ){
    //             SceneManager.LoadScene("Scene_01");
               
    //         }
    //     }

    // }


}
