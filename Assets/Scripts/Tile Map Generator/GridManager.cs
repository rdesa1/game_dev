/* This script handles the various maps. */

// Scenes: Game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
     [SerializeField] private int _width, _height; //Can adjust how big your tilemap is. Eventually change it so you can't just hard code it, but rather generate on start.
     [SerializeField] private Tile _tilePrefab; //The tile sprite used to generate the grid
     [SerializeField] private Transform _cam; //Adjusts the camera's position to fit the tilemap

     private Dictionary<Vector2, Tile> _tiles;
     public static List<Vector2> corners = new List<Vector2>(); // passed to the SpawnManager for spawnPoints

     //Currently on Game Start, this generates the grid map. Eventually change this to trigger off of lobby start instead.
     private void Start()
     {
          DestroyPreexistingGrid();
          setDimensions(GetWidth(MapSelectionManager.width), GetHeight(MapSelectionManager.height));
          GenerateGrid(_width, _height);
          SetCornerTiles();
     }

     // Takes the width selected from the MapSelectionManager
     private int GetWidth(int MapManagerWidth)
     {
          Debug.Log($"GetWidth returns: {MapManagerWidth}");
          return MapManagerWidth;
     }

     // Takes the height selected from the MapSelectionManager
     private int GetHeight(int MapManagerHeight)
     {
          Debug.Log($"GetHeight returns: {MapManagerHeight}");
          return MapManagerHeight;
     }

     // Destroys grid if it already exists
     private void DestroyPreexistingGrid()
     {
          if (_tiles != null)
          {
               foreach (var tile in _tiles.Values)
               {
                    Destroy(tile.gameObject);
               }
          }
     }

     //Generates the grid based off your width and height field parameters. 
     private void GenerateGrid(int _width, int _height)
     {

          _tiles = new Dictionary<Vector2, Tile>();
          for (int x = -1; x < _width + 1; x++)
          {
               for (int y = -1; y < _height + 1; y++)
               {
                    bool isBorder = x == -1 || x == _width || y == -1 || y == _height;

                    //Creates a tile & names it based on coordinate position (border or tile)
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = isBorder ? $"Border {x} {y}" : $"Tile {x} {y}";

                    //Determines if this tile is an odd tile, which helps distinguish an offset color.
                    var isOffset = (x + y) % 2 == 1;
                    spawnedTile.Init(isOffset, isBorder);

                    //Add tile to tileset for scenes manipulation.
                    if (!isBorder)
                    {
                         _tiles[new Vector2(x, y)] = spawnedTile;
                    }
               }
          }


          //Changes the camera's position to the generated tilemap's center.
          _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -5);
     }

     // Obtain tile at specified position
     public Tile getTileAtPosition(Vector2 pos)
     {
          if (_tiles.TryGetValue(pos, out var tile))
          {
               return tile;
          }

          return null;
     }

     // Set new values for _width and _height
     public void setDimensions(int w, int h)
     {
          _width = w; _height = h;
     }

     // Generate a new grid with current values of _width and _height
     public void GenerateNewGrid()
     {

          GenerateGrid(_width, _height);
     }

     // Get corner tiles for SpawnManager
     public void SetCornerTiles()
     {
          if (GridManager.corners != null)
          {
               GridManager.corners.Clear();

               Vector2 bottomLeft = new Vector2(0, 0);
               Vector2 bottomRight = new Vector2(_width - 1, 0);
               Vector2 topLeft = new Vector2(0, _height - 1);
               Vector2 topRight = new Vector2(_width - 1, _height - 1);

               GridManager.corners.Add(bottomLeft);
               GridManager.corners.Add(bottomRight);
               GridManager.corners.Add(topLeft);
               GridManager.corners.Add(topRight);
          }
          Debug.Log("corners count: " + GridManager.corners.Count);
     }
}