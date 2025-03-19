/* This script handles spawn points. */

// Scenes: MapSelection (persist)=> Game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
     public static List<Vector2> spawnPoints = new List<Vector2>();

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          StartCoroutine(WaitForGridManager());
     }

     // Update is called once per frame
     void Update()
     {

     }

     // Coroutine to wait until GridManager.corners is initialized
     private IEnumerator WaitForGridManager()
     {
          while (GridManager.corners == null || GridManager.corners.Count == 0)
          {
               Debug.LogWarning("Waiting for GridManager.corners to be initialized...");
               yield return null;  // Wait for the next frame
          }

          Debug.Log("GridManager.corners is ready!");
          SetSpawnPoints(GridManager.corners);
     }

     // start
     // Assign the corner tiles as spawn points.
     private void SetSpawnPoints(List<Vector2> cornerTiles)
     {
          spawnPoints.Clear(); // Clear previous spawn points to avoid duplicates
          foreach (Vector2 cornerTile in cornerTiles)
          {
               spawnPoints.Add(cornerTile);
               Debug.Log("Spawn point " + cornerTile + " added to spawnPoints");
          }
     }
}
