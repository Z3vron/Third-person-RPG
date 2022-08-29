using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// public class Test : EventTrigger{
//     Rigidbody rigidbody ;
//     private void Awake() {
//         rigidbody = GetComponent<Rigidbody>();
//     }private void Start() {
//         // Debug.Log(rigidbody.velocity);
//         // rigidbody.velocity = new Vector3(0,0,0);
//     }
//     private void Update() {
//         // rigidbody.velocity = new Vector3(0,0,0);
//         // rigidbody.angularVelocity = new Vector3(0,0,0);
//         //  Debug.Log(rigidbody.velocity);
//     }
//     public override void OnPointerEnter(PointerEventData data){
//         Debug.Log("OnPointerEnter called.");
//     }

    
// }
public class Test : MonoBehaviour{
    Rigidbody rigidbody ;
    public GameObject player;
    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }private void Start() {
        // Debug.Log(rigidbody.velocity);
        // rigidbody.velocity = new Vector3(0,0,0);
    }
    private void Update() {
        // rigidbody.velocity = new Vector3(0,0,0);
        // rigidbody.angularVelocity = new Vector3(0,0,0);
        //  Debug.Log(rigidbody.velocity);
    }
   private void FixedUpdate() {
    if(Vector3.Distance(transform.position,player.transform.position) > 10)
        rigidbody.velocity = transform.forward * 8;
    
    else
        rigidbody.velocity = Vector3.zero;
   }
    
}
