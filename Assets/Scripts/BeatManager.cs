using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class BeatManager : MonoBehaviour
{
     [SerializeField] private float bpm; // Beats per minute of the music
     [SerializeField] private AudioSource music; // Audio source for the music
     [SerializeField] private Intervals[] intervals; // Array of intervals to trigger events

     // Event that is triggered on each beat, passing the beat timestamp
     public static event Action<float> OnBeat;

     // Window size in seconds within which a keypress is considered "on beat"
     [SerializeField] private float beatWindowSize = 0.15f; // 150ms window

     public static float LastBeatTime { get; private set; } // Timestamp of the last beat
     public static float BeatWindowSize { get; private set; } // Static access to beat window size

     void Awake()
     {
          // Initialize the beat window size
          BeatWindowSize = beatWindowSize;
     }

     void Update()
     {
          // Iterate through all defined intervals
          foreach (Intervals interval in intervals)
          {
               // Calculate the current position in beats
               float sampledTime = (music.timeSamples / (music.clip.frequency * interval.GetIntervalLength(bpm)));
               bool newBeat = interval.CheckForNewInterval(sampledTime);

               if (newBeat)
               {
                    // Store the timestamp of the latest beat and invoke event
                    LastBeatTime = Time.time;
                    OnBeat?.Invoke(LastBeatTime);
               }
          }
     }

     // Helper method to check if a given timestamp falls within the beat window
     public static bool WasOnBeat(float inputTime)
     {
          return Mathf.Abs(inputTime - LastBeatTime) <= BeatWindowSize;
     }
}

[System.Serializable]
public class Intervals
{
     [SerializeField] private float steps; // Number of beats per interval
     [SerializeField] private UnityEvent trigger; // Event triggered when the interval occurs
     private int lastInterval; // Stores the last interval index

     // Calculates the length of each interval in seconds
     public float GetIntervalLength(float bpm)
     {
          return 60f / (bpm * steps);
     }

     // Checks if a new interval has been reached and triggers event if so
     public bool CheckForNewInterval(float interval)
     {
          if (Mathf.FloorToInt(interval) != lastInterval)
          {
               lastInterval = Mathf.FloorToInt(interval);
               trigger.Invoke();
               return true;
          }
          return false;
     }
}