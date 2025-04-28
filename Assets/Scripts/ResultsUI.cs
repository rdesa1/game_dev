using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsUI : MonoBehaviour
{
     public TextMeshProUGUI[] playerScoreTexts; // Array of TextMeshProUGUI elements for each player's score

     private void Start()
     {
          // Initialize and display final scores
          UpdateResultsScreen();
     }

     private void UpdateResultsScreen()
     {
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
          SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
     }
}
