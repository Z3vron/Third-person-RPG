using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush_contents : MonoBehaviour{
  public string interactable_text;
  public Berries berry_in_bush;
  public int amount_of_berry;
  public bool start_timer;

  private int _max_amount_of_berry;
  private float _timer;
  private void Start() {
    amount_of_berry = _max_amount_of_berry;
  }
  private void Update() {

    _timer += Time.deltaTime;
  }
}
