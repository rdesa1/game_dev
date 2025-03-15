using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class BeatManager : MonoBehaviour
{
     [SerializeField] private float bpm;
     [SerializeField] private AudioSource music;
     [SerializeField] private Intervals[] intervals;

     // Event that passes beat timestamp
     public static event Action<float> OnBeat;

     // Window size in seconds within which a keypress is considered "on beat"
     [SerializeField] private float beatWindowSize = 0.15f; // 150ms window

     public static float LastBeatTime { get; private set; }
     public static float BeatWindowSize { get; private set; }

     void Awake()
     {
          BeatWindowSize = beatWindowSize;
     }

     void Update()
     {
          foreach (Intervals interval in intervals)
          {
               float sampledTime = (music.timeSamples / (music.clip.frequency * interval.GetIntervalLength(bpm)));
               bool newBeat = interval.CheckForNewInterval(sampledTime);


               if (newBeat)
               {
                    // Store the beat time and broadcast the event
                    LastBeatTime = Time.time;
                    OnBeat?.Invoke(LastBeatTime);
               }
          }
     }

     // Helper method to check if a timestamp was on beat
     public static bool WasOnBeat(float inputTime)
     {
          // Check if input happened within the beat window
          return Mathf.Abs(inputTime - LastBeatTime) <= BeatWindowSize;
     }
}

[System.Serializable]
public class Intervals
{
     [SerializeField] private float steps;
     [SerializeField] private UnityEvent trigger;
     private int lastInterval;

     public float GetIntervalLength(float bpm)
     {
          return 60f / (bpm * steps);
     }

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