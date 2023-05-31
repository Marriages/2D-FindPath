using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TestAstarTilemap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Transform start;
    public Transform end;

    public PathLine line;

    GridMap map;
    private void Start()
    {
        map = new GridMap(background, obstacle);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.LClick.performed += OnLCLick;
        inputActions.Test.RClick.performed += OnRCLick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.RClick.performed -= OnRCLick;
        inputActions.Test.LClick.performed -= OnLCLick;
        base.OnDisable();
    }
    private void OnLCLick(InputAction.CallbackContext _)
    {
        //마우스를 클릭한 곳의 좌표를 받아옴(이건 로컬임)
        Vector2 screenPos = Mouse.current.position.ReadValue();
        //Debug.Log(screenPos);
        // 그 좌표를 월드 기준으로 바꿔주는건 카메라가 해준다
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        //Debug.Log(worldPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);
        //Debug.Log(gridPos);

        if( !map.IsWall(gridPos) && !map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            start.position = finalPos;
        }
    }
    private void OnRCLick(InputAction.CallbackContext _)
    {
        //마우스를 클릭한 곳의 좌표를 받아옴(이건 로컬임)
        Vector2 screenPos = Mouse.current.position.ReadValue();
        // 그 좌표를 월드 기준으로 바꿔주는건 카메라가 해준다
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (!map.IsWall(gridPos) && !map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            end.position = finalPos;
        }

    }

    protected override void Test1(InputAction.CallbackContext obj)
    {
        Vector2Int startGrid = map.WorldToGrid(start.position);
        //Debug.Log($"Start : {startGrid}");
        Vector2Int endGrid = map.WorldToGrid(end.position);
        //Debug.Log($"Start : {endGrid}");

        List<Vector2Int> path = Astar.PathFind(map, startGrid, endGrid);
        line.DrawPath(map,path);
    }
    protected override void Test2(InputAction.CallbackContext obj)
    {
        TestGetTile();
    }

    private void TestGetTile()
    {
        Debug.Log($"background.size.x : {background.size.x}");
        Debug.Log($"background.size.y : {background.size.y}");
        Debug.Log($"background.cellBounds.xMin : {background.cellBounds.xMin}");
        Debug.Log($"background.cellBounds.xMax : {background.cellBounds.xMax}");


        /*
        Debug.Log($"backgroud  : {background.size}");
        Debug.Log($"wall : {obstacle.size}");

        Debug.Log($"backgroud.origin : {background.origin}");
        Debug.Log($"obstacle.origin : {obstacle.origin}");

        for (int y = background.cellBounds.yMin; y <= background.cellBounds.yMax; y++)
        {
            for (int x = background.cellBounds.xMin; x <= background.cellBounds.xMax; x++)
            {
                TileBase tile = obstacle.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    Debug.Log($"Obstacle Pos :( {x} , {y} )");
                }
            }
        }*/
    }
}
