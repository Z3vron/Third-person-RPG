using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    
    private Rigidbody _Trap_rigidbody;
    [SerializeField]
    [Tooltip("Force with which trap will go up")]
    private float _Trap_force = 2f;
    [SerializeField]
    private GameObject _Player;
    void Start(){
        _Trap_rigidbody = GetComponent<Rigidbody>();
    }
    void Update(){
        if(_Player != null){
            bool activated =  _Player.GetComponent<Player_Movemnet.Movement>().Trap_active;     
            if(activated){
                _Trap_rigidbody.AddForce(Vector3.up * _Trap_force);
            //_Trap_rigidbody.AddExplosionForce(_Trap_force,_Trap_rigidbody.position,10f); // starting even if the condition is not met
            }
        }
        
        
    }
    
    
}
