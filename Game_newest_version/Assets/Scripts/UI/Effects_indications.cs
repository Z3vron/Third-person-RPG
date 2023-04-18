using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effects_indications : MonoBehaviour{
    [SerializeField] private GameObject _effect_indicator_prefab;
    private GameObject _instance_effect_indicator;
    private List<GameObject> _effect_indicators_active = new List<GameObject>();
    private void Start() {
        Player_info.Enable_potion_buff_icon += Turn_on_effect_icon;
    }
    private void Update() {
    }
    
    private void Turn_on_effect_icon(float effect_duration,Sprite effect_item_icon,string effect_name){
        foreach(var effect in _effect_indicators_active){
            if(effect.GetComponent<Effect_measurer>().effect_name == effect_name){
                effect.GetComponent<Effect_measurer>().Restart_effect();
                return;
            }
        }
        _instance_effect_indicator = Instantiate(_effect_indicator_prefab,new Vector3(0,0,0),new Quaternion(0,0,0,0)) as GameObject;
        _instance_effect_indicator.GetComponent<RectTransform>().position += new Vector3(470 + 100*_effect_indicators_active.Count,175,0);       
        _instance_effect_indicator.transform.SetParent(gameObject.transform);
        _instance_effect_indicator.GetComponentsInChildren<Image>()[1].sprite = effect_item_icon;
        _instance_effect_indicator.GetComponentsInChildren<Image>()[0].fillAmount = 1;
        _instance_effect_indicator.GetComponent<Effect_measurer>().effect_duration = effect_duration;
        _instance_effect_indicator.GetComponent<Effect_measurer>().effect_index = _effect_indicators_active.Count;
        _instance_effect_indicator.GetComponent<Effect_measurer>().effect_name = effect_name;
        _effect_indicators_active.Add(_instance_effect_indicator);

        
    }
    public void Remove_effect(int effect_index){
        _effect_indicators_active.RemoveAt(effect_index);
        for( int i = effect_index;i < _effect_indicators_active.Count;i++){
            _effect_indicators_active[i].GetComponent<RectTransform>().position += new Vector3(-100,0,0);
        }
        
    }    
}
