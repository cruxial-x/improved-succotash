using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CountTiles : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    GetWidthAndHeightInTiles();
  }

  void CountTileInTilemap()
  {
    // Get the tilemap
    Tilemap tilemap = GetComponent<Tilemap>();
    // Get the bounds of the tilemap
    BoundsInt bounds = tilemap.cellBounds;
    // Create a counter
    int count = 0;
    // Loop through the bounds
    foreach (var pos in bounds.allPositionsWithin)
    {
      // Get the tile at the position
      TileBase tile = tilemap.GetTile(pos);
      // If the tile is not null, increment the counter
      if (tile != null)
      {
        count++;
      }
    }
    // Print the count
    Dev.Log("Number of tiles: " + count);
  }
  void GetWidthAndHeightInTiles()
  {
    // Get the tilemap
    Tilemap tilemap = GetComponent<Tilemap>();
    // Get the bounds of the tilemap
    BoundsInt bounds = tilemap.cellBounds;
    // Get the width and height in tiles
    int width = bounds.size.x;
    int height = bounds.size.y;
    // Print the width and height
    Dev.Log("Width in tiles: " + width);
    Dev.Log("Height in tiles: " + height);
  }
}
