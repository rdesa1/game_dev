using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Import Unity's Input System

public class PlayerController2D : MonoBehaviour
{
     public float walkSpeed = 5f;
     public float frameRate;
     public Transform movePoint;
     public Rigidbody2D body;
     public SpriteRenderer spriteRenderer;

     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     public LayerMask whatStopsMovement;

     private Vector2 direction = Vector2.zero; // Stores movement direction

     public int playerID;

     private Gamepad assignedGamepad; // Assigned gamepad for this player

     void Start()
     {
          movePoint.parent = null;
     }

     public void MoveCharacter()
     {
          // Move toward the movePoint
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);

          if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
          {
               // Get input from the assigned controller
               float moveX = GetHorizontalInput();
               float moveY = GetVerticalInput();

               // Update direction before movement check
               direction = new Vector2(moveX, moveY);

               // Prioritize horizontal movement over vertical
               if (Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 0)
               {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveX, 0f, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(moveX, 0f, 0f);
                    }
               }
               else if (Mathf.Abs(moveY) == 1 && Mathf.Abs(moveX) == 0)
               {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveY, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(0f, moveY, 0f);
                    }
               }

               // Update sprite only when there's movement
               if (direction != Vector2.zero)
               {
                    List<Sprite> directionSprites = GetSpriteDirection();
                    if (directionSprites != null)
                    {
                         spriteRenderer.sprite = directionSprites[0];
                    }
               }
          }
     }

<<<<<<< HEAD
     // Get horizontal movement input from the assigned controller
=======
>>>>>>> bda86b3 (working on keyboard fix)
     private float GetHorizontalInput()
     {
          if (assignedGamepad != null) // Only use the assigned gamepad
          {
<<<<<<< HEAD
               float input = assignedGamepad.leftStick.x.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = assignedGamepad.dpad.x.ReadValue();
=======
               float gamepadInput = Gamepad.current.leftStick.x.ReadValue();
               if (Mathf.Abs(gamepadInput) > 0.5f)
               {
                    input = Mathf.Round(gamepadInput);
               }
               else
               {
                    input = Mathf.Round(Gamepad.current.dpad.x.ReadValue());
>>>>>>> bda86b3 (working on keyboard fix)
               }
               return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
          }

<<<<<<< HEAD
          // If no assigned controller and single-player mode is active, allow keyboard input
          return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Horizontal") : 0;
     }

     // Get vertical movement input from the assigned controller
=======
          return input;
     }

>>>>>>> bda86b3 (working on keyboard fix)
     private float GetVerticalInput()
     {
          if (assignedGamepad != null) // Only use the assigned gamepad
          {
<<<<<<< HEAD
               float input = assignedGamepad.leftStick.y.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = assignedGamepad.dpad.y.ReadValue();
=======
               float gamepadInput = Gamepad.current.leftStick.y.ReadValue();
               if (Mathf.Abs(gamepadInput) > 0.5f)
               {
                    input = Mathf.Round(gamepadInput);
               }
               else
               {
                    input = Mathf.Round(Gamepad.current.dpad.y.ReadValue());
>>>>>>> bda86b3 (working on keyboard fix)
               }
               return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
          }

<<<<<<< HEAD
          // If no assigned controller and single-player mode is active, allow keyboard input
          return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Vertical") : 0;
=======
          return input;
>>>>>>> bda86b3 (working on keyboard fix)
     }

     List<Sprite> GetSpriteDirection()
     {
          if (direction.y > 0) return nSprites;
          if (direction.y < 0) return sSprites;
          if (direction.x > 0) return eSprites;
          if (direction.x < 0) return wSprites;
          return null;
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
