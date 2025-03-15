using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController2D : MonoBehaviour
{
     public float walkSpeed = 5f;
     public Transform movePoint;
     public Rigidbody2D body;
     public SpriteRenderer spriteRenderer;
     public List<Sprite> nSprites, sSprites, eSprites, wSprites;
     public LayerMask whatStopsMovement;

     // Variables for rhythm-based movement
     private List<ActionItem> inputBuffer = new List<ActionItem>();
     public bool canMove = false;
     private bool movingOnBeat = false;

     // Visual feedback for on-beat movements
     public GameObject onBeatEffect;

     Vector2 direction;

     void Start()
     {
          movePoint.parent = null;

          // Subscribe to beat events
          BeatManager.OnBeat += OnBeatEvent;
     }

     void OnDestroy()
     {
          // Always unsubscribe when destroyed
          BeatManager.OnBeat -= OnBeatEvent;
     }

     private void OnBeatEvent(float beatTime)
     {
          // Enable movement on beat
          canMove = true;

          // Process any buffered inputs when a beat occurs
          ProcessBufferedInputs(beatTime);
     }

     void Update()
     {
          // Handle player input
          CheckInput(canMove);

          // Move character toward move point
          MoveTowardTarget();
     }

     private void ProcessBufferedInputs(float beatTime)
     {
          if (inputBuffer.Count > 0)
          {
               List<ActionItem> validActions = new List<ActionItem>();

               // Find valid actions closest to the beat
               foreach (ActionItem action in inputBuffer)
               {
                    if (action.CheckIfValid() && Mathf.Abs(action.timeStamp - beatTime) <= BeatManager.BeatWindowSize)
                    {
                         validActions.Add(action);
                    }
               }

               // Sort by how close they are to the beat
               validActions.Sort((a, b) =>
                   Mathf.Abs(a.timeStamp - beatTime).CompareTo(Mathf.Abs(b.timeStamp - beatTime)));

               // Take the most accurate input
               if (validActions.Count > 0)
               {
                    // Remove from buffer
                    inputBuffer.Remove(validActions[0]);

                    // Apply movement with bonus for being on beat
                    ApplyMovement(validActions[0].action, true);
               }

               // Clear old inputs that are no longer valid
               CleanBuffer();
          }
     }

     private void MoveTowardTarget()
     {
          transform.position = Vector3.MoveTowards(transform.position, movePoint.position, walkSpeed * Time.deltaTime);
     }

     private void ApplyMovement(ActionItem.InputAction action, bool isOnBeat)
     {
          // Only move if we're close enough to the target point
          if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
          {
               Vector3 moveDirection = Vector3.zero;

               switch (action)
               {
                    case ActionItem.InputAction.up:
                         moveDirection = new Vector3(0f, 1f, 0f);
                         direction = Vector2.up;
                         break;
                    case ActionItem.InputAction.down:
                         moveDirection = new Vector3(0f, -1f, 0f);
                         direction = Vector2.down;
                         break;
                    case ActionItem.InputAction.left:
                         moveDirection = new Vector3(-1f, 0f, 0f);
                         direction = Vector2.left;
                         break;
                    case ActionItem.InputAction.right:
                         moveDirection = new Vector3(1f, 0f, 0f);
                         direction = Vector2.right;
                         break;
               }

               // Check if there's no obstacle
               if (!Physics2D.OverlapCircle(movePoint.position + moveDirection, 0.2f, whatStopsMovement))
               {
                    movePoint.position += moveDirection;

                    // Apply on-beat effects if this was a precise movement
                    if (isOnBeat)
                    {
                         ShowOnBeatEffect();
                    }
               }

               // Update character sprite
               UpdateSprite();
          }
     }

     private void ShowOnBeatEffect()
     {
          // Here you can instantiate a particle effect, play a sound,
          // or provide any visual feedback for successful on-beat movement
          if (onBeatEffect != null)
          {
               Instantiate(onBeatEffect, transform.position, Quaternion.identity);
          }

          // You could also apply gameplay benefits here:
          // - Extra points
          // - Speed boost
          // - Special abilities
     }

     private void UpdateSprite()
     {
          List<Sprite> directionSprites = GetSpriteDirection();
          if (directionSprites != null)
          {
               spriteRenderer.sprite = directionSprites[0];
          }
     }

     // determine the direction the character should face based on directional input
     List<Sprite> GetSpriteDirection()
     {
          if (direction.y > 0) return nSprites;
          else if (direction.y < 0) return sSprites;
          else if (direction.x > 0) return eSprites;
          else if (direction.x < 0) return wSprites;

          return null;
     }

     private void CheckInput(bool canProcess)
     {
          // Always collect input for the buffer
          if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
          {
               inputBuffer.Add(new ActionItem(ActionItem.InputAction.up, Time.time));
          }
          else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
          {
               inputBuffer.Add(new ActionItem(ActionItem.InputAction.left, Time.time));
          }
          else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
          {
               inputBuffer.Add(new ActionItem(ActionItem.InputAction.down, Time.time));
          }
          else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
          {
               inputBuffer.Add(new ActionItem(ActionItem.InputAction.right, Time.time));
          }

          // For non-beat based (continuous) movement
          if (canProcess && !movingOnBeat)
          {
               // Handle continuous movement here using standard Input system
               if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
               {
                    float horizontal = Input.GetAxisRaw("Horizontal");
                    float vertical = Input.GetAxisRaw("Vertical");

                    if (Mathf.Abs(horizontal) == 1)
                    {
                         if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(horizontal, 0f, 0f), 0.2f, whatStopsMovement))
                         {
                              movePoint.position += new Vector3(horizontal, 0f, 0f);
                              direction = new Vector2(horizontal, 0);
                              UpdateSprite();
                         }
                    }
                    else if (Mathf.Abs(vertical) == 1)
                    {
                         if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, vertical, 0f), 0.2f, whatStopsMovement))
                         {
                              movePoint.position += new Vector3(0f, vertical, 0f);
                              direction = new Vector2(0, vertical);
                              UpdateSprite();
                         }
                    }
               }
          }
     }

     private void CleanBuffer()
     {
          // Remove expired actions
          inputBuffer.RemoveAll(action => !action.CheckIfValid());
     }
}

public class ActionItem
{
     public enum InputAction { up, left, down, right };
     public InputAction action;
     public float timeStamp;

     public static float TimeBeforeActionsExpire = 0.25f; // 1/4 second buffer

     public ActionItem(InputAction ia, float stamp)
     {
          action = ia;
          timeStamp = stamp;
     }

     public bool CheckIfValid()
     {
          return timeStamp + TimeBeforeActionsExpire >= Time.time;
     }
}