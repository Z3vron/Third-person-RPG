using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HUD : MonoBehaviour{
    //could transfer part of logic into this script from enemy but not sure if worth it
    [SerializeField] private float _health_taken_bar_multiplayer = 0.15f;
    [SerializeField] private Image _health_bar;
    [SerializeField] private Image _health_taken_bar;

    private void Update() {
        if(_health_taken_bar.fillAmount > _health_bar.fillAmount)
            _health_taken_bar.fillAmount -= Time.deltaTime * _health_taken_bar_multiplayer; 
        else if(_health_taken_bar.fillAmount <= _health_bar.fillAmount)
            _health_taken_bar.fillAmount = _health_bar.fillAmount;
    }
    public void Set_health_bar_fill_amount(float value){
        _health_bar.fillAmount = value;
    }
}
