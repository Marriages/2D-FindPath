using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestAstarTilemap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    private void Start()
    {
        Debug.Log($"backgroud  : {background.size}");
        Debug.Log($"wall : {obstacle.size}");

        Debug.Log($"backgroud.origin : {background.origin}");
        Debug.Log($"obstacle.origin : {obstacle.origin}");

        for(int y = background.cellBounds.yMin; y <= background.cellBounds.yMax; y++)
        {
            for(int x = background.cellBounds.xMin;x <= background.cellBounds.xMax; x++)
            {
                TileBase tile =  obstacle.GetTile(new Vector3Int(x, y,0));
                if(tile != null)
                {
                    Debug.Log($"Obstacle Pos :( {x} , {y} )");
                }
            }
        }
    }
}
