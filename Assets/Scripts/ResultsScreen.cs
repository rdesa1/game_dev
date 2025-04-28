using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
     public GameObject resultsPanel; // Parent panel for results
     public TextMeshProUGUI resultsText;
     public TextMeshProUGUI[] playerScoresTexts; // One for each player
     private bool resultsShown = false;

     private void Start()
     {
          resultsPanel.SetActive(false); // Hide results panel at the start
     }

     public void ShowResults()
     {
          resultsPanel.SetActive(true);
          resultsShown = true;

          ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>(); // Find ScoreManager

          if (scoreManager == null)
          {
               Debug.LogError("ScoreManager not found!");
               return;
          }

          int highestScore = -1; // Initialize highest score

          // You can continue updating player score texts here...
     }


     public void Rematch()
     {
          ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
          if (scoreManager != null)
          {
               for (int i = 0; i < scoreManager.playerScores.Length; i++)
               {
                    scoreManager.playerScores[i] = 0;
               }
          }
          SceneManager.LoadScene("Game");
     }



     public void QuitToMainMenu()
     {
          ControllerManager.controllerList.Clear(); // Clear list of detected controllers
          ControllerManager.controllerCount = 0; // Reset controller count

          // Clear player setup
          PlayerManager.numberOfPlayers = 0;
          PlayerManager.playerPrefabList.Clear();

          SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
     }


     private Color GetPlayerColor(int playerID)
     {
          switch (playerID)
          {
               case 1: return new Color(1f, 0.25f, 0.25f, 1f);
               case 2: return new Color(0.4f, 0.8f, 1f, 1f);
               case 3: return new Color(0.5f, 1f, 0.5f, 1f);
               case 4: return new Color(0.9f, 0.6f, 1f, 1f);
               default: return Color.white;
          }
     }
}
