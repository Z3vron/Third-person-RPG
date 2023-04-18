using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_measurer : MonoBehaviour{
    public float effect_duration;
    public int effect_index;
    public string effect_name;

    private float _effect_timer = 0;
    private Image _filler;

    private void Start() {
        _filler = GetComponent<Image>();
    }
    private void Update() {
        _effect_timer += Time.deltaTime;
        _filler.fillAmount -= 1/effect_duration * Time.deltaTime;
        if(_effect_timer >= effect_duration){
            GetComponentInParent<Effects_indications>().Remove_effect(effect_index);
            Destroy(gameObject);
        }
    }
    public void Restart_effect(){
        _effect_timer = 0f;
        _filler.fillAmount = 1;
    }

}
