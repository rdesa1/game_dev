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
     public static Dictionary<int, string> RegisteredPlayers = new Dictionary<int, string>();
     private const int MaxPlayers = 4;
     private int minPlayersRequired = 2; // Minimum players needed to enable the start button

     void Start()
     {

          //Debug.Log($"Registered Players: {ReadyUpUI.RegisteredPlayers.Count}");

          startGameButton.interactable = false; // Disable start button at launch
          UpdatePlayerCountText();
     }

     void Update()
     {
          // Single Player Mode: If Enter is pressed on keyboard, start the game immediately
          //if (Keyboard.current.enterKey.wasPressedThisFrame)
          //{
          //     StartGame(1);
          //}

          //// Detect controller Start button (PS5: Options button)
          //if (Gamepad.all.Count > 0)
          //{
          //     foreach (Gamepad gamepad in Gamepad.all)
          //     {
          //          if (gamepad.startButton.wasPressedThisFrame)
          //          {
          //               RegisterPlayer(gamepad);
          //          }
          //     }
          //}

          UpdatePlayerCountText();
          TogglePlayButton();
     }

     // update
     // start
     private void UpdatePlayerCountText()
     {
          playerCountText.text = $"Players ready: {ControllerManager.controllerCount} / {PlayerManager.MAX_PLAYERS}";
          //Debug.Log("Controller count from ReadyUpUI: " + ControllerManager.controllerCount);
     }

     // update
     private void TogglePlayButton()
     {
          if (ControllerManager.controllerCount > 1)
          {
               startGameButton.interactable = true;
          }
     }

     // Starts the game when the start button is pressed
     //public void StartGameFromButton()
     //{
     //     if (readyPlayers.Count >= minPlayersRequired)
     //     {
     //          StartGame(readyPlayers.Count);
     //     }
     //}

     // Loads the MapSelection scene and begins the game
     //private void StartGame(int playerCount)
     //{
     //     Debug.Log($"Starting game with {playerCount} players!");
     //     SceneManager.LoadScene("MapSelection"); // Load the MapSelection scene instead of Game
     //}
}
