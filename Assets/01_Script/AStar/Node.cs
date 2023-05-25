using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
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
        Monster
    }

    public GridType gridType = GridType.Plain;

    //명령어 오버로딩
       
}
