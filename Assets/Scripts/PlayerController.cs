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
               // Get input from keyboard or controller
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

     // Get horizontal movement input from keyboard & controller
     private float GetHorizontalInput()
     {
          float input = Input.GetAxisRaw("Horizontal"); // Keyboard input

          if (Gamepad.current != null)
          {
               input = Gamepad.current.leftStick.x.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = Gamepad.current.dpad.x.ReadValue();
               }
          }

          return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
     }

     // Get vertical movement input from keyboard & controller
     private float GetVerticalInput()
     {
          float input = Input.GetAxisRaw("Vertical"); // Keyboard input

          if (Gamepad.current != null)
          {
               input = Gamepad.current.leftStick.y.ReadValue(); // Left Stick
               if (Mathf.Abs(input) < 0.5f) // Prioritize D-Pad if left stick is not used much
               {
                    input = Gamepad.current.dpad.y.ReadValue();
               }
          }

          return Mathf.Round(input); // Round to -1, 0, or 1 to ensure discrete movement
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
}
