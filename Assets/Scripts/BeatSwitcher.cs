using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BeatSwitcher : MonoBehaviour
{
     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          //CheckIfGameStarted();
          StartCoroutine(SwapTrack());

     }

     // Update is called once per frame
     void Update()
     {

     }

     IEnumerator SwapTrack()
     {
          AudioSource audioSource = GetComponent<AudioSource>();
          BeatManager beatManager = GetComponent<BeatManager>(); // may not need this
          if (audioSource != null)
          {
               Debug.Log("Acquired the audioSource! Now waiting 60 seconds...");
               yield return new WaitForSeconds(5f); // currently 5 seconds for testing
               AudioClip nextClip = Resources.Load<AudioClip>("Audio/medicinal");
               if (nextClip != null)
               {
                    audioSource.clip = nextClip;
                    audioSource.Play();
                    beatManager.bpm = 78; // will change this to a constant value 
               }
               else
               {
                    Debug.Log("nextClip is null!");
               }
               //audioSource.resource = Resources.Load<AudioClip>("Audio/Medicinal");
               //beatManager.bpm = 78;
          }
          else
          {
               Debug.Log("The audioSource was null...");  
          }
          yield return null;
     }

     private void CheckIfGameStarted()
     {
               Debug.Log("The game has started!");
               StartCoroutine("SwapTack");
     }

}
