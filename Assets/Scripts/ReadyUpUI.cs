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

     private HashSet<int> readyPlayers = new HashSet<int>();
     private const int MaxPlayers = 4;
     private int minPlayersRequired = 2;

     void Start()
     {
          startGameButton.interactable = false;
          UpdatePlayerCountText();
     }

     void Update()
     {
          // Single Player Mode: If Enter is pressed, start the game immediately
          if (Keyboard.current.enterKey.wasPressedThisFrame)
          {
               StartGame(1);
          }
     }

     public void OnPlayerReady(PlayerInput playerInput)
     {
          int playerId = playerInput.playerIndex;
          if (!readyPlayers.Contains(playerId))
          {
               readyPlayers.Add(playerId);
               Debug.Log($"Player {playerId + 1} is ready!");

               UpdatePlayerCountText();

               // Enable start button if enough players are ready
               if (readyPlayers.Count >= minPlayersRequired)
               {
                    startGameButton.interactable = true;
               }
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
          SceneManager.LoadScene("SampleScene"); // Replace with your actual scene name
     }
}
