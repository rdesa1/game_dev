/* This script is responsible for instantiating player prefabs. */

// Scenes: ReadyUpScene (persist to)=> Game

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
     public const int MAX_NUMBER_OF_PLAYERS = 4; // Maximum number of players allowed
     public static int numberOfPlayers = 0; // number of players to add to the game

     // Prefab player objects
     [SerializeField] private GameObject Player1;
     [SerializeField] private GameObject Player2;
     [SerializeField] private GameObject Player3;
     [SerializeField] private GameObject Player4;

     // List that will contain some number of the above prefab player objects
     public static List<GameObject> playerList = new List<GameObject>();

     private void Awake()
     {
          DontDestroyOnLoad(this); // persist across scenes
     }

     private void OnEnable()
     {
          SceneManager.sceneLoaded += OnSceneLoaded; // add to sceneLoaded event
     }

     // built-in callback function for the sceneLoaded
     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
     {
          CheckScene(SceneManager.GetActiveScene().name);
     }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     private void Start()
     {
          
     }

     private void OnDisable()
     {
          SceneManager.sceneLoaded -= OnSceneLoaded; // prevent memory leaks
     }

     // Update is called once per frame
     private void Update()
     {

     }

     // start
     private void SetNumberOfPlayers()
     {
          numberOfPlayers = ControllerManager.controllerCount;
     }

     // start
     // Adds the players to the list so they can get spawn points and controllers assigned
     private void SetPlayerList(int numberOfPlayers)
     {
          playerList.Clear();
          switch (numberOfPlayers)
          {
               case 1:
                    playerList.Add(Player1);
                    break;
               case 2:
                    playerList.Add(Player1);
                    playerList.Add(Player2);
                    break;
               case 3:
                    playerList.Add(Player1);
                    playerList.Add(Player2);
                    playerList.Add(Player3);
                    break;
               case 4:
                    playerList.Add(Player1);
                    playerList.Add(Player2);
                    playerList.Add(Player3);
                    playerList.Add(Player4);
                    break;
          }
     }

     // Get the spawn points from spawnManager
     private List<Vector2> GetSpawnPoints()
     { 
          return SpawnManager.spawnPoints;
     }

     // Get the list of gamepads from controllerManager
     private List<Gamepad> GetControllerList()
     {
          return ControllerManager.controllerList;
     }

     // start
     // Instantiates all players and assigns them their respective gamepad and spawnpoint
     private void InstantiatePlayers(List<GameObject> playerList, List<Vector2> spawnsPoints, List<Gamepad> controllerList)
     {
               for (int index = 0; index < playerList.Count; index++)
               {
                    GameObject player = Instantiate(original: playerList[index], position: spawnsPoints[index], rotation: Quaternion.identity);
                    PlayerController2D playerProperties = player.GetComponent<PlayerController2D>();
                    playerProperties.assignedSpawnPoint = spawnsPoints[index];
                    playerProperties.assignedController = controllerList[index];
                    Debug.Log("Spawned " + player + " with spawnPoint " + playerProperties.assignedSpawnPoint + " and controller " +
                         playerProperties.assignedController);
               }
     }

     // perform logic depending on the scene
     private void CheckScene(string sceneName)
     {
          //Debug.Log("From ControllerManager, the current scene is " + sceneName);
          if (sceneName.Equals("ReadUpScene"))
          {
               SetNumberOfPlayers();
               SetPlayerList(numberOfPlayers);
          }

          if (sceneName.Equals("Game"))
          {
               InstantiatePlayers(playerList, GetSpawnPoints(), GetControllerList());
          }
     }
}
