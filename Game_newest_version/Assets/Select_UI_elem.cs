using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Select_UI_elem : MonoBehaviour, IDeselectHandler{
    public void OnSelect (BaseEventData data){
		Debug.Log ("Selected");
	}
    public void OnDeselect (BaseEventData data){
		Debug.Log ("Deselected");
	}
}
