/* This script handles spawn points. */

// Scenes: Game

using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
     public static List<Vector2> spawnPoints;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
          SetSpawnPoints(GridManager.corners);
     }

     // Update is called once per frame
     void Update()
     {

     }

     // start
     // Assign the corner tiles as spawn points.
     void SetSpawnPoints(List<Vector2> cornerTiles)
     {
          foreach (Vector2 cornerTile in cornerTiles)
          {
               spawnPoints.Add(cornerTile);
               Debug.Log("Spawn point " + cornerTile + " added to spawnPoints");
          }
     }
}
