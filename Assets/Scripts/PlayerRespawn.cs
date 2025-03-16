using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
     [SerializeField] private float respawnDelay = 2f; // Default respawn time
     [SerializeField] private float invincibilityDuration = 3f;
     [SerializeField] private float blinkInterval = 0.2f;

     private Vector3 spawnPoint;
     private SpriteRenderer spriteRenderer;
     private Collider2D playerCollider;
     private bool isInvincible;

     private void Start()
     {
          spawnPoint = transform.position;
          Debug.Log("Spawn point set at: " + spawnPoint);
          spriteRenderer = GetComponent<SpriteRenderer>();
          playerCollider = GetComponent<Collider2D>();
     }


     public void SetRespawnTime(float time)
     {
          respawnDelay = time; // Allow dynamic updates
     }

     public void Respawn()
     {
          Debug.Log("Respawn function called.");
          StartCoroutine(RespawnCoroutine());
     }


     private IEnumerator RespawnCoroutine()
     {
          Debug.Log("Respawn Coroutine started.");
          spriteRenderer.enabled = false;
          playerCollider.enabled = false;

          yield return new WaitForSeconds(respawnDelay);

          Debug.Log("Respawning player now.");
          transform.position = spawnPoint;
          spriteRenderer.enabled = true;
          playerCollider.enabled = true;

          StartCoroutine(InvincibilityCoroutine());
     }


     private IEnumerator InvincibilityCoroutine()
     {
          isInvincible = true;
          Debug.Log("Player is now invincible.");

          float timer = 0f;
          while (timer < invincibilityDuration)
          {
               spriteRenderer.enabled = !spriteRenderer.enabled;
               yield return new WaitForSeconds(blinkInterval);
               timer += blinkInterval;
          }

          spriteRenderer.enabled = true;
          isInvincible = false; // Make sure this actually executes!
          Debug.Log("Player invincibility ended.");
     }



     public bool IsInvincible()
     {
          Debug.Log("Checking invincibility: " + isInvincible);
          return isInvincible;
     }

}
