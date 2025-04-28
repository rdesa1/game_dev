using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
     // Stores the selected map width
     public static int width { get; private set; }

     // Stores the selected map height
     public static int height { get; private set; }

     public static List<Vector2> corners;

     // Sets the map dimensions based on player selection
     public static void SetMapSize(int w, int h)
     {
          width = w;
          height = h;
     }

     // Sets the map size to 8x8 and loads the Game scene
     public void Select8x8()
     {
          SetMapSize(8, 8);
          Debug.Log($"Height = {height}. Width = {width}");
          SceneManager.LoadScene("Game");
     }

     // Sets the map size to 16x8 and loads the Game scene
     public void Select16x8()
     {
          SetMapSize(16, 8);
          SceneManager.LoadScene("Game");
     }
}
