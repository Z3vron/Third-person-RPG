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
    private float _poison_duration;
    private void Start() {
        _poisoned_filler = _poisoned_indication.GetComponentInChildren<Image>();
        Enemy_statistics.Changed_enemy_hp_UI += Set_health_bar_fill_amount;
    }
    private void Update() {
        if(_health_taken_bar.fillAmount > _health_bar.fillAmount)
            _health_taken_bar.fillAmount -= Time.deltaTime * _health_taken_bar_multiplayer; 
        else if(_health_taken_bar.fillAmount < _health_bar.fillAmount)
            _health_taken_bar.fillAmount = _health_bar.fillAmount;
        if(_enemy_poisoned){
            _poisoned_filler.fillAmount -= 1/_poison_duration * Time.deltaTime;
        }
    }
    public void Set_health_bar_fill_amount(float value,Enemy_HUD enemy_HUD_to_compare_to_stats){
        if(enemy_HUD_to_compare_to_stats == this)
            _health_bar.fillAmount = value;
    }
    public void Start_poisoned(float poison_duration){
        _poisoned_indication.SetActive(true);
        _enemy_poisoned = true;
        _poison_duration = poison_duration;
        _poisoned_filler.fillAmount = 1;
    }
    public void End_poisoned(){
        _poisoned_indication.SetActive(false);
        _enemy_poisoned = false;
    }
}
