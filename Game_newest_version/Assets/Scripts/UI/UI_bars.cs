using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI_elements{
    public class UI_bars : MonoBehaviour{
            
        public Slider Stamina_slider;
        public Image health_bar;
        public Image health_taken_bar;
        public float health_taken_bar_multiplayer = 0.15f;
        public Text exp_level;
        private void Start() {
            Player_statistics.Changed_player_hp_UI += Set_current_health;
            Player_statistics.Changed_player_stamina_UI += Set_current_stamina;
            Player_statistics.Changed_player_lvl_UI += Set_exp_level;
        }

        public void Set_current_health( float Current_health_in_percentage){
            health_bar.fillAmount = Current_health_in_percentage;
        }
        public void Set_current_stamina( float Current_stamina_in_percentage){
            Stamina_slider.value = Current_stamina_in_percentage;
        }
        private void Update() {
            if(health_taken_bar.fillAmount > health_bar.fillAmount)
                health_taken_bar.fillAmount -= Time.deltaTime * health_taken_bar_multiplayer; 
            else if(health_taken_bar.fillAmount < health_bar.fillAmount)
                health_taken_bar.fillAmount = health_bar.fillAmount;
        }
        public void Set_exp_level(int level,int exp_amount, int exp_cap){
            exp_level.text = "Lvl: " + level + "Exp: " + exp_amount + "/" + exp_cap;
        }

    }
}

