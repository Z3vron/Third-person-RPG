using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_active_if : StateMachineBehaviour{
    public string targeted_bool;
    public string if_bool_not;
    public bool new_target_status;
    override public  void  OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex){
        if(if_bool_not != null){
            if(!animator.GetBool(if_bool_not))
                animator.SetBool(targeted_bool,new_target_status);
        }
        else
            animator.SetBool(targeted_bool,new_target_status);
        
    }
}

