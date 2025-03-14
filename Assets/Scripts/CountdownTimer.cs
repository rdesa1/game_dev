using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
     public float timeRemaining = 180f; // 3 minutes in seconds
     public TextMeshProUGUI timerText;
     private bool timerRunning = true;

     void Start()
     {
          UpdateTimerDisplay();
     }

     void Update()
     {
          if (timerRunning && timeRemaining > 0)
          {
               timeRemaining -= Time.deltaTime;
               UpdateTimerDisplay();
          }
          else if (timeRemaining <= 0)
          {
               timeRemaining = 0;
               timerRunning = false;
               UpdateTimerDisplay();
               OnTimerEnd();
          }
     }

     void UpdateTimerDisplay()
     {
          int minutes = Mathf.FloorToInt(timeRemaining / 60);
          int seconds = Mathf.FloorToInt(timeRemaining % 60);
          timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
     }

     void OnTimerEnd()
     {
          Debug.Log("Timer Ended!");
          // Add any logic here for when the timer reaches 0
     }
}
