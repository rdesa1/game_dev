using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
     [SerializeField] private Color baseColor, offSetColor, borderColor; //The main colors of the tiles
     [SerializeField] private SpriteRenderer renderer; //The renderer creating tiles for the player to see.


     public void Init(bool isOffset, bool isBorder = false) //Creates a tile. The Tile's color depends on isOffset and if it's a border tile.
     {
          renderer.color = isBorder ? borderColor : (isOffset ? baseColor : offSetColor);

          if (isBorder) //Makes the border tiles on the Border Layer Mask
          {
               gameObject.layer = LayerMask.NameToLayer("Border");

               //Adds collision to the border tile
               BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
               collider.isTrigger = true;
          }
     }
}
