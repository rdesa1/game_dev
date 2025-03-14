using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController2D : MonoBehaviour
{
     // Public variables
     public float walkSpeed = 5f; // The speed at which the player moves
     public float frameRate;
     public Transform movePoint; //Essentially the object that determines whether a player can move or not.

     // Reference to the Rigidbody2D component attached to the player
     public Rigidbody2D body;

     // reference the sprites per direction
     public SpriteRenderer spriteRenderer;
     public List<Sprite> nSprites;
     public List<Sprite> sSprites;
     public List<Sprite> eSprites;
     public List<Sprite> wSprites;

     Vector2 direction; // Stores the direction of player movement

     public LayerMask whatStopsMovement; //Checks which layer prevents the player to move into that area.

     void Start()
     {
          //I think this is where the move point spawns on start. 
          //We can adjust this later, but for now it's set to null.
          movePoint.parent = null;
     }

     public void Update()
     {
          
     }

     public void MoveCharacter()
     {
          //Synchronizes all movement to for all systems.
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);

          //Locks movement to either 1 unit veritcally or horizontally.
          if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
          {
               if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
               {
                    //Checks collision ahead. If there's nothing, the player can move horizontally.
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
               }

               else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
               {
                    //Checks collision ahead. If there's nothing, the player can move vertically.
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, whatStopsMovement))
                    {
                         movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
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