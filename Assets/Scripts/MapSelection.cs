using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{ 
    public void Select8x8()
    {
        MapSelectionManager.setMapSize(8, 8);
        SceneManager.LoadScene("Game");
    }

    public void Select16x8()
    {
        MapSelectionManager.setMapSize(16, 8);
        SceneManager.LoadScene("Game");
    }
}
