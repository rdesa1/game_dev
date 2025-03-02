using UnityEngine;
using System.Collections.Generic;

public class PlayerController2D : MonoBehaviour
{
     // Public variables
     public float walkSpeed = 5f; // The speed at which the player moves
     public float frameRate;
     public bool canMoveDiagonally = true; // Controls whether the player can move diagonally

     // Reference to the Rigidbody2D component attached to the player
     public Rigidbody2D body;

     // reference the sprites per direction
     public SpriteRenderer spriteRenderer;
     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     Vector2 direction; // Stores the direction of player movement

     // Private variables 
     private bool isMovingHorizontally = true; // Flag to track if the player is moving horizontally

     void Start()
     {
         
     }

     void Update()
     {
          // get direction of input
          direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

          // set walk based on direction
          body.linearVelocity = direction * walkSpeed;

          
          // get sprite that faces same direction as input
          List<Sprite> directionSprites =  GetSpriteDirection();

          if (directionSprites != null )
          {
               spriteRenderer.sprite = directionSprites[0];
          } else
          {

          }
     } 

     // determine the direction the character should face based on directional input
     List<Sprite> GetSpriteDirection()
     {
          List<Sprite> selectedSprites = null;

          if (direction.y > 0) // north
          {
               selectedSprites = nSprites;
          }
          else if (direction.y < 0) // south
          {
               selectedSprites = sSprites;
          }
          else if (direction.x > 0) // east
          {
               selectedSprites = eSprites;
          }
          else if (direction.x < 0) // west
          {
               selectedSprites = wSprites;
          }

          return selectedSprites;
     }
     
}