using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
     // Loads the ReadyUpScene when the Play button is selected
     public void SelectPlay()
     {
          SceneManager.LoadScene("ReadyUpScene");
     }

     // Quits the application when the Quit button is selected
     public void SelectQuit()
     {
          Application.Quit(); // Close the application (only works in a built game)
          Debug.Log("Game is exiting"); // Log message for debugging (useful for testing in the editor)
     }
}
