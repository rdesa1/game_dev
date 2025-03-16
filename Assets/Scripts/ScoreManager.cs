using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
     public static ScoreManager Instance { get; private set; }

     private Dictionary<GameObject, int> playerScores = new Dictionary<GameObject, int>();

     private void Awake()
     {
          if (Instance == null)
          {
               Instance = this;
          }
          else
          {
               Destroy(gameObject);
          }
     }

     public void AddScore(GameObject player)
     {
          if (playerScores.ContainsKey(player))
          {
               playerScores[player]++;
          }
          else
          {
               playerScores[player] = 1;
          }

          Debug.Log($"{player.name} now has {playerScores[player]} points.");
     }

     public int GetScore(GameObject player)
     {
          return playerScores.ContainsKey(player) ? playerScores[player] : 0;
     }
}
