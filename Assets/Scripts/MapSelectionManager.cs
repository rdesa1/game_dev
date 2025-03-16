using UnityEngine;

public static class MapSelectionManager
{
     // Stores the selected map width
     public static int width { get; private set; }

     // Stores the selected map height
     public static int height { get; private set; }

     // Sets the map dimensions based on player selection
     public static void setMapSize(int w, int h)
     {
          width = w;
          height = h;
     }
}
