using UnityEngine;

public static class MapSelectionManager
{
    public static int width { get; private set; }
    public static int height { get; private set; }

    public static void setMapSize(int w, int h)
    {
        width = w;
        height = h;
    }
}

