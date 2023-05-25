using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Astar
{
    public static List<Vector2Int> PathFind(GridMap grid, Vector2Int start, Vector2Int end)
    {
        return new List<Vector2Int>();
    }

    //이걸 구현하는건 사람 마음!
    static float GetHeuristic(Node current, Vector2Int goal)
    {
        return 0.0f;
    }
    
}
