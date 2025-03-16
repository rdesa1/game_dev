using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ReadyUpUI : MonoBehaviour
{
     public TMP_Text instructionText;
     public TMP_Text playerCountText;
     public Button startGameButton;

     private Dictionary<int, Gamepad> readyPlayers = new Dictionary<int, Gamepad>(); // Tracks unique controllers
     public static Dictionary<int, string> RegisteredPlayers = new Dictionary<int, string>();
     private const int MaxPlayers = 4;
     private int minPlayersRequired = 2;

     void Start()
     {
          startGameButton.interactable = false;
          UpdatePlayerCountText();
          Debug.Log($"Registered Players: {ReadyUpUI.RegisteredPlayers.Count}");
     }

     void Update()
     {
          // Single Player Mode: If Enter is pressed, start the game immediately
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

     private void RegisterPlayer(Gamepad gamepad)
     {
          if (!readyPlayers.ContainsValue(gamepad) && readyPlayers.Count < MaxPlayers)
          {
               int playerId = readyPlayers.Count;
               readyPlayers[playerId] = gamepad;

               // Store player ID and controller name before switching scenes
               RegisteredPlayers[playerId] = gamepad.name;

               Debug.Log($"Player {playerId + 1} is ready with {gamepad.name}!");

               UpdatePlayerCountText();
               startGameButton.interactable = readyPlayers.Count >= minPlayersRequired;
          }
     }

     void UpdatePlayerCountText()
     {
          playerCountText.text = $"Players Ready: {readyPlayers.Count} / {MaxPlayers}";
     }

     public void StartGameFromButton()
     {
          if (readyPlayers.Count >= minPlayersRequired)
          {
               StartGame(readyPlayers.Count);
          }
     }

     private void StartGame(int playerCount)
     {
          Debug.Log($"Starting game with {playerCount} players!");
          SceneManager.LoadScene("MapSelection"); // Load the MapSelection scene instead of Game
     }

}
