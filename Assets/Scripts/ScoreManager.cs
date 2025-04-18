using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
     //public static ScoreManager Instance { get; private set; } // Singleton instance

     public static Dictionary<GameObject, int> playerScores = new Dictionary<GameObject, int>(); // Stores player scores

     private void Awake()
     {
          // Ensure only one instance of ScoreManager exists
          //if (Instance == null)
          //{
          //     Instance = this;
          //}
          //else
          //{
          //     Destroy(gameObject);
          //}
     }

     // Increments the score for a given player
     public void AddScore(GameObject player)
     {
          if (playerScores.ContainsKey(player))
          {
               playerScores[player]++; // Increase existing player's score
          }
          else
          {
               playerScores[player] = 1; // Initialize score for new player
          }

          Debug.Log($"{player.name} now has {playerScores[player]} points.");
     }

     // Retrieves the current score of a player
     public int GetScore(GameObject player)
     {
          return playerScores.ContainsKey(player) ? playerScores[player] : 0;
     }
}
