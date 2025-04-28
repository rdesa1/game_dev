using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsUI : MonoBehaviour
{
     public TextMeshProUGUI[] playerScoreTexts; // Array of TextMeshProUGUI elements for each player's score
     public TextMeshProUGUI winnerText; // TextMeshProUGUI element for displaying the winner

     private void Start()
     {
          // Initialize and display final scores
          UpdateResultsScreen();
     }

     private void UpdateResultsScreen()
     {
          int highestScore = -1;
          int winningPlayerIndex = -1;
          bool tie = false;

          // Display each player's score
          for (int i = 0; i < playerScoreTexts.Length; i++)
          {
               playerScoreTexts[i].gameObject.SetActive(true);

               playerScoreTexts[i].text = $"Player {i + 1}: {ResultsData.playerScoresCopy[i]}";

               switch (i)
               {
                    case 0:
                         playerScoreTexts[i].color = new Color(1f, 0.25f, 0.25f, 1f); // Red
                         break;
                    case 1:
                         playerScoreTexts[i].color = new Color(0.4f, 0.8f, 1f, 1f); // Blue
                         break;
                    case 2:
                         playerScoreTexts[i].color = new Color(0.5f, 1f, 0.5f, 1f); // Green
                         break;
                    case 3:
                         playerScoreTexts[i].color = new Color(0.9f, 0.6f, 1f, 1f); // Purple
                         break;
                    default:
                         playerScoreTexts[i].color = Color.white; // Default color for unexpected index
                         break;
               }

               // Determine highest score and winner
               if (ResultsData.playerScoresCopy[i] > highestScore)
               {
                    highestScore = ResultsData.playerScoresCopy[i];
                    winningPlayerIndex = i;
                    tie = false;
               }
               else if (ResultsData.playerScoresCopy[i] == highestScore && highestScore != 0)
               {
                    tie = true;
               }
          }

          // Set winner text
          if (tie)
          {
               winnerText.text = "Tie!";
               winnerText.color = Color.white;
          }
          else if (winningPlayerIndex >= 0)
          {
               winnerText.text = $"Player {winningPlayerIndex + 1} Wins!";
               winnerText.color = playerScoreTexts[winningPlayerIndex].color;
          }
          else
          {
               winnerText.text = "No Winner!";
               winnerText.color = Color.white;
          }
     }

     public void Rematch()
     {
          ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>(); // Find ScoreManager

          if (scoreManager != null)
          {
               for (int i = 0; i < scoreManager.playerScores.Length; i++)
               {
                    scoreManager.playerScores[i] = 0; // Reset all scores to 0
               }
          }
          else
          {
               Debug.LogWarning("ScoreManager not found. Cannot reset scores.");
          }

          SceneManager.LoadScene("Game"); // Reload the Game scene
     }

     public void QuitToMainMenu()
     {
          ControllerManager.controllerList.Clear(); // Clear list of detected controllers
          ControllerManager.controllerCount = 0; // Reset controller count
          PlayerManager.numberOfPlayers = 0; // Reset number of players
          PlayerManager.playerPrefabList.Clear(); // Clear player prefab list

          SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
     }
}
