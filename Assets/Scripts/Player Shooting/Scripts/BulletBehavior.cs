using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletBehavior : MonoBehaviour
{
     [SerializeField] private float normalBulletSpeed = 3f;
     [SerializeField] private float destroyTime = 5f;
     [SerializeField] private LayerMask whatDestroysBullet;

     private Rigidbody2D rb;
     private GameObject shooter; // Reference to the shooter

     public void Initialize(GameObject shooter)
     {
          this.shooter = shooter; // Store who fired the bullet
     }

     private void Start()
     {
          rb = GetComponent<Rigidbody2D>();

          SetDestroyTime(); // Destroys the bullet after a certain period of time
          SetVelocity(); // Sets the speed of the bullet
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
          if (collision.gameObject == shooter) return;

          if (collision.CompareTag("Player")) // Player was hit
          {
               Debug.Log("Player hit!");

               PlayerController2D playerProperties = collision.GetComponent<PlayerController2D>();
               if (playerProperties != null)
               {
                    PlayerManager.respawnQueue.Enqueue(collision.gameObject); // Add player to respawn queue
                    collision.gameObject.SetActive(false); // Make player disappear instead of destroying
                    Debug.Log($"{collision.gameObject.name} added to respawn queue!");
               }
          }

          // Destroy bullet
          if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
          {
               Destroy(gameObject);
          }
     }




     //private void OnTriggerEnter2D(Collider2D collision)
     //{
     //     // Ignore collision with the shooter
     //     if (collision.gameObject == shooter) return;

     //     if (collision.CompareTag("Player")) // Player was hit
     //     {
     //          Debug.Log("Player hit!");
     //          GameObject copyOfPlayer = new GameObject();
     //          Debug.Log($"{collision.gameObject} has just been added to the respawn queue!");
     //          Destroy(collision.gameObject); // Eliminate the player

     //          // Increase shooter's score (You will need to implement this in a score manager)
     //     }

     //     // Destroy bullet if it hits something in the whatDestroysBullet layer
     //     if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
     //     {
     //          Destroy(gameObject);
     //     }
     //}



     private void HandleBulletImpact(GameObject hitObject)
     {
          // If the bullet hit a PlayerCharacter and it's not the shooter
          if (hitObject.CompareTag("PlayerCharacter") && hitObject != shooter)
          {
               // Increase the shooter's score
               ScoreManager.Instance.AddScore(shooter);

               // Destroy the hit player (or disable them)
               Destroy(hitObject); // Alternative: hitObject.SetActive(false);

               Debug.Log($"{shooter.name} killed {hitObject.name}! Score updated.");
          }

          // Destroy the bullet after hitting any valid target
          Destroy(gameObject);
     }

     private void SetVelocity()
     {
          rb.linearVelocity = transform.right * normalBulletSpeed;
     }

     private void SetDestroyTime()
     {
          Destroy(gameObject, destroyTime);
     }
}
