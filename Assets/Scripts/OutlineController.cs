using UnityEngine;

public class OutlineController : MonoBehaviour
{
     private Material outlineMaterial;

     private void Awake()
     {
          SpriteRenderer sr = GetComponent<SpriteRenderer>();
          if (sr != null)
          {
               outlineMaterial = sr.material;
          }
     }

     public void SetOutlineColor(Color color)
     {
          if (outlineMaterial != null)
          {
               outlineMaterial.SetColor("_OutlineColor", color);
          }
     }
}
