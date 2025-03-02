using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class Intervals
{
     [SerializeField] private float steps;
     [SerializeField] private UnityEvent trigger;
     private int lastInterval;

     public float GetIntervalLength(float bpm)
     {
          return 60f / (bpm * steps);
     }

     public void CheckForNewInterval (float interval)
     {
          if (Mathf.FloorToInt(interval) != lastInterval)
          {
               lastInterval = Mathf.FloorToInt(interval);
               trigger.Invoke();
          }
     }
}

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

     [SerializeField] private float bpm;
     [SerializeField] private AudioSource music; // Handles synchronization of the music
     [SerializeField] private Intervals[] intervals;

     void Start()
     {
         
     }

     void Update()
     {
          Synchronize(music);

          // get direction of input
          direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

          // set walk based on direction
          body.linearVelocity = direction * walkSpeed;

          // get sprite that faces same direction as input
          List<Sprite> directionSprites =  GetSpriteDirection();
          if (directionSprites != null )
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
     
     void Synchronize(AudioSource music)
     {
          foreach (Intervals interval in intervals)
          {
               float sampledTime = (music.timeSamples / music.clip.frequency * interval.GetIntervalLength(bpm));
               interval.CheckForNewInterval(sampledTime);

          }

     }

}