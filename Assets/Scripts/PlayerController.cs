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
               // Get input from keyboard or controller
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

     private float GetHorizontalInput()
     {
          float input = Input.GetAxisRaw("Horizontal"); // Keyboard input

          if (Gamepad.current != null)
          {
               float gamepadInput = Gamepad.current.leftStick.x.ReadValue();
               if (Mathf.Abs(gamepadInput) > 0.5f)
               {
                    input = Mathf.Round(gamepadInput);
               }
               else
               {
                    input = Mathf.Round(Gamepad.current.dpad.x.ReadValue());
               }
          }

          return input;
     }

     private float GetVerticalInput()
     {
          float input = Input.GetAxisRaw("Vertical"); // Keyboard input

          if (Gamepad.current != null)
          {
               float gamepadInput = Gamepad.current.leftStick.y.ReadValue();
               if (Mathf.Abs(gamepadInput) > 0.5f)
               {
                    input = Mathf.Round(gamepadInput);
               }
               else
               {
                    input = Mathf.Round(Gamepad.current.dpad.y.ReadValue());
               }
          }

          return input;
     }

     List<Sprite> GetSpriteDirection()
     {
          if (direction.y > 0) return nSprites;
          if (direction.y < 0) return sSprites;
          if (direction.x > 0) return eSprites;
          if (direction.x < 0) return wSprites;
          return null;
     }
}
