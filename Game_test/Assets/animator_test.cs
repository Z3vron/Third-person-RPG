using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animator_test : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator _animator;
    public float _enemy_speed = 10;
    // private void OnAnimatorMove() {
    //     Debug.Log("fuck");
    //     Vector3 velocity = _animator.deltaPosition/Time.deltaTime;
    //     rigidbody.velocity = velocity * _enemy_speed;
    // }
}