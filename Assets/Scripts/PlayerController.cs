using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Import Unity's Input System

public class PlayerController2D : MonoBehaviour
{
     public float moveSpeed = 100f;
     public Transform movePoint;
     public Rigidbody2D body;
     public SpriteRenderer spriteRenderer;

     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     public LayerMask whatStopsMovement;

     private Vector2 direction = Vector2.zero; // Stores movement direction
     //private Vector2 lastInput = Vector2.zero; // Stores last keyboard input

     public Gamepad assignedController; // Assigned gamepad for this player
     public Vector2 assignedSpawnPoint; // Assigned spawnPoint for this player

     private void Awake()
     {

     }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          movePoint.parent = null;
     }

     // Update is called once per frame
     void Update()
     {

     }

     private List<Sprite> GetSpriteDirection()
     {
          if (direction.y > 0) return nSprites;
          if (direction.y < 0) return sSprites;
          if (direction.x > 0) return eSprites;
          if (direction.x < 0) return wSprites;
          return null;
     }

     /* currently turns the character too slowly, causing them to miss a beat every so often */
     private void UpdateWhereSpriteIsFacing()
     {
          // Update sprite only when there's movement
          if (direction != Vector2.zero)
          {
               List<Sprite> directionSprites = GetSpriteDirection();
               if (directionSprites != null && directionSprites.Count > 0)
               {
                    spriteRenderer.sprite = directionSprites[0]; // Pick the first frame for now
               }
          }
     }

     private void MoveCharacterWithKeyboard()
     {
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

          // Check if the character has reached the movement point
          if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
          {
               float horizontal = Input.GetAxisRaw("Horizontal");
               float vertical = Input.GetAxisRaw("Vertical");

               // Handle horizontal movement
               if (Mathf.Abs(horizontal) == 1f)
               {
                    // Check for collisions before moving
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(horizontal, 0f, 0f), .2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(horizontal, 0f, 0f);
                         direction = new Vector2(horizontal, 0f); // Update movement direction
                                                                  //UpdateWhereSpriteIsFacing();
                    }
               }
               // Handle vertical movement
               else if (Mathf.Abs(vertical) == 1f)
               {
                    // Check for collisions before moving
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, vertical, 0f), .2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(0f, vertical, 0f);
                         direction = new Vector2(0f, vertical); // Update movement direction
                                                                //UpdateWhereSpriteIsFacing();
                    }
               }

          }
     }


     private void MoveCharacterWithController()
     {
          if (assignedController == null) return; // Ensure there's an assigned controller

          if (assignedController.wasUpdatedThisFrame) // Ensure only the assigned controller sends input
          {
               // Read input directly from the assigned controller
               float horizontal = assignedController.leftStick.x.ReadValue();
               float vertical = assignedController.leftStick.y.ReadValue();

               Vector2 moveDirection = new Vector2(horizontal, vertical);

               if (moveDirection.magnitude > 0.1f) // Ensure there's enough input before moving
               {
                    transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
                    {
                         moveDirection = moveDirection.normalized;

                         // Handle horizontal movement
                         if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y)) // Prioritize horizontal
                         {
                              if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveDirection.x, 0f, 0f), 0.2f, whatStopsMovement))
                              {
                                   movePoint.position += new Vector3(moveDirection.x, 0f, 0f);
                                   direction = new Vector2(moveDirection.x, 0f);
                              }
                         }
                         // Handle vertical movement
                         else
                         {
                              if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveDirection.y, 0f), 0.2f, whatStopsMovement))
                              {
                                   movePoint.position += new Vector3(0f, moveDirection.y, 0f);
                                   direction = new Vector2(0f, moveDirection.y);
                              }
                         }
                    }
               }
          }
     }

     public void EnableMovement()
     {
          MoveCharacterWithKeyboard();
          MoveCharacterWithController();
     }

     //public void MoveCharacter()
     //{
     //     // Move toward the movePoint
     //     transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);

     //     if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
     //     {
     //          // Use last stored input for movement
     //          float moveX = lastInput.x;
     //          float moveY = lastInput.y;

     //          // Update direction before movement check
     //          direction = new Vector2(moveX, moveY);

     //          // Prioritize horizontal movement over vertical
     //          if (Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 0)
     //          {
     //               if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveX, 0f, 0f), 0.2f, whatStopsMovement))
     //               {
     //                    movePoint.position += new Vector3(moveX, 0f, 0f);
     //               }
     //          }
     //          else if (Mathf.Abs(moveY) == 1 && Mathf.Abs(moveX) == 0)
     //          {
     //               if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveY, 0f), 0.2f, whatStopsMovement))
     //               {
     //                    movePoint.position += new Vector3(0f, moveY, 0f);
     //               }
     //          }

     // Update sprite only when there's movement
     //if (direction != Vector2.zero)
     //{
     //     List<Sprite> directionSprites = GetSpriteDirection();
     //     if (directionSprites != null)
     //     {
     //          spriteRenderer.sprite = directionSprites[0];
     //     }
     //}
     //     }
     //}



     //private float GetHorizontalInput()
     //{
     //     if (assignedController != null) // Only use the assigned gamepad
     //     {
     //          float input = assignedController.leftStick.x.ReadValue(); // Left Stick
     //          if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
     //          {
     //               input = assignedController.dpad.x.ReadValue();
     //               float gamepadInput = Gamepad.current.leftStick.x.ReadValue();
     //               if (Mathf.Abs(gamepadInput) > 0.5f)
     //               {
     //                    input = Mathf.Round(gamepadInput);
     //               }
     //               else
     //               {
     //                    input = Mathf.Round(Gamepad.current.dpad.x.ReadValue());
     //               }
     //               return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
     //          }

     //          // If no assigned controller and single-player mode is active, allow keyboard input
     //          return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Horizontal") : 0;
     //     }
     //}

     //// Get vertical movement input from the assigned controller
     //     return input;
     //}

     //private float GetVerticalInput()
     //{
     //     if (assignedGamepad != null) // Only use the assigned gamepad
     //     {
     //          float input = assignedGamepad.leftStick.y.ReadValue(); // Left Stick
     //          if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
     //          {
     //               input = assignedGamepad.dpad.y.ReadValue();
     //          float gamepadInput = Gamepad.current.leftStick.y.ReadValue();
     //          if (Mathf.Abs(gamepadInput) > 0.5f)
     //          {
     //               input = Mathf.Round(gamepadInput);
     //          }
     //          else
     //          {
     //               input = Mathf.Round(Gamepad.current.dpad.y.ReadValue());
     //          }
     //          return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
     //     }

     //     // If no assigned controller and single-player mode is active, allow keyboard input
     //     return (Gamepad.all.Count == 0) ? Input.GetAxisRaw("Vertical") : 0;
     //     return input;
     //}



     //public void Respawn()
     //{
     //     PlayerManager playerManager = FindObjectOfType<PlayerManager>();

     //     if (playerManager.spawnPoints.Count > 0) // Use .Count instead of .Length
     //     {
     //          int spawnIndex = playerID % playerManager.spawnPoints.Count; // Use .Count
     //          transform.position = (Vector3)playerManager.spawnPoints[spawnIndex]; // No .position needed
     //          Debug.Log($"Player {playerID + 1} respawned at {transform.position}");
     //     }
     //     else
     //     {
     //          Debug.LogError("No spawn points assigned in PlayerManager!");
     //     }
     //}

     // Assign a specific controller to this player
     //public void AssignController(string gamepadName)
     //{
     //     foreach (Gamepad gamepad in Gamepad.all)
     //     {
     //          if (gamepad.name == gamepadName)
     //          {
     //               assignedGamepad = gamepad;
     //               Debug.Log($"Player {playerID + 1} assigned to {gamepad.name}");
     //               return;
     //          }
     //     }
     //     Debug.LogWarning($"Gamepad {gamepadName} not found for Player {playerID + 1}");
     //}
}