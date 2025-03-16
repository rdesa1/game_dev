using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
     [SerializeField] private GameObject pointer;
     [SerializeField] private GameObject bullet;
     [SerializeField] private Transform bulletSpawnPoint;

     private GameObject bulletInstance;

     private Vector2 worldPosition;
     private Vector2 direction;

     void Start()
     {
     }

     private void Update()
     {
          HandlePointer();
     }

     private void OnDestroy()
     {
     }

     private void HandlePointer()
     {
          // Get the world position of the mouse
          worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());


          // Calculate the direction from the player to the mouse
          direction = worldPosition - (Vector2)(transform.position); // Calculate the direction from player

          // Normalize the direction to ensure the pointer stays 1 unit away
          direction.Normalize();

          // Set the pointer's position 1 unit away from the player (along the direction)
          pointer.transform.position = (Vector2)(transform.position) + direction;

          // Calculate the angle based on the direction
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          // Rotate the pointer to face the mouse
          pointer.transform.rotation = Quaternion.Euler(0, 0, angle);
     }


     // Fires projectile. Triggered every 4th quarter note by the BeatManager.
     public void HandleShooting()
     {
          Instantiate(bullet, bulletSpawnPoint.position, pointer.transform.rotation);
     }
}
