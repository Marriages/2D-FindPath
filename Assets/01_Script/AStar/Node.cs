using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Node : IComparable<Node>
{
    public int x;
    public int y;

    public float G;
    public float H;
    public float F => G + H;
    public Node parent;

    public enum GridType
    {
        Plain=0,
        Wall,
        Monster,
        NextStep,
        Tutorial

    }

    public GridType gridType = GridType.Plain;

    //----------생성자

    public Node(int x, int y, GridType gridType = GridType.Plain)
    {
        this.x = x;
        this.y = y;
        this.gridType = gridType;

        ClearData();
    }

    //데이터를 전부 초기화 시켜줄 목적의 함수
    public void ClearData()
    {
        G=float.MaxValue;
        H=float.MaxValue;
        parent = null;
    }

    //----------생성자


    //이게 없으면 Node리스트의 소팅이 안된다! 그래서 ICompareable 인터페이스를 상속받음.
    //크기를 비교함. returnType int는 나랑 other 를 비교했을 때  0 : 내가 other랑 같다 /  0미만 : 내가 other보다 작다  /  0초과 : 내가 other보다 크다
    public int CompareTo(Node other)
    {
        //코드의 안전을 위해! 근데 other가 null인 경우가 있나?
        if (other == null)
            return 1;

        //F(G+H)는 float. float의 경우 이미 CompareTo가 이미 구현되어있음.
        return F.CompareTo(other.F);
    }


    //명령어 오버로딩. 노드간 연산을 편하게 하기 위함.
    //Equals,GetHashCode,==,!=은 셋트당!
    public override bool Equals(object obj)
    {
        // obj는 Node타입의 node라고 하고, x는 node의 x, y는 node의 y와 같다.
        return obj is Node node && this.x == node.x && this.y == node.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.x, this.y);
    }


    public static bool operator ==(Node left, Vector2Int right)
    {
        //노드위 위치가 같을때의 처리
        return (left.x == right.x) && (left.y==right.y);
    }
    public static bool operator !=(Node left, Vector2Int right)
    {
        //노드위 위치가 같을때의 처리
        return (left.x != right.x) || (left.y != right.y);
    }
    
}
