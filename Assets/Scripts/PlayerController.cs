/* This script handles the player character prefab logic. */

// Scenes: Game

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Import Unity's Input System

public class PlayerController2D : MonoBehaviour
{

     public float moveSpeed = 100f;
     public Transform movePoint;
     public Rigidbody2D body;
     public SpriteRenderer spriteRenderer;

     // Lists of sprite frames based on direction
     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     public LayerMask whatStopsMovement; // This character will bump into objects in this layermask

     //private CharacterController controller;
     private Vector2 movementInput = Vector2.zero; // Stores movement direction

     public Gamepad assignedController; // Assigned gamepad for this player
     public Vector2 assignedSpawnPoint; // Assigned spawnPoint for this player

     private void Awake()
     {

     }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          //controller = gameObject.GetComponent<CharacterController>();
          movePoint.parent = null; // Detach movePoint from parent to move independently
     }

     // Update is called once per frame
     void Update()
     {

     }

     // Callback function for the InputManager
     public void OnMove(InputAction.CallbackContext context)
     {
          movementInput = context.ReadValue<Vector2>();
     }

     // Change the direction the character is facing depending on direction moved
     private List<Sprite> GetSpriteDirection()
     {
          if (movementInput.y > 0) return nSprites;
          if (movementInput.y < 0) return sSprites;
          if (movementInput.x > 0) return eSprites;
          if (movementInput.x < 0) return wSprites;
          return null;
     }


     /* The logic for how the character moves. 
      * This is only triggered by the BeatManager, ensuring that the character moves on beat */
     public void MoveCharacter()
     {
          //Synchronizes all movement to for all systems.
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

          //Locks movement to either 1 unit vertically or horizontally.
          if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
          {
               if (Mathf.Abs(movementInput.x) == 1)
               {
                    //Checks collision ahead. If there's nothing, the player can move horizontally.
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(movementInput.x, 0f, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(movementInput.x, 0f, 0f);
                    }
               }

               else if (Mathf.Abs(movementInput.y) == 1)
               {
                    //Checks collision ahead. If there's nothing, the player can move vertically.
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, movementInput.y, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(0f, movementInput.y, 0f);
                    }
               }
          }

          // get sprite that faces same direction as input
          List<Sprite> directionSprites = GetSpriteDirection();

          if (directionSprites != null)
          {
               spriteRenderer.sprite = directionSprites[0];
          }
     }
}