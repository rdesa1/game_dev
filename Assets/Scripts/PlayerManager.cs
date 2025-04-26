/* This script is responsible for instantiating player prefabs. */

// Scenes: ReadyUpScene (persist to)=> Game

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
     public const int MAX_NUMBER_OF_PLAYERS = 4; // Maximum number of players allowed

     public static int numberOfPlayers = 0; // number of players to add to the game

     [SerializeField] private double spawnProtectionDuration = 2.5f; // Time in seconds for a player to be invulnerable after respawning
     private const float BLINK_RATE = 0.2f; // Blink interval used for smooth transitions

     [SerializeField] private GameObject Player1; // Prefab for player 1
     [SerializeField] private GameObject Player2; // Prefab for player 2
     [SerializeField] private GameObject Player3; // Prefab for player 3
     [SerializeField] private GameObject Player4; // Prefab for player 4

     public static List<GameObject> playerPrefabList = new List<GameObject>(); // List that will contain some number of the above prefab player objects
     public static Queue<GameObject> respawnQueue = new Queue<GameObject>(); // Queue for players awaiting respawn

     private static HashSet<GameObject> invinciblePlayers = new HashSet<GameObject>(); // Set of players currently under spawn protection
     private BeatManager beatManager; // Reference to BeatManager for syncing blinking to beats

     private void Awake()
     {
          DontDestroyOnLoad(gameObject); // Persist across scenes
     }

     private void OnEnable()
     {
          SceneManager.sceneLoaded += OnSceneLoaded; // Add to sceneLoaded event
     }

     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
     {
          CheckScene(SceneManager.GetActiveScene().name);
          beatManager = FindObjectOfType<BeatManager>(); // Cache BeatManager when the scene loads
     }

     private void OnDisable()
     {
          SceneManager.sceneLoaded -= OnSceneLoaded; // Prevent memory leaks
     }

     // Sets the number of players based on connected controllers
     private void SetNumberOfPlayers()
     {
          numberOfPlayers = ControllerManager.controllerCount;
     }

     // Adds the players to the list so they can get spawn points and controllers assigned
     private void SetPlayerList(int numberOfPlayers)
     {
          playerPrefabList.Clear();
          switch (numberOfPlayers)
          {
               case 1:
                    playerPrefabList.Add(Player1);
                    break;
               case 2:
                    playerPrefabList.Add(Player1);
                    playerPrefabList.Add(Player2);
                    break;
               case 3:
                    playerPrefabList.Add(Player1);
                    playerPrefabList.Add(Player2);
                    playerPrefabList.Add(Player3);
                    break;
               case 4:
                    playerPrefabList.Add(Player1);
                    playerPrefabList.Add(Player2);
                    playerPrefabList.Add(Player3);
                    playerPrefabList.Add(Player4);
                    break;
          }
          Debug.Log("Updated playerList count: " + playerPrefabList.Count);
     }

     // Get the spawn points from SpawnManager
     public static List<Vector2> GetSpawnPoints()
     {
          Debug.Log("Spawn points count: " + SpawnManager.spawnPoints.Count);
          return SpawnManager.spawnPoints;
     }

     // Get the list of gamepads from ControllerManager
     private List<Gamepad> GetControllerList()
     {
          Debug.Log("Controller count: " + ControllerManager.controllerList.Count);
          return ControllerManager.controllerList;
     }

     // Instantiates all players and assigns them their respective gamepad and spawnpoint
     private void InstantiatePlayers(List<GameObject> playerList, List<Vector2> spawnPoints, List<Gamepad> controllerList)
     {
          BeatManager beatManagerInstance = FindObjectOfType<BeatManager>(); //TODO: TEMP FIX
          if (beatManagerInstance == null)
          {
               Debug.LogError("BeatManager not found! Players won't sync to beats.");
               return;
          }

          if (controllerList.Count == 0)
          {
               Vector2 singlePlayerSpawnPoint = new Vector2(0, 0);
               InstantiateSinglePlayer(beatManagerInstance, playerList, singlePlayerSpawnPoint);
          }
          else
          {
               InstantiateMultiPlayer(beatManagerInstance, playerList, spawnPoints, controllerList);
          }

          DebugPlayerIDs(); // Debug print player IDs after spawning
     }

     // Helper function that sets up a 1 player game with keyboard controls
     private void InstantiateSinglePlayer(BeatManager beatManager, List<GameObject> playerList, Vector2 singlePlayerSpawnPoint)
     {
          GameObject player = Instantiate(Player1, singlePlayerSpawnPoint, Quaternion.identity);
          PlayerController2D playerProperties = player.GetComponent<PlayerController2D>();
          playerProperties.assignedSpawnPoint = singlePlayerSpawnPoint;
          playerProperties.movePoint.transform.position = singlePlayerSpawnPoint;
          playerProperties.playerID = 1; // Single player gets ID 1

          PlayerInput playerInput = player.GetComponent<PlayerInput>();
          playerInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
          PlayerAimAndShoot playerAiming = player.GetComponentInChildren<PlayerAimAndShoot>();

          foreach (Intervals interval in beatManager.intervals)
          {
               if (interval.steps == 1)
               {
                    interval.trigger.AddListener(playerProperties.MoveCharacter);
                    break;
               }
          }
          foreach (Intervals interval in beatManager.intervals)
          {
               if (interval.steps == .25)
               {
                    interval.trigger.AddListener(playerAiming.HandleShooting);
                    break;
               }
          }
     }

     // Helper function that sets up a multiplayer game with gamepad controls
     private void InstantiateMultiPlayer(BeatManager beatManager, List<GameObject> playerList, List<Vector2> spawnPoints, List<Gamepad> controllerList)
     {
          for (int index = 0; index < playerList.Count; index++)
          {
               Debug.Log("controllerList: " + controllerList[index]);
               GameObject player = Instantiate(playerList[index], spawnPoints[index], Quaternion.identity);

               PlayerController2D playerProperties = player.GetComponent<PlayerController2D>();
               playerProperties.assignedSpawnPoint = spawnPoints[index];
               playerProperties.movePoint.transform.position = spawnPoints[index];
               playerProperties.playerID = index + 1; // Assign playerID based on spawn order

               PlayerInput controller = player.GetComponent<PlayerInput>();
               controller.SwitchCurrentControlScheme("Controller", controllerList[index]);
               PlayerAimAndShoot playerAiming = player.GetComponentInChildren<PlayerAimAndShoot>();

               foreach (Intervals interval in beatManager.intervals)
               {
                    if (interval.steps == 1)
                    {
                         interval.trigger.AddListener(playerProperties.MoveCharacter);
                         break;
                    }
               }
               foreach (Intervals interval in beatManager.intervals)
               {
                    if (interval.steps == .25)
                    {
                         interval.trigger.AddListener(playerAiming.HandleShooting);
                         break;
                    }
               }
          }
     }

     // perform logic depending on the scene
     private void CheckScene(string sceneName)
     {
          if (sceneName.Equals("ReadyUpScene"))
          {
               SetNumberOfPlayers();
               SetPlayerList(numberOfPlayers);
          }
          if (sceneName.Equals("Game"))
          {
               Debug.Log("Game scene loaded, waiting for spawn points...");
               StartCoroutine(WaitForSpawnPoints());
          }
     }

     // Coroutine to wait until spawn points are available
     private IEnumerator WaitForSpawnPoints()
     {
          while (SpawnManager.spawnPoints == null || SpawnManager.spawnPoints.Count < numberOfPlayers)
          {
               Debug.Log("Waiting for spawn points to be initialized...");
               yield return null;
          }

          Debug.Log("Spawn points ready! Spawning players...");
          InstantiatePlayers(playerPrefabList, GetSpawnPoints(), GetControllerList());
     }

     // Respawn players using a random spawn point
     public static Vector2 GetRandomSpawnPoint()
     {
          List<Vector2> spawnPointPool = GetSpawnPoints();
          int index = UnityEngine.Random.Range(0, spawnPointPool.Count);
          Vector2 randomSpawnPoint = spawnPointPool[index];
          return randomSpawnPoint;
     }

     // Respawns players after they've been hit
     public static void RespawnPlayers(GameObject player)
     {
          PlayerController2D playerSpawnPoint = player.GetComponent<PlayerController2D>();
          playerSpawnPoint.movePoint.transform.position = GetRandomSpawnPoint();
          player.transform.position = playerSpawnPoint.movePoint.transform.position;
          player.SetActive(true);
          GrantSpawnProtection(player);
     }

     // Grants spawn protection and starts blinking
     public static void GrantSpawnProtection(GameObject player)
     {
          PlayerManager instance = FindObjectOfType<PlayerManager>();
          if (instance != null)
          {
               invinciblePlayers.Add(player);
               instance.StartCoroutine(instance.SpawnProtectionCoroutine(player, instance.spawnProtectionDuration));
          }
     }

     private IEnumerator SpawnProtectionCoroutine(GameObject player, double protectionDuration)
     {
          Renderer playerRenderer = player.GetComponent<Renderer>();
          Color originalColor = playerRenderer.material.color;

          if (beatManager == null)
          {
               beatManager = FindObjectOfType<BeatManager>();
          }

          Intervals beatInterval = null;
          foreach (Intervals interval in beatManager.intervals)
          {
               if (interval.steps == 1)
               {
                    beatInterval = interval;
                    break;
               }
          }

          if (beatInterval == null)
          {
               Debug.LogError("No interval with steps == 1 found! Cannot sync blinking.");
               yield break;
          }

          PlayerController2D playerProperties = player.GetComponent<PlayerController2D>();
          Color blinkColor = GetPlayerColor(playerProperties.playerID); // Get assigned color

          bool fadeOut = true;

          UnityAction blinkAction = () =>
          {
               if (playerRenderer != null)
               {
                    playerRenderer.material.color = fadeOut ? blinkColor : originalColor;
                    fadeOut = !fadeOut;
               }
          };

          beatInterval.trigger.AddListener(blinkAction);

          double elapsedTime = 0;
          while (elapsedTime < protectionDuration)
          {
               elapsedTime += Time.deltaTime;
               yield return null;
          }

          if (playerRenderer != null)
          {
               playerRenderer.material.color = originalColor;
          }

          invinciblePlayers.Remove(player);
          beatInterval.trigger.RemoveListener(blinkAction);
     }

     public static bool IsPlayerInvincible(GameObject player)
     {
          return invinciblePlayers.Contains(player);
     }

     // Gets the assigned blink color for each player
     private Color GetPlayerColor(int playerID)
     {
          switch (playerID)
          {
               case 1:
                    return new Color(1f, 0.25f, 0.25f, 1f); // Red
               case 2:
                    return new Color(0.4f, 0.8f, 1f, 1f); // Blue
               case 3:
                    return new Color(0.5f, 1f, 0.5f, 1f); // Green
               case 4:
                    return new Color(0.9f, 0.6f, 1f, 1f); // Purple
               default:
                    return Color.white;
          }
     }

     // Debug function to print all playerIDs after spawning
     private void DebugPlayerIDs()
     {
          PlayerController2D[] allPlayers = FindObjectsOfType<PlayerController2D>();

          foreach (PlayerController2D player in allPlayers)
          {
               Debug.Log($"{player.gameObject.name} has playerID: {player.playerID}");
          }
     }
}
