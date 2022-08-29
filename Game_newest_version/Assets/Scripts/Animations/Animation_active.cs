using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_active : StateMachineBehaviour{
    public string Targeted_bool;
    public bool status;
    override public  void  OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.SetBool(Targeted_bool,status);
    }
}
