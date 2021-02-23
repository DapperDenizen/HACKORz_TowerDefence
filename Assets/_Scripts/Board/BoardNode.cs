using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNode : IHeapItem<BoardNode>
{
    public Unit_Base unitObj;
    public BoardMaker.BoardArea areaType;
    public Vector2Int gridIndex;
    //
    public bool walkable {
        get {
            // if (unitObj != null)
            /// {
            //     return !(unitObj.type == Unit_Base.UnitType.Block);
            // }
           if (unitObj == null) { return true; } //not real
           if (unitObj.type == Unit_Base.UnitType.Block) { return false; } // a blocked section
           return (unitObj.type != Unit_Base.UnitType.Wall); // a blocked section

        }
    }
    public int gCost;
    public int hCost;
    public BoardNode parent;
    int heapIndex;

    public BoardNode(int gridX, int gridY)
    {
        gridIndex = new Vector2Int(gridX, gridY);

    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }

    }
    public int HeapIndex
    {

        get { return heapIndex; }
        set { heapIndex = value; }

    }
    public int CompareTo(BoardNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);

        }
        return -compare;
    }

}
