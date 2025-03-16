using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
     private RectTransform rectTransform; // UI element transform
     private Vector3 originalScale; // Original scale before pulsing
     public float pulseScale = 1.2f; // Scale multiplier for the pulse effect
     public float pulseDuration = 0.2f; // Duration of the pulse animation

     private Image image; // Reference to the UI Image component
     public Color defaultColor = Color.white; // Default color of the timer
     public Color pulseColor = Color.red; // Color change during pulse effect

     void Start()
     {
          rectTransform = GetComponent<RectTransform>();
          originalScale = rectTransform.localScale;

          image = GetComponent<Image>(); // Get the Image component
          if (image != null)
          {
               image.color = defaultColor; // Set initial color
          }
     }

     // Triggers the pulsing animation
     public void Pulse()
     {
          StopAllCoroutines(); // Stop any existing pulse effect before starting a new one
          StartCoroutine(PulseEffect());
     }

     private IEnumerator PulseEffect()
     {
          float elapsed = 0f;

          // Change color to pulse color
          if (image != null)
               image.color = pulseColor;

          while (elapsed < pulseDuration)
          {
               float scale = Mathf.Lerp(1f, pulseScale, elapsed / pulseDuration);
               rectTransform.localScale = originalScale * scale;
               elapsed += Time.deltaTime;
               yield return null;
          }

          // Reset scale and color after pulse effect ends
          rectTransform.localScale = originalScale;
          if (image != null)
               image.color = defaultColor;
     }
}
