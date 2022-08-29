using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_slot_activator : MonoBehaviour{
    public bool activated = false;
    public void Force_activation() {
       // Debug.Log("Activate right slot");
        this.gameObject.SetActive(true);
    }
    private void OnEnable() {
        //Debug.Log("hand enable");
        activated = true;

    }
}
