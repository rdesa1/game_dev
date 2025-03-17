using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
     public const int MAX_PLAYERS = 4; // Maximum number of players allowed

     //public GameObject playerPrefab; // Assign the player prefab in the Inspector
     //public Transform[] spawnPoints; // Assign spawn points in the Inspector

     //private Dictionary<int, Gamepad> activePlayers = new Dictionary<int, Gamepad>(); // Stores assigned controllers
     //private List<GameObject> spawnedPlayers = new List<GameObject>(); // Stores instantiated player objects

     //public static PlayerManager Instance { get; private set; } // Singleton instance

     private void Awake()
     {
          // Ensure only one instance of PlayerManager exists
          //if (Instance == null)
          //{
          //     Instance = this;
          //}
          //else
          //{
          //     Destroy(gameObject);
          //}


     }

     private void Start()
     {
          //Debug.Log($"PlayerManager Start() called. Players assigned: {activePlayers.Count}");
     }

     //// Assigns players' controllers after scene transition
     //public void AssignPlayerControllers(Dictionary<int, Gamepad> readyPlayers)
     //{
     //     activePlayers = new Dictionary<int, Gamepad>(readyPlayers); // Copy the assigned players
     //     Debug.Log($"PlayerManager assigned {readyPlayers.Count} players before scene transition.");

     //     SpawnPlayers();
     //}

     // Instantiates player characters at predefined spawn points
     //private void SpawnPlayers()
     //{
     //     Debug.Log($"Spawning {activePlayers.Count} players...");
     //     int i = 0;
     //     foreach (var player in activePlayers)
     //     {
     //          if (i >= spawnPoints.Length) break; // Ensure we don't exceed available spawn points

     //          GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
     //          Debug.Log($"Player {i + 1} instantiated at {spawnPoints[i].position}");

     //          // Assign Rigidbody2D to PlayerController2D
     //          PlayerController2D controller = newPlayer.GetComponent<PlayerController2D>();
     //          if (controller != null)
     //          {
     //               controller.body = newPlayer.GetComponent<Rigidbody2D>();
     //          }

     //          spawnedPlayers.Add(newPlayer);
     //          i++;
     //     }
     //}
}
