using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_opening : MonoBehaviour
{
    /*
    [SerializeField]
    private GameObject _Player;
    [SerializeField]
    private float _Touch_force = 80f;
    private Rigidbody _Door_rigidbody;
    [SerializeField]
    private Transform _Character_direction;
    [SerializeField]
    private bool _Door_to_push;
  
    
    void Start(){
        _Door_rigidbody = GetComponent<Rigidbody>();
        _Character_direction = _Player.GetComponent<Player_Movemnet.Movement>().Root_for_shot_raycast;
    }

    // Update is called once per frame
    void Update(){
        
            bool activated = _Player.GetComponent<Player_Movemnet.Movement>().Door_touched;
            if(activated){
                Debug.Log("Opening door");
                if(_Door_to_push){
                    _Door_rigidbody.AddForce( _Character_direction.forward * _Touch_force,ForceMode.Acceleration);
                }
                else{
                     _Door_rigidbody.AddForce( -_Character_direction.forward * _Touch_force,ForceMode.Acceleration);
                }
               
                _Player.GetComponent<Player_Movemnet.Movement>().Door_touched = false;
            }
        
        
        
    }
    */
}
