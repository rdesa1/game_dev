using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
     // Sets the map size to 8x8 and loads the Game scene
     public void Select8x8()
     {
          MapSelectionManager.setMapSize(8, 8);
          SceneManager.LoadScene("Game");
     }

     // Sets the map size to 16x8 and loads the Game scene
     public void Select16x8()
     {
          MapSelectionManager.setMapSize(16, 8);
          SceneManager.LoadScene("Game");
     }
}
