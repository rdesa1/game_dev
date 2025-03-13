using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PreciseMovementWindow : MonoBehaviour
{
     public static bool preciseMovementIsOpen = false;
     public int preciseMoveWindowDuration = 3000;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     //private void LateUpdate()
     //{
     //     CheckPreciseMovementWindow(preciseMovementIsOpen);
     //}

     public void OpenPreciseMovementWindow(bool preciseMovementIsOpen)
     {
          // need to specify the class for global change
          PreciseMovementWindow.preciseMovementIsOpen = true; 
          //Debug.Log("value of preciseMovementIsOpen = " + preciseMovementIsOpen);
     }

     private void CheckPreciseMovementWindow(bool preciseMovementIsOpen)
     {
          //Debug.Log("CheckPreciseMovementWindow() has been called!");
          //Debug.Log("From CheckPreciseMovementWindow: " + preciseMovementIsOpen);
          if (preciseMovementIsOpen == true)
          {
               //Debug.Log("Precise movement window is open");
               Thread.Sleep(preciseMoveWindowDuration);
               PreciseMovementWindow.preciseMovementIsOpen = false;
          }
     }
}


 