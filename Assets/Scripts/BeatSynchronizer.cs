/* This script handles the synchronization of music and player actions based on BPM. */

// Scenes: Game

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BeatSynchronizer : MonoBehaviour
{
     [SerializeField] public float bpm; // Beats per minute of the current track
     [SerializeField] private AudioSource music; // Reference to the audio source component
     [SerializeField] public Intervals[] intervals; // Array of intervals to trigger events on beat

     // TODO: Decouple the work for controller rumble
     /* For vibrating a controller, 
      * the left motor is for higher frequency rumbles, the right motor is for lower frequency rumbles */
     [SerializeField] private float controllerMotorLeft = 0.9f; 
     [SerializeField] private float controllerMotorRight = 0.9f;
     [SerializeField] private float rumbleRate = 0.25f; // 0.25f = every fourth beat

     private bool isRumbling = false; // Track if controllers are currently rumbling
     private float rumbleTimer = 0f; // Timer to control rumble duration
     private const float rumbleDuration = 0.05f; // Duration of controller rumble in seconds

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          // Add rumble to the interval that happens according to rumbleRate
          foreach (Intervals interval in intervals)
          {
               if (Mathf.Approximately(interval.steps, rumbleRate))
               {
                    interval.trigger.AddListener(TriggerControllerRumble);
                    break;
               }
          }
     }

     // Update is called once per frame
     void Update()
     {
          foreach (Intervals interval in intervals)
          {
               float sampledTime = (music.timeSamples / (music.clip.frequency * interval.GetIntervalLength(bpm)));
               interval.CheckForNewInterval(sampledTime);
          }

          // Handle rumble timing manually
          if (isRumbling)
          {
               rumbleTimer += Time.deltaTime;
               if (rumbleTimer >= rumbleDuration)
               {
                    StopAllControllerRumble();
               }
          }
     }

     // Start controller rumble for all connected gamepads
     public void TriggerControllerRumble()
     {
          for (int i = 0; i < Gamepad.all.Count; i++)
          {
               Gamepad.all[i].SetMotorSpeeds(controllerMotorLeft, controllerMotorRight); // Left motor: lower buzz, Right motor: stronger buzz
          }
          isRumbling = true;
          rumbleTimer = 0f;
     }

     // Stop all controller rumble
     private void StopAllControllerRumble()
     {
          for (int i = 0; i < Gamepad.all.Count; i++)
          {
               Gamepad.all[i].SetMotorSpeeds(0f, 0f);
          }
          isRumbling = false;
     }

     // Get the music AudioSource
     public AudioSource GetMusicSource()
     {
          return music;
     }
}

/* This class represents an interval that triggers UnityEvents based on BPM timing. */
[System.Serializable]
public class Intervals
{
     [SerializeField] public float steps; // Number of beats per interval (e.g., 1 = every beat, 0.25 = every 4 beats)
     [SerializeField] public UnityEvent trigger; // Unity event to invoke when the interval triggers
     private int lastInterval; // Tracks the last interval triggered

     // Calculates the interval length based on BPM
     public float GetIntervalLength(float bpm)
     {
          return 60f / (bpm * steps);
     }

     // Checks if a new interval has been reached and triggers the event
     public void CheckForNewInterval(float interval)
     {
          if (Mathf.FloorToInt(interval) != lastInterval)
          {
               lastInterval = Mathf.FloorToInt(interval);
               trigger.Invoke();
          }
     }
}
