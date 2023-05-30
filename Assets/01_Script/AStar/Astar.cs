using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/*
 * A*알고리즘 구하는 순서! ( 외워둘 것 )
 * 1. 오픈리스트에 G,H를 구한 시작위치를 넣음.
 * 2. 오픈리스트에서 F값이 가장 작은걸 하나 꺼냄.
 */
public static class Astar
{
    public static List<Vector2Int> PathFind(GridMap gridMap,Vector2Int start, Vector2Int end)
    {
        //이거를 하면 지형별 이동속도 감소 효과를 넣기가 어려움. 직선, 대각선 거리에 따른 값을 지정해버리니까.
        const float sideDistance = 1.0f;        //직선거리
        //const float diagonalDistance = 1.4f;    //대각선거리

        gridMap.ClearData();
        List<Vector2Int> path = null; //최종적으로 리턴시킬 길. null이 되는 경우는? 목적지가 안 나왔을 경우 + 파라메터가 비정상일경우

        if(gridMap.IsValidPosition(start) && gridMap.IsValidPosition(end))
        {
            //올바른 경로일 경우만 실행됨.
            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            //--------------------------------- A*의 시작 부분 ---------------------------------
            Node current = gridMap.GetNode(start);      //맨처음 시작위치를 오픈리스트에 넣어야함.
            current.G = 0;      //시작이니까 특수하게 0을 넣어줌.
            current.H = GetHeuristic(current, end);     // 시작위치의 휴리스틱을 구해줌.
            open.Add(current);  //오픈리스트에 넣기 전,G,H값을 구해야 하므로 위의 단계를 수행해 줌.


            //--------------------------------- A*의 핵심 루틴 ---------------------------------
            //openlist가 아무것도 없다? 길을 못 찾는다. 나 못 감!!
            while (open.Count > 0)       // Openlist의 값이 있다면? 계속 돌려!!
            {
                //1. openlist에서 f값이 가장 작은걸 꺼내야함.
                open.Sort();        // 정렬시켜버려! 이를 위해 Node에서 CompareTo를 구현했음.
                current = open[0];
                open.RemoveAt(0);

                //2. openlist에서 꺼낸 값이 목표지점인지 검사하기. 도착이면 끝내기.
                if(current != end)      //노드와 Vector2Int를 비교할 수 있도록 node쪽에 ==, !=를 구현해놨음.
                {
                    //도착이 아님! -> current를 close
                    close.Add(current);
                    //주변의 모든 칸을 다 넣어버리기!    나는 위아래왼쪽아래만...
                    for(int y=-1;y<=1;y++)
                    {
                        for(int x=-1;x<=1;x++)
                        {
                            //Debug.Log($"Astar : {current.x+x}, {current.y+y}");
                            Node node = gridMap.GetNode(current.x+x,current.y+y);

                            // 스킵!!!-----------
                            if (node == null)       //만약 범위를 벗어난 값을 구하려고 한다면, 걸러줘야함!
                                continue;
                            if(node == current)
                                continue;
                            if (node.gridType != Node.GridType.Plain)
                                continue;
                            if (close.Exists((x) => x == node))      //close에 있냐 없냐!! 물어보는 C#을 람다로 구현      이 식은 이해가 조금 부족하니 추가 공부필요
                                continue;
                            bool isDiagonal = (Mathf.Abs(x) == Mathf.Abs(y));             //Diagonal : 대각선  -> 내가 하고자 하는건 이친구는 피료 없다!
                            // 벽인지 벽이 아닌지를 판단하는 구문! 사이에 있는 노드가 벽이냐!  -> 대각선이고, 둘중하나라도 벽이면!          이부분은 다시 고민해보기

                            if (isDiagonal)
                                continue;
                            /*
                            if(isDiagonal &&
                                ( gridMap.GetNode(current.x + x, current.y).gridType==Node.GridType.Wall ||
                                  gridMap.GetNode(current.x,   current.y+y).gridType==Node.GridType.Wall    ) )
                            {
                                continue;
                            }
                            // 스킵끝!-----------

                            //이제 current를 오픈리스트에 넣을건데, 이미 오픈리스트에 있으면 넣을 필요가 없음.
                            //exist로 처리를 해도 되지만, parent를 이용하고자 함.
                            float distance;
                            if (isDiagonal)
                                distance = diagonalDistance;
                            else
                                distance = sideDistance;
                            */
                            //내가 갈 곳의 G값이 내 이동거리보다 크다면? 값을 갱신해야함.
                            if(node.G > current.G+ sideDistance)
                            {
                                //만약 여기에 휴리스틱이 계산안된 친구가 들어온다면?? 혹시모르니...
                                //휴리스틱이 계산안된 친구? 오픈리스트에 없던 휴리스틱이 계산안된 친구.
                                if(node.parent == null)
                                {
                                    node.H = GetHeuristic(node, end);
                                    open.Add(node);
                                }
                                node.G = current.G + sideDistance;
                                node.parent = current;
                            }


                        }
                    }
                }
                else
                {
                    break;      //도착일 경우 그대~로 종료
                }

            }


            //-------------------------- A*의 마무리 작업(Path만들기) --------------------------
            if(current==end)        //목적지에 잘 도착함
            {
                path = new List<Vector2Int>();
                Node result = current;
                while(result!=null)     //시작지점은 parent가 null이니, null이 아니면 계속 돌아돌아
                {
                    path.Add(new Vector2Int(result.x, result.y));       //마지막부터 넣네..? 마지막에 reverse가필요함.
                    result = result.parent;
                }
                path.Reverse();     //뒤집어버렷!
            }
            // 그렇지 안흥면 위에 설정한대로 null로 진행!

        }


        return path;
    }

    //이걸 구현하는건 사람 마음!
    static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.x - end.x)+Mathf.Abs(current.y - end.y);
    }
    
}
