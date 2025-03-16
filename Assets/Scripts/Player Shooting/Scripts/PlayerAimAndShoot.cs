using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
     [SerializeField] private GameObject pointer;
     [SerializeField] private GameObject bullet;
     [SerializeField] private Transform bulletSpawnPoint;

     private Vector2 worldPosition;
     private Vector2 direction;
     private bool usingController = false; // Track input type

     void Update()
     {
          HandlePointer();
     }

     private void HandlePointer()
     {
          if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
          {
               usingController = true; // Switch to controller input
          }
          else if (Mouse.current.delta.ReadValue().sqrMagnitude > 0)
          {
               usingController = false; // Switch to mouse input
          }

          if (usingController && Gamepad.current != null)
          {
               Vector2 stickInput = Gamepad.current.rightStick.ReadValue();

               if (stickInput.magnitude > 0.1f) // Dead zone to prevent drift
               {
                    direction = stickInput.normalized;
               }
          }
          else
          {
               // Get the world position of the mouse
               worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
               direction = worldPosition - (Vector2)(transform.position); // Calculate direction from player
               direction.Normalize();
          }

          // Set the pointer's position 1 unit away from the player (along the direction)
          pointer.transform.position = (Vector2)(transform.position) + direction;

          // Calculate the angle based on the direction
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          // Rotate the pointer to face the target
          pointer.transform.rotation = Quaternion.Euler(0, 0, angle);
     }

     // Fires projectile. Triggered every 4th quarter note by the BeatManager.
     public void HandleShooting()
     {
          Instantiate(bullet, bulletSpawnPoint.position, pointer.transform.rotation);
     }
}
