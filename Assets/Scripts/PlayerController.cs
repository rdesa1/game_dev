using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Import Unity's Input System

public class PlayerController2D : MonoBehaviour
{
     public float walkSpeed = 5f; // Speed of movement
     public float frameRate; // Frame rate for animations
     public Transform movePoint; // Target position for movement
     public Rigidbody2D body; // Rigidbody for physics interactions
     public SpriteRenderer spriteRenderer; // Sprite renderer for character visuals

     public List<Sprite> nSprites; // North-facing sprites
     public List<Sprite> sSprites; // South-facing sprites
     public List<Sprite> eSprites; // East-facing sprites
     public List<Sprite> wSprites; // West-facing sprites

     public LayerMask whatStopsMovement; // Layer mask for obstacles

     private Vector2 direction = Vector2.zero; // Stores movement direction
     private Vector2 lastInput = Vector2.zero; // Stores last keyboard input

     void Start()
     {
          movePoint.parent = null; // Detach movePoint from parent to move independently
     }

     void Update()
     {
          // Check keyboard input every frame and store the last input
          float moveX = Input.GetAxisRaw("Horizontal");
          float moveY = Input.GetAxisRaw("Vertical");

          // Store the last valid input for movement
          if (moveX != 0)
          {
               lastInput = new Vector2(Mathf.Round(moveX), 0); // Prioritize horizontal movement
          }
          else if (moveY != 0)
          {
               lastInput = new Vector2(0, Mathf.Round(moveY));
          }
     }

     public void MoveCharacter()
     {
          // Move toward the movePoint
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);

          if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
          {
               // Use last stored input for movement
               float moveX = lastInput.x;
               float moveY = lastInput.y;

               // Update direction before movement check
               direction = new Vector2(moveX, moveY);

               // Prioritize horizontal movement over vertical
               if (Mathf.Abs(moveX) == 1 && Mathf.Abs(moveY) == 0)
               {
                    // Check for obstacles before moving
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveX, 0f, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(moveX, 0f, 0f);
                    }
               }
               else if (Mathf.Abs(moveY) == 1 && Mathf.Abs(moveX) == 0)
               {
                    // Check for obstacles before moving
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
                         spriteRenderer.sprite = directionSprites[0]; // Update sprite to match direction
                    }
               }
          }
     }


     // Returns the correct sprite list based on movement direction
     List<Sprite> GetSpriteDirection()
     {
          if (direction.y > 0) return nSprites;
          if (direction.y < 0) return sSprites;
          if (direction.x > 0) return eSprites;
          if (direction.x < 0) return wSprites;
          return null;
     }
}