using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_over : MonoBehaviour{

    public Player_info player;
    public float restart_delay = 10f;

    private Animator _animator;
    private float _restart_timer;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    private void Update(){

        if(player.player_stats.Current_health <=0){
            _animator.SetTrigger("GameOver");
            // Destroy(Player.gameObject);
            _restart_timer += Time.deltaTime;

            if(_restart_timer >= restart_delay ){
                SceneManager.LoadScene("Scene_01");
               
            }
        }

    }


}
