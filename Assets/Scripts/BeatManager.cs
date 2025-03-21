using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{

     // Private variables
     [SerializeField] private float bpm;
     [SerializeField] private AudioSource music; // Handles synchronization of the music
     [SerializeField] public Intervals[] intervals;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
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
     [SerializeField] public float steps;
     [SerializeField] public UnityEvent trigger;
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
               //Debug.Log("This is a beat");
          }
     }
}