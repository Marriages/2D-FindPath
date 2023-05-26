using UnityEngine;
using UnityEngine.Tilemaps;

//왼쪽 위를 0,0
/*
 0,0   0,1   0,2   0,3
 1,0   1,1   1,2   1,3
 2,0   2,1   2,2   2,3
 3,0   3,1   3,2   3,3
 
 
 */
public class GridMap
{
    Node[] nodes;

    int width;
    int height;

    Vector2Int origin;      //원점 그리드 좌표

    //위치 입력이 잘못되었다는 것을 표시하기 위한 상수. 관리를 위함.
    public const int Error_NotValid_Position = -1;

    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[height * width];

        for(int y=0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                //2차원 배열을 사용하지 않기 위함
                nodes[index] = new Node(x, y);
            }
        }
    }
    public GridMap(Tilemap background, Tilemap obstacle)
    {
        // nodes생성하기
        // 
    }
    public Node GetNode(int x,int y)
    {
        //Debug.Log($"x:{x}, y:{y}의 GetNode");
        int index = GridToIndex(x, y);
        Node result = null;
        if (index != Error_NotValid_Position)
            result = nodes[index];
        return result;
    }
    public Node GetNode(Vector2Int gridPos)
    {
        return GetNode(gridPos.x, gridPos.y);
    }
    public Node GetNode(Vector3 worldPos)
    {
        return GetNode(WorldToGrid(worldPos));
    }

    public void ClearData()
    {
        foreach(Node node in nodes)
        {
            node.ClearData();
        }
    }

    // -------------------------------------------------------------Utility함수. 좌표 변환 용
    
    public Vector2Int WorldToGrid(Vector3 worldPos) //IndexToGrid가 맞지않나?
    {
        //worldPos 월드좌표를 그리드 좌표로 변환하기 위한 목적의 함수

        return new Vector2Int((int)worldPos.x, (int)worldPos.y);
    }
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        //그리드 좌표 gridPos를 월드좌표로 변환하기 위한 목적의 함수
        return new Vector2(gridPos.x+0.5f, gridPos.y+0.5f);
        //만약 문제가 생길 경우 0.5f씩 더해주기
    }
    private int GridToIndex(int x, int y)
    {
        //Debug.Log($"x:{x},y:{y}의 GridToIndex작업.");
        int index = Error_NotValid_Position;
        if(IsValidPosition(x,y))
        {
            index= x + (height - 1 - y) * width;
        }
        //Debug.Log($"index : {index}");
        // 그리드 x,y를 2차원배열 인덱서로 바꾸기 위한 함수
        return index;       //만약 잘못된 것을 넘겨줄 경우, 에러가 날거임!

        /*
         0,1 1,1 2,1 3,1
         0,0 1,0 2,0 3,0
        를
        0,0 1,0 2,0 3,0 0,1 1,1 2,1 ,3,1
        로 바꾸기위함.
         */
    }

    //입력받은 위치가 내부 위치인지 확인하기 위함. 배열의 범위 또는 그리드 범위 벗어나면 안되니까!
    public bool IsValidPosition(int x,int y)
    {
        //맵 내부면 true 아니면 false가 나올거임
        return (x >= 0 && x < width) && (y >= 0 && y < height);
    }
    public bool IsValidPosition(Vector2Int gridPos)
    {
        //맵 내부면 true 아니면 false가 나올거임
        return IsValidPosition(gridPos.x, gridPos.y);
    }
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node!=null && node.gridType == Node.GridType.Wall;
    }

    public bool IsWall(Vector2Int gridPos)
    {
        return IsWall(gridPos.x, gridPos.y);
    }
    public bool IsMonster(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Monster;
    }
    public bool IsMonster(Vector2Int gridPos)
    {
        return IsMonster(gridPos.x, gridPos.y);
    }


}
