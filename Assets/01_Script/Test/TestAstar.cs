using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAstar : MonoBehaviour
{
    private void Start()
    {
        GridMap map = new GridMap(4, 3);

        Node node = map.GetNode(1, 0);
        node.gridType = Node.GridType.Wall;
        node = map.GetNode(2, 2);
        node.gridType = Node.GridType.Wall;

        List<Vector2Int> path = Astar.PathFind(map, new Vector2Int(0, 0), new Vector2Int(3, 2));

        string pathStr = "Path : ";
        foreach(Vector2Int p in path)
        {
            pathStr += $"({p.x}, {p.y}) ->";
        }
        pathStr += " 끝";
        Debug.Log(pathStr);


    }
}
