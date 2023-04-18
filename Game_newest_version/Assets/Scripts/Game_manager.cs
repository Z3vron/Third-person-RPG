using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public  class Game_manager : MonoBehaviour{
   public static Game_manager Instance {get; private set;}
   [field: SerializeField] public Player_info player_info {get; private set;}
   [field: SerializeField] public Player_inventory_info.Player_inventory player_inventory {get; private set;}
   [field: SerializeField] public Player_Movemnet.Movement player_movement {get; private set;}
   [field: SerializeField] public Input_handler input_handler {get; private set;}
   
   [SerializeField] private CinemachineFreeLook _cinemachine_camera;
   #region Variables used to store camera values to restore them after freezing camera for browsing inventory
      private float _cinemachine_camera_x_sensitivity;
      private float _cinemachine_camera_y_sensitivity;
   #endregion
   private bool isPaused = false;
   private void Start() {
      Input_handler.Pause_game += Pause_game_timer;
   }
   
   private void Awake() {
    Instance = this;
   }
   private void Pause_game_timer(){
      if(isPaused){
         Time.timeScale = 1;
         Unlock_camera_and_mouse();
         isPaused = false;
      }
      else{
         Time.timeScale = 0;
         Lock_camera_and_mouse();
         isPaused = true;
      }
         
   }
   public void Lock_camera_and_mouse(){
      Cursor.lockState = CursorLockMode.Confined;
      _cinemachine_camera_x_sensitivity = _cinemachine_camera.m_XAxis.m_MaxSpeed;
      _cinemachine_camera_y_sensitivity = _cinemachine_camera.m_YAxis.m_MaxSpeed;
      _cinemachine_camera.m_XAxis.m_MaxSpeed = 0.0f;
      _cinemachine_camera.m_YAxis.m_MaxSpeed = 0.0f;
   }
   public void Unlock_camera_and_mouse(){
      Cursor.lockState = CursorLockMode.Locked;
      _cinemachine_camera.m_XAxis.m_MaxSpeed = _cinemachine_camera_x_sensitivity;
      _cinemachine_camera.m_YAxis.m_MaxSpeed = _cinemachine_camera_y_sensitivity;
   }
}
