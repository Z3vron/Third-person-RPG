using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_transform : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;

    private void Start() {
        _controller = GetComponentInParent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    private void OnAnimatorMove() {
        if(_animator.GetBool("Movement_driven_by_animation")){
            Debug.Log("test");
            Vector3 velocity = _animator.deltaPosition;
            velocity.y = 0;
            _controller.Move(velocity);
        }
    }
}
