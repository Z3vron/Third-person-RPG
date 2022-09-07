using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_transform : MonoBehaviour
{
    public Transform character_direction;
    public Transform animation_transform;
    public bool isInteracting = false;
    private Vector3 _move;
    private CharacterController _cotroller;

    private void Start() {
        _cotroller = GetComponent<CharacterController>();
    }
    private void Update() {
        isInteracting = GetComponent<Player_info>().active_animation;
        if(isInteracting){
            _move = new Vector3(animation_transform.localPosition.x,0,animation_transform.localPosition.z);
            _move = _move.z * character_direction.forward + _move.x * character_direction.right;;
           // Debug.Log("Moving player: " + _move.x + _move.y + _move.z);
            _cotroller.Move(_move);
        //     Player_transform.localPosition += Animation_transform.transform.localPosition;
        //    Animation_transform.transform.localPosition = new Vector3(0,0,0);
           //Debug.Log("Position:" + Animation_transform.transform.localPosition);
        }
        animation_transform.localPosition = new Vector3(0,0,0);
        animation_transform.localRotation = new Quaternion(0,0,0,0);
    }
}
