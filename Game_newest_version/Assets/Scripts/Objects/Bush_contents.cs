using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush_contents : MonoBehaviour{
  public string interactable_text;
  public Berries berry_in_bush;
  public int amount_of_berry;
  public float time_to_respawn_new_berries;

  private int _max_amount_of_berry;
  private float _timer;
  private void Start() {
    _max_amount_of_berry = amount_of_berry;
  }
  private void Update() {
    if(amount_of_berry < _max_amount_of_berry){
      _timer += Time.deltaTime;
      if(_timer >= time_to_respawn_new_berries){
        amount_of_berry = _max_amount_of_berry;
        _timer = 0f;
      }
    }
  }
}
