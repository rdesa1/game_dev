using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
     private RectTransform rectTransform;
     private Vector3 originalScale;
     public float pulseScale = 1.2f;
     public float pulseDuration = 0.2f;

     private Image image; // Reference to the UI Image component
     public Color defaultColor = Color.white;
     public Color pulseColor = Color.red;

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

     public void Pulse()
     {
          StopAllCoroutines();
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

          // Reset scale and color
          rectTransform.localScale = originalScale;
          if (image != null)
               image.color = defaultColor;
     }
}
