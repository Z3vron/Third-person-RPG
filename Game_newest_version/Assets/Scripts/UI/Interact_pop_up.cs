
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class _interact_pop_up : MonoBehaviour{
    private text _interact_pop_up_text;
    private string _back_up_interact_text;
    private bool _text_added = false;

    private void Start(){
        //maybe do it in awake
      _interact_pop_up_text =  GetComponentInChildren<Text>().text
      _back_up_interact_text = _interact_pop_up_text;   
    }
    public void Set_interact_pop_up(text text_to_put_in_pop_up){
        if(!_text_added){
            _interact_pop_up_text += text_to_put_in_pop_up;
            _text_added = true;
        }     
    }
    public void Reset_interact_pop_up(){
        _interact_pop_up_text = _back_up_interact_text;
        _text_added = false;
        gameObject.SetActive(false);

    }
}