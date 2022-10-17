using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Function_timer:MonoBehaviour{
    private float _timer;
    private bool _isDestroyed;
    private GameObject _gameobject;
    private Action _action;
    public static Function_timer Create(Action action, float timer){
        GameObject gameObject = new GameObject("Timer");
        gameObject.AddComponent(typeof(Function_timer));
        Function_timer function_timer = gameObject.GetComponent<Function_timer>();//new Function_timer(action,timer,gameObject);
        function_timer.Set_function_timer(action,timer,gameObject);
        return function_timer;
    }
    private void Set_function_timer(Action action,float timer,GameObject gameobject){
        this._action = action;
        this._timer = timer;
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
        UnityEngine.Object.Destroy(_gameobject);
    }
}