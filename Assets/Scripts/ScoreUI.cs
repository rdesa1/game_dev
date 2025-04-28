using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
     public TextMeshProUGUI[] playerTexts;

     private ScoreManager scoreManager;
     private float[] pulseTimers;
     private const float pulseDuration = 0.5f; // How long the pulse lasts
     private const float pulseScale = 1.2f; // How big the text grows during the pulse

     private void Start()
     {
          scoreManager = FindFirstObjectByType<ScoreManager>();

          pulseTimers = new float[playerTexts.Length];
          UpdateColors();
     }

     private void Update()
     {
          if (scoreManager == null) return;

          for (int i = 0; i < playerTexts.Length; i++)
          {
               if (i < PlayerManager.numberOfPlayers)
               {
                    playerTexts[i].gameObject.SetActive(true);

                    string newText = $"P{i + 1}: {scoreManager.playerScores[i]}";

                    // Check if score changed
                    if (playerTexts[i].text != newText)
                    {
                         pulseTimers[i] = pulseDuration; // Start pulse!
                    }

                    playerTexts[i].text = newText;

                    // Handle pulsing animation
                    if (pulseTimers[i] > 0)
                    {
                         float t = pulseTimers[i] / pulseDuration;
                         float scale = Mathf.Lerp(1f, pulseScale, t);
                         playerTexts[i].transform.localScale = new Vector3(scale, scale, 1f);

                         pulseTimers[i] -= Time.deltaTime;
                    }
                    else
                    {
                         playerTexts[i].transform.localScale = Vector3.one;
                    }
               }
               else
               {
                    playerTexts[i].gameObject.SetActive(false);
               }
          }
     }

     private void UpdateColors()
     {
          for (int i = 0; i < playerTexts.Length; i++)
          {
               if (playerTexts[i] == null) continue;

               switch (i)
               {
                    case 0:
                         playerTexts[i].color = new Color(1f, 0.25f, 0.25f, 1f); // Red
                         break;
                    case 1:
                         playerTexts[i].color = new Color(0.4f, 0.8f, 1f, 1f); // Blue
                         break;
                    case 2:
                         playerTexts[i].color = new Color(0.5f, 1f, 0.5f, 1f); // Green
                         break;
                    case 3:
                         playerTexts[i].color = new Color(0.9f, 0.6f, 1f, 1f); // Purple
                         break;
                    default:
                         playerTexts[i].color = Color.white;
                         break;
               }
          }
     }
}
