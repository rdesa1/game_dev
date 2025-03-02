using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height; //Can adjust how big your tilemap is. Eventually change it so you can't just hard code it, but rather generate on start.
    [SerializeField] private Tile _tilePrefab; //The tile sprite used to generate the grid
    [SerializeField] private Transform _cam; //Adjusts the camera's position to fit the tilemap

    private Dictionary<Vector2, Tile> _tiles;

    //Currently on Game Start, this generates the grid map. Eventually change this to trigger off of lobby start instead.
    private void Start()
    {   
        GenerateGrid();
    }

    //Generates the grid based off your width and height field parameters. 
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Creates a tile & names it based on coordinate position
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                //Determines if this tile is an odd tile, which helps distinguish an offset color.
                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                //Add tile to tileset for scenes manipulation.
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }


        //Changes the camera's position to the generated tilemap's center.
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -5);
    }

    public Tile getTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }

}
