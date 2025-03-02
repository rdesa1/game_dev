using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, offSetColor; //The two main colors of the tiles
    [SerializeField] private SpriteRenderer _renderer; //The renderer creating tiles for the player to see.

    //Creates a tile. The Tile's color depends on isOffset.
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _baseColor : offSetColor;
    }
}
