using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Import Unity's Input System

public class PlayerController2D : MonoBehaviour
{
     // Public variables
     public float walkSpeed = 5f; // The speed at which the player moves
     public float frameRate;
     public Transform movePoint; // Object that determines whether a player can move

     // Reference to the Rigidbody2D component attached to the player
     public Rigidbody2D body;

     // Reference the sprites per direction
     public SpriteRenderer spriteRenderer;
     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     Vector2 direction; // Stores the direction of player movement

     public LayerMask whatStopsMovement; // Checks which layer prevents the player to move into that area.

     public int playerID;

     private Gamepad assignedGamepad; // Assigned gamepad for this player

     void Start()
     {
          // Move point starts detached from the player
          movePoint.parent = null;
     }

     // Update is called once per frame
     public void Update()
     {

     }

     public void MoveCharacter()
     {
          // Synchronize movement across all input systems.
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);

          if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
          {
               // Get input from the assigned controller
               float moveX = GetHorizontalInput();
               float moveY = GetVerticalInput();

               // Process horizontal movement
               if (Mathf.Abs(moveX) == 1)
               {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveX, 0f, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(moveX, 0f, 0f);
                    }
               }
               // Process vertical movement
               else if (Mathf.Abs(moveY) == 1)
               {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveY, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(0f, moveY, 0f);
                    }
               }
          }

          // Get sprite that faces the same direction as input
          List<Sprite> directionSprites = GetSpriteDirection();
          if (directionSprites != null)
          {
               spriteRenderer.sprite = directionSprites[0];
          }
     }

     // Get horizontal movement input from the assigned controller
     private float GetHorizontalInput()
     {
          if (assignedGamepad != null) // Only use the assigned gamepad
          {
               float input = assignedGamepad.leftStick.x.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = assignedGamepad.dpad.x.ReadValue();
               }
               return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
          }

          // If no assigned controller and single-player mode is active, allow keyboard input
          return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Horizontal") : 0;
     }

     // Get vertical movement input from the assigned controller
     private float GetVerticalInput()
     {
          if (assignedGamepad != null) // Only use the assigned gamepad
          {
               float input = assignedGamepad.leftStick.y.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = assignedGamepad.dpad.y.ReadValue();
               }
               return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
          }

          // If no assigned controller and single-player mode is active, allow keyboard input
          return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Vertical") : 0;
     }

     // Determine the direction the character should face based on movement
     List<Sprite> GetSpriteDirection()
     {
          List<Sprite> selectedSprites = null;

          if (direction.y > 0) // North
          {
               selectedSprites = nSprites;
          }
          else if (direction.y < 0) // South
          {
               selectedSprites = sSprites;
          }
          else if (direction.x > 0) // East
          {
               selectedSprites = eSprites;
          }
          else if (direction.x < 0) // West
          {
               selectedSprites = wSprites;
          }

          return selectedSprites;
     }

     public void Respawn()
     {
          PlayerManager playerManager = FindObjectOfType<PlayerManager>();

          if (playerManager.spawnPoints.Count > 0) // Use .Count instead of .Length
          {
               int spawnIndex = playerID % playerManager.spawnPoints.Count; // Use .Count
               transform.position = (Vector3)playerManager.spawnPoints[spawnIndex]; // No .position needed
               Debug.Log($"Player {playerID + 1} respawned at {transform.position}");
          }
          else
          {
               Debug.LogError("No spawn points assigned in PlayerManager!");
          }
     }

     // Assign a specific controller to this player
     public void AssignController(string gamepadName)
     {
          foreach (Gamepad gamepad in Gamepad.all)
          {
               if (gamepad.name == gamepadName)
               {
                    assignedGamepad = gamepad;
                    Debug.Log($"Player {playerID + 1} assigned to {gamepad.name}");
                    return;
               }
          }
          Debug.LogWarning($"Gamepad {gamepadName} not found for Player {playerID + 1}");
     }
}
