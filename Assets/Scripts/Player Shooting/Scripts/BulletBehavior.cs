using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletBehavior : MonoBehaviour
{
     [SerializeField] private float normalBulletSpeed = 3f; // Speed of the bullet
     [SerializeField] private float destroyTime = 5f; // Time before bullet auto-destroys
     [SerializeField] private LayerMask whatDestroysBullet; // Layers that can destroy the bullet

     private Rigidbody2D rb;
     private GameObject shooter; // Reference to the shooter
     private ScoreManager scoreManager; // Reference to the ScoreManager

     public void Initialize(GameObject shooter)
     {
          this.shooter = shooter; // Store who fired the bullet
     }

     private void Start()
     {
          rb = GetComponent<Rigidbody2D>();

          SetDestroyTime(); // Destroys the bullet after a certain period of time
          SetVelocity(); // Sets the speed of the bullet

          scoreManager = FindObjectOfType<ScoreManager>(); // Find ScoreManager in the scene
     }

     private void OnTriggerEnter2D(Collider2D collision)
     {
          // no friendly fire
          if (collision.gameObject == shooter)
          {
               return;
          }

          if (collision.CompareTag("Player")) // Player was hit
          {
               GameObject player = collision.gameObject;
               if (PlayerIsProtected(player))
               {
                    Debug.Log($"{player.name} had spawn protection");
                    return;
               }
               else
               {
                    Debug.Log($"{player.name} was hit by bullet!");

                    HandleBulletImpact(player); // Kill player and update score

                    Destroy(gameObject); // Destroy bullet on hit
               }
          }

          // Destroy bullet on collision with environment
          if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
          {
               Destroy(gameObject);
          }
     }

     private void SetVelocity()
     {
          rb.linearVelocity = transform.right * normalBulletSpeed; // Move the bullet forward
     }

     private void SetDestroyTime()
     {
          Destroy(gameObject, destroyTime); // Auto-destroy bullet after timer
     }

     // Check if the player is invincible (spawn protection active)
     bool PlayerIsProtected(GameObject player)
     {
          return PlayerManager.IsPlayerInvincible(player);
     }

     // Handles killing the player, updating the shooter's score, and respawning the player
     private void HandleBulletImpact(GameObject hitPlayer)
     {
          if (scoreManager != null && shooter != null)
          {
               scoreManager.AddScore(shooter); // Increase shooter's score
               Debug.Log($"{shooter.name} killed {hitPlayer.name}! Score updated.");
          }
          else
          {
               Debug.LogWarning("ScoreManager missing or shooter missing. No score updated.");
          }

          hitPlayer.SetActive(false); // Disable the hit player
          PlayerManager.RespawnPlayers(hitPlayer); // Respawn the player
     }
}
