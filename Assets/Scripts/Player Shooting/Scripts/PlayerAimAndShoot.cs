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
     private Color bulletColor; // Store the bullet color

     void Start()
     {
          // Set bullet color based on player ID
          PlayerController2D playerProperties = GetComponent<PlayerController2D>();
          if (playerProperties != null)
          {
               bulletColor = GetPlayerColor(playerProperties.playerID);
          }
          else
          {
               bulletColor = Color.white; // Default fallback
          }
     }

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

     public void HandleShooting()
     {
          Debug.Log($"{gameObject.name} fired a bullet!");
          GameObject bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, pointer.transform.rotation);
          bulletInstance.GetComponent<BulletBehavior>().Initialize(gameObject); // Set shooter

          // Set bullet color
          SpriteRenderer bulletRenderer = bulletInstance.GetComponent<SpriteRenderer>();
          if (bulletRenderer != null)
          {
               bulletRenderer.color = bulletColor;
          }
     }

     // Helper function to retrieve player color
     private Color GetPlayerColor(int playerID)
     {
          switch (playerID)
          {
               case 1:
                    return new Color(1f, 0.25f, 0.25f, 1f); // Red
               case 2:
                    return new Color(0.4f, 0.8f, 1f, 1f); // Blue
               case 3:
                    return new Color(0.5f, 1f, 0.5f, 1f); // Green
               case 4:
                    return new Color(0.9f, 0.6f, 1f, 1f); // Purple
               default:
                    return Color.white;
          }
     }
}
