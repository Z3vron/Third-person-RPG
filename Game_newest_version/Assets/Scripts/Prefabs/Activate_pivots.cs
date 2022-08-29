using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Activate_pivots{
    public class Activate_pivots : MonoBehaviour{
        public GameObject Right_Pivot;
        public GameObject Left_Pivot;
        public  bool isRight;
        

        private void Start() {
            if(isRight){
                //Debug.Log("Right slot");
                Right_Pivot.GetComponent<Hand_slot_activator>().Force_activation();
            }
            else {
                //Debug.Log("Left slot");
                Left_Pivot.GetComponent<Hand_slot_activator>().Force_activation();
                //Debug.Log( Left_Pivot.GetComponent<Hand_slot_activator>().activated);
                
            }
                
        }
    }
}

