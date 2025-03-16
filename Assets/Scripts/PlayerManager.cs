using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
     public GameObject playerPrefab; // Assign the player prefab in the Inspector
     public Transform[] spawnPoints; // Assign spawn points in the Inspector

     private Dictionary<int, Gamepad> activePlayers = new Dictionary<int, Gamepad>();
     private List<GameObject> spawnedPlayers = new List<GameObject>();

     private const int MaxPlayers = 4;

     public static PlayerManager Instance { get; private set; }

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

     private void Start()
     {
          Debug.Log($"PlayerManager Start() called. Players assigned: {activePlayers.Count}");

          if (spawnPoints == null || spawnPoints.Count < 4)
          {
               Debug.LogError("Not enough spawn points available!");
               return;
     }

     public void AssignPlayerControllers(Dictionary<int, Gamepad> readyPlayers)
     {
          activePlayers = new Dictionary<int, Gamepad>(readyPlayers);
          Debug.Log($"PlayerManager assigned {readyPlayers.Count} players before scene transition.");

          SpawnPlayers();
     }

     private void SpawnPlayers()
     {
          Debug.Log($"Spawning {activePlayers.Count} players...");
          int i = 0;
          foreach (var player in activePlayers)
          {
               if (i >= spawnPoints.Length) break;

               GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
               Debug.Log($"Player {i + 1} instantiated at {spawnPoints[i].position}");

               PlayerController2D controller = newPlayer.GetComponent<PlayerController2D>();

               if (controller != null)
               {
                    controller.body = newPlayer.GetComponent<Rigidbody2D>(); // Assign Rigidbody2D
               }

               spawnedPlayers.Add(newPlayer);
               i++;
               

          }
     }
}
