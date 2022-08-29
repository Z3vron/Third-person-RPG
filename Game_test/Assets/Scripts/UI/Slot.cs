using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory Slot")]
public class Slot : ScriptableObject{
    public int slot_number;
    public int  stack_amount;
    public int max_stack_amount;
    public bool in_inventory;
    public bool isRight;
    public bool in_obj_inv;
    public Item_info.Item item;
}
