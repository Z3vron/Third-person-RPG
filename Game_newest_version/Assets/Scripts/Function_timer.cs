using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Function_timer:MonoBehaviour{
    private float _timer;
    private bool _isDestroyed ;
    private GameObject _gameobject;
    private Action _action;
    private string _timer_name;
    private static List<Function_timer> _active_timers = new List<Function_timer>();
    public static Function_timer Create(Action action, float timer,string timer_name = null){
        GameObject gameObject = new GameObject("Timer");
        gameObject.AddComponent(typeof(Function_timer));
        Function_timer function_timer = gameObject.GetComponent<Function_timer>();//new Function_timer(action,timer,gameObject);
        _active_timers.Add(function_timer);
        function_timer.Set_function_timer(action,timer,gameObject,timer_name);
        return function_timer;
    }
    public static bool Find_timer(string timer_name){
        for(int i = 0; i < _active_timers.Count; i++){
            if(_active_timers[i]._timer_name == timer_name)
                return true;
        }
        return false;
    }
    public static void Stop_timer(string timer_name){
        // throwing errors about changing collection
        // foreach(Function_timer timer in _active_timers){
        //     if(timer._timer_name == timer_name){
        //         _active_timers.Remove(timer);
        //         Destroy(timer.gameObject);
        //     }
        // }
        for(int i = 0; i < _active_timers.Count; i++){
            if(_active_timers[i]._timer_name == timer_name)
                _active_timers[i].Auto_destroy();
        }
    }
    private void Set_function_timer(Action action,float timer,GameObject gameobject,string timer_name){
        this._action = action;
        this._timer = timer;
        this._timer_name = timer_name;
        _gameobject = gameobject;
        _isDestroyed = false;
    }
    private void Update(){  
        if(!_isDestroyed && _action != null){
         
            _timer -= Time.deltaTime;
            if(_timer <= 0){
                _action();
                Auto_destroy();
            }
        }
    }
    private void Auto_destroy(){
        _isDestroyed = true;
        _active_timers.Remove(this);
        UnityEngine.Object.Destroy(_gameobject);
    }
}