using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour{

    public abstract State Run_current_state(Enemy_manager eneme_manager);   
}
