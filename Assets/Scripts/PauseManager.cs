/* This script handles pausing and resuming the game during the Game scene.
 * It ensures that BeatSynchronizer remains in sync after resuming.
 * Pause can be triggered with Escape (keyboard) or Start (controller).
 */

// Scenes: Game

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
     [SerializeField] private GameObject pauseMenuUI; // Reference to the pause menu canvas
     [SerializeField] private Button resumeButton; // Reference to the Resume button
     [SerializeField] private Button quitButton; // Reference to the Quit button

     private BeatSynchronizer beatSynchronizer; // Reference to BeatSynchronizer
     private AudioSource music; // Reference to the music AudioSource
     private bool isPaused = false; // Tracks if the game is currently paused
     private int currentSelection = 0; // Tracks current selected button (0 = Resume, 1 = Quit)

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          beatSynchronizer = FindObjectOfType<BeatSynchronizer>();
          music = beatSynchronizer.GetMusicSource();
          pauseMenuUI.SetActive(false);
     }

     // Update is called once per frame
     void Update()
     {
          // Handle pause/resume input
          if (!isPaused)
          {
               if (Keyboard.current.escapeKey.wasPressedThisFrame || StartButtonPressed())
               {
                    PauseGame();
               }
          }
          else
          {
               HandlePauseMenuNavigation();
          }
     }

     // Checks if any connected gamepad pressed the Start button
     private bool StartButtonPressed()
     {
          foreach (var gamepad in Gamepad.all)
          {
               if (gamepad.startButton.wasPressedThisFrame)
               {
                    return true;
               }
          }
          return false;
     }

     // Handle navigation of the pause menu
     private void HandlePauseMenuNavigation()
     {
          if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame ||
              AnyDpadDown())
          {
               currentSelection = (currentSelection + 1) % 2;
               UpdateButtonSelection();
          }
          else if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame ||
                   AnyDpadUp())
          {
               currentSelection = (currentSelection - 1 + 2) % 2;
               UpdateButtonSelection();
          }

          // Confirm selection
          if (Keyboard.current.enterKey.wasPressedThisFrame || AnyAButtonPressed())
          {
               if (currentSelection == 0)
               {
                    ResumeGame();
               }
               else if (currentSelection == 1)
               {
                    QuitToMainMenu();
               }
          }
     }

     // Update visual selection highlight
     private void UpdateButtonSelection()
     {
          if (currentSelection == 0)
          {
               resumeButton.Select();
          }
          else if (currentSelection == 1)
          {
               quitButton.Select();
          }
     }

     // Pause the game
     private void PauseGame()
     {
          pauseMenuUI.SetActive(true);
          music.Pause(); // Pause the music
          isPaused = true;
          Time.timeScale = 0f;
          currentSelection = 0;
          UpdateButtonSelection();
     }

     // Resume the game
     public void ResumeGame()
     {
          pauseMenuUI.SetActive(false);
          music.Play(); // Resume the music
          isPaused = false;
          Time.timeScale = 1f;
     }

     // Quit the game and load the main menu
     public void QuitToMainMenu()
     {
          Time.timeScale = 1f;
          SceneManager.LoadScene("MainMenu");
     }

     // Detect D-Pad Down input
     private bool AnyDpadDown()
     {
          foreach (var gamepad in Gamepad.all)
          {
               if (gamepad.dpad.down.wasPressedThisFrame)
               {
                    return true;
               }
          }
          return false;
     }

     // Detect D-Pad Up input
     private bool AnyDpadUp()
     {
          foreach (var gamepad in Gamepad.all)
          {
               if (gamepad.dpad.up.wasPressedThisFrame)
               {
                    return true;
               }
          }
          return false;
     }

     // Detect A button input (submit)
     private bool AnyAButtonPressed()
     {
          foreach (var gamepad in Gamepad.all)
          {
               if (gamepad.buttonSouth.wasPressedThisFrame)
               {
                    return true;
               }
          }
          return false;
     }
}
