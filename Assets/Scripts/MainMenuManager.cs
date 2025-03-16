using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void SelectPlay()
    {
        SceneManager.LoadScene("ReadyUpScene");
    }

    public void SelectQuit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
