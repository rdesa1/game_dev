using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BeatSwitcher : MonoBehaviour
{
     // Each track has its own bpm that doesn't change for that track.
     const float SECOND_TRACK_BPM = 120f;
     const float THIRD_TRACK_BPM = 140f;
     const float FOURTH_TRACK_BPM = 160f;

     /* The segway between the first and the second track sounds better at 55 seconds
      * because the first track is a 9.6 second loop. */
     const float TIME_BEFORE_SECOND_TRACK = 55f;
     const float TIME_BEFORE_THIRD_TRACK = 66f;
     const float TIME_BEFORE_FOURTH_TRACK = 30f;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          StartCoroutine(ModulateMusic());
     }

     // Update is called once per frame
     void Update()
     {

     }

     /* Obtain the next track in the Resource directory. 
      * The next track is dependant on the current track playing. */
     public AudioClip GetNextTrack()
     {
          AudioClip nextTrack = null;
          AudioSource audioSource = GetComponent<AudioSource>();
          BeatSynchronizer beatManager = GetComponent<BeatSynchronizer>();

          if (audioSource != null)
          {
               string currentTrack = audioSource.clip.name;
               switch (currentTrack)
               {
                    case "Battle tune":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_120");
                         break;

                    case "Battle tune_120":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_140");
                         break;

                    case "Battle tune_140":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_160");
                         break;

                    default:
                         UnityEngine.Debug.Log($"The current track: {audioSource.clip.name} is not a Battle tune track");
                         break;
               }
               if (nextTrack != null)
                    UnityEngine.Debug.Log($"Acquired the next track: {nextTrack.name}.");
               else
                    UnityEngine.Debug.Log($"The next track: {nextTrack.name} was null.");
          }
          else
          {
               UnityEngine.Debug.Log($"The audioSource was null.");
          }
          return nextTrack;
     }

     // Obtain the BPM of the next track
     public float GetNextBPM(AudioClip nextTrack)
     {
          float defaultBPM = 100f;
          if (nextTrack != null)
          {
               switch (nextTrack.name)
               {
                    case "Battle tune_120":
                         return SECOND_TRACK_BPM;

                    case "Battle tune_140":
                         return THIRD_TRACK_BPM;

                    case "Battle tune_160":
                         return FOURTH_TRACK_BPM;

                    default:
                         UnityEngine.Debug.Log("Could not determine the BPM of the next track.");
                         break;
               }
          }
          return defaultBPM;
     }

     private void SetNextTrack(AudioClip nextTrack, float nextBPM)
     {
          if (nextTrack != null)
          {
               AudioSource audioSource = GetComponent<AudioSource>();
               BeatSynchronizer beatManager = GetComponent<BeatSynchronizer>();
               audioSource.Stop();
               audioSource.clip = nextTrack;
               audioSource.Play();
               beatManager.bpm = nextBPM;
          }
     }

     // Waits for the appropriate amount of time between tracks before swapping them.
     IEnumerator ModulateMusic()
     {
          // Swap to the second track after approximately a minute.
          // Timer: 2:06
          // Track: Battle tune_120
          // BPM: 120
          AudioClip nextTrack = GetNextTrack();
          float nextTrackBPM = GetNextBPM(nextTrack);
          yield return new WaitForSeconds(TIME_BEFORE_SECOND_TRACK);
          SetNextTrack(nextTrack, nextTrackBPM);

          // Swap to the third track after another minute.
          // Timer: 1:00.
          // Track: Battle tune_140
          // BPM: 140
          nextTrack = GetNextTrack();
          nextTrackBPM = GetNextBPM(nextTrack);
          yield return new WaitForSeconds(TIME_BEFORE_THIRD_TRACK);
          SetNextTrack(nextTrack, nextTrackBPM);

          // Swap to the final track in the last 30 seconds.
          // Timer: 0:30
          // Track: Battle tune_160
          // BPM: 160
          nextTrack = GetNextTrack();
          nextTrackBPM = GetNextBPM(nextTrack);
          yield return new WaitForSeconds(TIME_BEFORE_FOURTH_TRACK);
          SetNextTrack(nextTrack, nextTrackBPM);
     }
}
