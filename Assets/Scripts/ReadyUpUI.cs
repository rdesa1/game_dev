using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ReadyUpUI : MonoBehaviour
{
     public TMP_Text instructionText; // UI text displaying instructions
     public TMP_Text playerCountText; // UI text displaying the number of ready players
     public Button startGameButton; // Button to start the game

     private Dictionary<int, Gamepad> readyPlayers = new Dictionary<int, Gamepad>(); // Tracks unique controllers
<<<<<<< HEAD
     public static Dictionary<int, string> RegisteredPlayers = new Dictionary<int, string>();
     private const int MaxPlayers = 4;
     private int minPlayersRequired = 2;
=======
     private const int MaxPlayers = 4; // Maximum number of players
     private int minPlayersRequired = 2; // Minimum players needed to enable the start button
>>>>>>> 1f58f05 (robust commenting)

     void Start()
     {
          startGameButton.interactable = false; // Disable start button at launch
          UpdatePlayerCountText();
          Debug.Log($"Registered Players: {ReadyUpUI.RegisteredPlayers.Count}");
     }

     void Update()
     {
          // Single Player Mode: If Enter is pressed on keyboard, start the game immediately
          if (Keyboard.current.enterKey.wasPressedThisFrame)
          {
               StartGame(1);
          }

          // Detect controller Start button (PS5: Options button)
          if (Gamepad.all.Count > 0)
          {
               foreach (Gamepad gamepad in Gamepad.all)
               {
                    if (gamepad.startButton.wasPressedThisFrame)
                    {
                         RegisterPlayer(gamepad);
                    }
               }
          }
     }

     // Registers a new player when they press Start on their controller
     private void RegisterPlayer(Gamepad gamepad)
     {
<<<<<<< HEAD
          if (!readyPlayers.ContainsValue(gamepad) && readyPlayers.Count < MaxPlayers)
          {
               int playerId = readyPlayers.Count;
               readyPlayers[playerId] = gamepad;

               // Store player ID and controller name before switching scenes
               RegisteredPlayers[playerId] = gamepad.name;
=======
          // Ensure the controller is not already registered and the max player count isn't exceeded
          if (!readyPlayers.ContainsValue(gamepad) && readyPlayers.Count < MaxPlayers)
          {
               int playerId = readyPlayers.Count; // Assigns a sequential player number
               readyPlayers[playerId] = gamepad; // Stores the controller reference
>>>>>>> 1f58f05 (robust commenting)

               Debug.Log($"Player {playerId + 1} is ready with {gamepad.name}!");

               UpdatePlayerCountText();
               startGameButton.interactable = readyPlayers.Count >= minPlayersRequired;
          }
     }

     // Updates the UI text to reflect the number of ready players
     void UpdatePlayerCountText()
     {
          playerCountText.text = $"Players Ready: {readyPlayers.Count} / {MaxPlayers}";
     }

     // Starts the game when the start button is pressed
     public void StartGameFromButton()
     {
          if (readyPlayers.Count >= minPlayersRequired)
          {
               StartGame(readyPlayers.Count);
          }
     }

     // Loads the MapSelection scene and begins the game
     private void StartGame(int playerCount)
     {
          Debug.Log($"Starting game with {playerCount} players!");
          SceneManager.LoadScene("MapSelection"); // Load the MapSelection scene instead of Game
     }
}
