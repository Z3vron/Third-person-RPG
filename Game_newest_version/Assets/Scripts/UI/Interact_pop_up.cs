using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interact_pop_up : MonoBehaviour{
    [SerializeField] private string _interact_pop_up_text;
    [SerializeField] private string _back_up_interact_text;
    private bool _text_added = false;
    private void Awake() {
       // Debug.Log("Awake");
        _interact_pop_up_text =  GetComponentInChildren<Text>().text;
        _back_up_interact_text = _interact_pop_up_text;
    }
    private void OnEnable() {
        //Debug.Log("enable");
    }
      
    public void Set_interact_pop_up(string text_to_put_in_pop_up){
        //Debug.Log("set");
        if(!_text_added){
            GetComponentInChildren<Text>().text += text_to_put_in_pop_up;
            _text_added = true;
        }     
    }
    public void Reset_interact_pop_up(){
       // Debug.Log("reset");
        GetComponentInChildren<Text>().text = _back_up_interact_text;
        _text_added = false;
        gameObject.SetActive(false);
    }
}