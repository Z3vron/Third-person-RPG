using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HUD : MonoBehaviour{
    //could transfer part of logic into this script from enemy but not sure if worth it
    [SerializeField] private float _health_taken_bar_multiplayer = 0.15f;
    [SerializeField] private Image _health_bar;
    [SerializeField] private Image _health_taken_bar;
    [SerializeField] private GameObject _poisoned_indication;
    private Image _poisoned_filler;
    private bool _enemy_poisoned = false;
    private float _posion_duration;
    private void Start() {
        _poisoned_filler = _poisoned_indication.GetComponentInChildren<Image>();
    }
    private void Update() {
        if(_health_taken_bar.fillAmount > _health_bar.fillAmount)
            _health_taken_bar.fillAmount -= Time.deltaTime * _health_taken_bar_multiplayer; 
        else if(_health_taken_bar.fillAmount <= _health_bar.fillAmount)
            _health_taken_bar.fillAmount = _health_bar.fillAmount;
        
        if(_enemy_poisoned){
            _poisoned_filler.fillAmount -= 1/_posion_duration * Time.deltaTime;
        }
    }
    public void Set_health_bar_fill_amount(float value){
        _health_bar.fillAmount = value;
    }
    public void Start_poisoned(float poison_duration){
        _poisoned_indication.SetActive(true);
        _enemy_poisoned = true;
        _posion_duration = poison_duration;
        _poisoned_filler.fillAmount = 1;
    }
    public void End_poisoned(){
        _poisoned_indication.SetActive(false);
    }
}
