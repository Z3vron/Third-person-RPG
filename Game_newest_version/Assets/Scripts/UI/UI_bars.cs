using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI_elements{
    public class UI_bars : MonoBehaviour{
            
       // public Slider Health_slider;
        public Slider Stamina_slider;
        public Image health_bar;
        public Image health_taken_bar;
        public float health_taken_bar_multiplayer = 0.15f;
        public Text exp_level;

        public void Set_current_health( float Current_health){
            health_bar.fillAmount = Current_health;
            //Health_slider.value = Current_health;
        }
        public void Set_current_stamina( float Current_stamina){
            Stamina_slider.value = Current_stamina;
        }
        private void Update() {
            if(health_taken_bar.fillAmount > health_bar.fillAmount)
                health_taken_bar.fillAmount -= Time.deltaTime * health_taken_bar_multiplayer; 
            else if(health_taken_bar.fillAmount <=health_bar.fillAmount)
                health_taken_bar.fillAmount = health_bar.fillAmount;
        }
        public void Set_exp_level(int exp_amount, int exp_cap,int level){
            exp_level.text ="Exp: " + exp_amount + "/" + exp_cap + "   level: " + level;
        }

    }
}

