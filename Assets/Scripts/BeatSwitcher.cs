using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BeatSwitcher : MonoBehaviour
{
     const float SECOND_TRACK_BPM = 120;
     const float THIRD_TRACK_BPM = 140;
     const float FOURTH_TRACK_BPM = 160;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          //StartCoroutine(SwapTrack());
     }

     // Update is called once per frame
     void Update()
     {

     }


     public AudioClip GetNextTrack()
     {
          AudioClip nextTrack = null;
          AudioSource audioSource = GetComponent<AudioSource>();
          BeatManager beatManager = GetComponent<BeatManager>();

          if (audioSource != null)
          {
               switch (audioSource.clip.name)
               {
                    case "Battle tune":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_120");
                         if (nextTrack != null)
                              UnityEngine.Debug.Log($"Acquired the next track: {nextTrack.name}.");
                         else
                              UnityEngine.Debug.Log($"The next track: {nextTrack.name} was null.");
                         break;

                    case "Battle tune_120":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_140");
                         if (nextTrack != null)
                              UnityEngine.Debug.Log($"Acquired the next track: {nextTrack.name}.");
                         else
                              UnityEngine.Debug.Log($"The next track: {nextTrack.name} was null.");
                         break;

                    case "Battle tune_140":
                         nextTrack = Resources.Load<AudioClip>("Audio/Battle tune_160");
                         if (nextTrack != null)
                              UnityEngine.Debug.Log($"Acquired the next track: {nextTrack.name}.");
                         else
                              UnityEngine.Debug.Log($"The next track: {nextTrack.name} was null.");
                         break;

                    default:
                         UnityEngine.Debug.Log($"The current track: {audioSource.clip.name} is not a Battle tune track");
                         break;
               }
          }
          else
          {
               UnityEngine.Debug.Log($"The audioSource was null.");
          }
          return nextTrack;
     }

     IEnumerator SwapTrack(AudioClip track)
     {

          AudioSource audioSource = GetComponent<AudioSource>();
          BeatManager beatManager = GetComponent<BeatManager>();
          if (audioSource != null)
          {
               UnityEngine.Debug.Log("Acquired the audioSource! Now waiting 60 seconds...");
               yield return new WaitForSeconds(60f);
               AudioClip nextClip = Resources.Load<AudioClip>("Audio/Battle tune_120");
               if (nextClip != null)
               {
                    audioSource.clip = nextClip;
                    audioSource.Play();
                    beatManager.bpm = SECOND_TRACK_BPM;
               }
               else
               {
                    UnityEngine.Debug.Log("nextClip is null!");
               }
          }
          else
          {
               UnityEngine.Debug.Log("The audioSource was null...");
          }
          yield return null;
     }
}
