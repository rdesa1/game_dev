/* This class is responsible for the UI components of the ReadyUp Scene */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ReadyUpUI : MonoBehaviour
{
     public TMP_Text instructionText; // UI text displaying instructions
     public TMP_Text playerCountText; // UI text displaying the number of ready players
     public Button startGameButton; // Button to start the game

     private int MIN_NUM_FOR_MULTIPLAYER = 2; // Minimum players needed to enable the start button

     private void Awake()
     {
          SetStartButtonEvent();
     }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          startGameButton.interactable = false; // Disable start button at launch
          SetStartButtonEvent();
     }

     // Update is called once per frame
     void Update()
     {
          UpdatePlayerCountText();
          ToggleStartButtonClickability();
          LoadSceneWithKeyboard();
          LoadSceneWithController();
     }

     // start
     // update
     // Updates the counter of players who will play in multiplayer.
     private void UpdatePlayerCountText()
     {
          playerCountText.text = $"Players ready: {ControllerManager.controllerCount} / {PlayerManager.MAX_NUMBER_OF_PLAYERS}";
          //Debug.Log("Controller count from ReadyUpUI: " + ControllerManager.controllerCount);
     }

     // update
     // For multiplayer, enables the play button when enough controllers are detected
     private void ToggleStartButtonClickability()
     {
          if (ControllerManager.controllerCount >= MIN_NUM_FOR_MULTIPLAYER)
          {
               startGameButton.interactable = true;
          }
     }

     // awake
     // Sets up the functionality of the UI start button to load the next scene.
     private void SetStartButtonEvent()
     {
          startGameButton.onClick.AddListener(LoadScene);
     }

     // Loads the next scene.
     private void LoadScene()
     {
          SceneManager.LoadScene("MapSelection");
     }

     // update
     // Loads the next scene when the "Enter" key is pressed. Necessary for singleplayer.
     private void LoadSceneWithKeyboard()
     {
          if (Input.GetKey(KeyCode.Return))
          {
               Debug.Log("Logging the next scene by the Enter Key!");
               LoadScene();
          }
     }

     // update
     // Loads the next scene when the start button on registed controller has been pressed.
     private void LoadSceneWithController()
     {
          if (ControllerManager.controllerCount >= MIN_NUM_FOR_MULTIPLAYER)
          {
               if (Gamepad.current.startButton.wasPressedThisFrame)
               {
                    Debug.Log("Loading the next scene by a controller press!");
                    LoadScene();
               }
          }
     }

}
