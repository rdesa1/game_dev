using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{

     // Private variables
     [SerializeField] private float bpm;
     [SerializeField] private AudioSource music; // Handles synchronization of the music
     [SerializeField] private Intervals[] intervals;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
        
     }

     // Update is called once per frame
     void Update()
     {
          Synchronize(music);
     }

     void Synchronize(AudioSource music)
     {
          foreach (Intervals interval in intervals)
          {
               float sampledTime = (music.timeSamples / (music.clip.frequency * interval.GetIntervalLength(bpm)));
               interval.CheckForNewInterval(sampledTime);

          }

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

     public void CheckForNewInterval(float interval)
     {
          if (Mathf.FloorToInt(interval) != lastInterval)
          {
               lastInterval = Mathf.FloorToInt(interval);
               trigger.Invoke();
          }
     }
}
