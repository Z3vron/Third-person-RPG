using UnityEngine;
using UnityEngine.UI;
 
public class FPS_counter : MonoBehaviour
{
    private float current = 0;
 
    public void Update ()
    {
        
        current = (int)(1f / Time.unscaledDeltaTime);
        
        Debug.Log("FPS: " + current);
    }
}