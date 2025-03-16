using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
     public float timeRemaining = 180f; // 3 minutes in seconds
     public TextMeshProUGUI timerText; // UI text element for displaying the timer
     private bool timerRunning = true; // Flag to track whether the timer is running

     void Start()
     {
          // Initialize the timer display
          UpdateTimerDisplay();
     }

     void Update()
     {
          // Decrease time if the timer is running and there's time left
          if (timerRunning && timeRemaining > 0)
          {
               timeRemaining -= Time.deltaTime;
               UpdateTimerDisplay();
          }
          else if (timeRemaining <= 0)
          {
               // Ensure the timer doesn't go negative and stop it
               timeRemaining = 0;
               timerRunning = false;
               UpdateTimerDisplay();
               OnTimerEnd();
          }
     }

     // Updates the UI text to display the remaining time in minutes and seconds
     void UpdateTimerDisplay()
     {
          int minutes = Mathf.FloorToInt(timeRemaining / 60);
          int seconds = Mathf.FloorToInt(timeRemaining % 60);
          timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
     }

     // Handles logic when the timer reaches zero
     void OnTimerEnd()
     {
          Debug.Log("Timer Ended!");
          // Additional logic for when the timer reaches zero can be added here
     }
}
