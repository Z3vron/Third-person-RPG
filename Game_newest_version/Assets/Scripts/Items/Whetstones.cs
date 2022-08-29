using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Whetstones")]
public class Whetstones : Item_info.Item{
    [Header("Special whetstone class values")]
    public float durability;
    public float grid_toughness;
    

    public void Remove_durability(float durability_value_to_remove){
        durability -= durability_value_to_remove;
        if(durability<0.0001)
            durability = 0;
    }
    public void Reset_durability(){
        durability = 100;
    }
    private void OnDestroy() {
        Debug.Log("whetstone destroyed");
    }
}
