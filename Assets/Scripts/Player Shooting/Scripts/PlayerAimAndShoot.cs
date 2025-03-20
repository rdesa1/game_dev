using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
     [SerializeField] private GameObject pointer;
     [SerializeField] private GameObject bullet;
     [SerializeField] private Transform bulletSpawnPoint;

     private Vector2 worldPosition;
     private Vector2 direction = Vector2.right;
     private bool usingController = false; // Track input type

     void Update()
     {
          DetermineKeyboardOrGamepad(GetNumberOfPlayers());
          HandlePointer();
     }

     private int GetNumberOfPlayers()
     {
          return PlayerManager.numberOfPlayers;
     }

     private void DetermineKeyboardOrGamepad(int numberOfPlayers)
     {
          if (numberOfPlayers > 1)
          {
               usingController = true;
          }
     }

     // Callback function for the InputManager
     public void OnAim(InputAction.CallbackContext context)
     {
          Vector2 newDirection = context.ReadValue<Vector2>();

          if (newDirection.magnitude > 0.1f) // Ignore small input (prevents drift)
          {
               direction = newDirection.normalized; // Update direction only when input is present
          }

          Debug.Log($"Player {gameObject.name} received aim input from {context.control.device}");
     }

     private void HandlePointer()
     {
          if (!usingController) // If using mouse, update direction constantly
          {
               worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
               direction = (worldPosition - (Vector2)transform.position).normalized;
          }

          // Keep pointer in last known direction
          pointer.transform.position = (Vector2)transform.position + direction;

          // Calculate the angle based on the direction
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          pointer.transform.rotation = Quaternion.Euler(0, 0, angle);
     }

     // Callback function for the InputManager
     //public void OnAim(InputAction.CallbackContext context)
     //{
     //     direction = context.ReadValue<Vector2>();
     //     Debug.Log($"Player {gameObject.name} received aim input from {context.control.device}");


     //     //if (context.control.device is Gamepad)
     //     //     usingController = true;
     //     //else if (context.control.device is Mouse)
     //     //     usingController = false;
     //}

     //private void HandlePointer()
     //{
     //     if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f)
     //     {
     //          usingController = true; // Switch to controller input
     //     }
     //     else if (Mouse.current.delta.ReadValue().sqrMagnitude > 0)
     //     {
     //          usingController = false; // Switch to mouse input
     //     }

     //     if (usingController && Gamepad.current != null)
     //     {
     //          Vector2 stickInput = Gamepad.current.rightStick.ReadValue();

     //          if (stickInput.magnitude > 0.1f) // Dead zone to prevent drift
     //          {
     //               direction = stickInput.normalized;
     //          }
     //     }
     //     else
     //     {
     //          // Get the world position of the mouse
     //          worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
     //          direction = worldPosition - (Vector2)(transform.position); // Calculate direction from player
     //          direction.Normalize();
     //     }

     //     // Set the pointer's position 1 unit away from the player (along the direction)
     //     pointer.transform.position = (Vector2)(transform.position) + direction;

     //     // Calculate the angle based on the direction
     //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

     //     // Rotate the pointer to face the target
     //     pointer.transform.rotation = Quaternion.Euler(0, 0, angle);
     //}

     // Fires projectile. Triggered every 4th quarter note by the BeatManager.
     public void HandleShooting()
     {
          GameObject bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, pointer.transform.rotation);
          bulletInstance.GetComponent<BulletBehavior>().Initialize(gameObject); // Set shooter
     }
}
