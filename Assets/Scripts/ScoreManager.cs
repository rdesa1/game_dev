using UnityEngine;

public class ScoreManager : MonoBehaviour
{
     // Score array for 4 players (playerID 1 maps to index 0)
     public int[] playerScores = new int[4];

     // Adds score for the given player object
     public void AddScore(GameObject player)
     {
          PlayerController2D playerController = player.GetComponent<PlayerController2D>();
          if (playerController != null)
          {
               int id = playerController.playerID;

               if (id >= 1 && id <= 4)
               {
                    playerScores[id - 1]++; // ID 1 → index 0, ID 2 → index 1, etc.
                    Debug.Log($"Player {id} scored! Total: {playerScores[id - 1]}");
               }
               else
               {
                    Debug.LogWarning("Player has invalid playerID! (Expected 1-4)");
               }
          }
          else
          {
               Debug.LogWarning("PlayerController2D not found on player object!");
          }
     }
}
