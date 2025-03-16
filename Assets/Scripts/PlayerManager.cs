using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
     public GameObject playerPrefab;  // Assign your player prefab in the Inspector

     private Dictionary<int, PlayerController2D> activePlayers = new Dictionary<int, PlayerController2D>();

     private GridManager gridManager;
     public List<Vector2> spawnPoints { get; private set; } // Expose spawn points with a public getter

     private void Awake()
     {
          gridManager = FindObjectOfType<GridManager>();
     }

     void Start()
     {
          if (gridManager == null)
          {
               Debug.LogError("GridManager not found! Cannot set spawn points.");
               return;
          }

          spawnPoints = gridManager.GetSpawnPoints();

          if (spawnPoints == null || spawnPoints.Count < 4)
          {
               Debug.LogError("Not enough spawn points available!");
               return;
          }

          var registeredPlayers = ReadyUpUI.RegisteredPlayers;

          if (registeredPlayers.Count == 0)
          {
               // No controllers detected, spawn a single keyboard player for testing.
               Debug.Log("No controllers detected. Spawning single keyboard player.");
               SpawnPlayer(0, "Keyboard");
          }
          else
          {
               // Spawn only controller-assigned players.
               foreach (var entry in registeredPlayers)
               {
                    int playerId = entry.Key;
                    string gamepadName = entry.Value;
                    SpawnPlayer(playerId, gamepadName);
               }
          }
     }

     public void SpawnPlayer(int playerID, string gamepadName)
     {
          if (spawnPoints == null || spawnPoints.Count == 0)
          {
               Debug.LogError("No spawn points set in PlayerManager! Cannot spawn player.");
               return;
          }

          int spawnIndex = playerID % spawnPoints.Count;
          Vector2 spawnPosition = spawnPoints[spawnIndex];

          GameObject newPlayer = Instantiate(playerPrefab, (Vector3)spawnPosition, Quaternion.identity);
          PlayerController2D playerController = newPlayer.GetComponent<PlayerController2D>();

          if (playerController != null)
          {
               playerController.playerID = playerID;
               playerController.AssignController(gamepadName);
               activePlayers[playerID] = playerController; // Track spawned players
          }
     }

     // Respawn a player after they are killed
     public void RespawnPlayer(int playerID)
     {
          if (activePlayers.ContainsKey(playerID))
          {
               activePlayers[playerID].Respawn();
          }
          else
          {
               Debug.LogWarning($"Player {playerID + 1} is missing, spawning a new one.");
               string gamepadName = ReadyUpUI.RegisteredPlayers.ContainsKey(playerID) ? ReadyUpUI.RegisteredPlayers[playerID] : "Keyboard";
               SpawnPlayer(playerID, gamepadName);
          }
     }
}
