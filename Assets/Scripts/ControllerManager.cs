/* This class is responsible for handling connected gamepads */

using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour
{
     private List<Gamepad> controllerList = new List<Gamepad>(); // List of connect gamepads
     public static int controllerCount = 0; // count of connected gamepads

     private void Awake()
     {
          SetControllerList();
          UpdateControllerCount();
     }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {
          CheckScene(SceneManager.GetActiveScene().name);
     }

     // awake
     // update the list of connected gamepads
     private void SetControllerList()
     {
          if (controllerList != null)
          {
               foreach (Gamepad controller in Gamepad.all)
               {
                    if (controllerList.Count == PlayerManager.MAX_PLAYERS)
                    {
                         break;
                    }
                    else
                    {
                         controllerList.Add(controller);
                         Debug.Log("SetControllerList() just added " + controller);
                    }
               }
          }
          else
          {
               Debug.Log("SetControllerList() failed, the list was null!");
          }
     }

     // get the list of connected gamepads
     private List<Gamepad> GetControllerList()
     {
          return controllerList;
     }

     // awake
     // update the count of connected game pads
     private void UpdateControllerCount()
     {
          ControllerManager.controllerCount = controllerList.Count;
          //Debug.Log("Number of gamepads detected: " + controllerCount);
     }

     private void CheckScene(string sceneName)
     {
          //Debug.Log("From ControllerManager, the current scene is " + sceneName);
          while (sceneName.Equals("ReadUpScene"))
          {
               SetControllerList();
               UpdateControllerCount();
          }
               
     }

}
