using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
//THIS WAS MADE USING A YOUTUBE TUTORIAL BY SABASTIAN LAGUE- https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ - specifically the A* Pathfinding tutorial, i do not own this code, however i may have modified it.


public class Pathfinding : MonoBehaviour
{
    [SerializeField]PathRequestManager requestManager;
    BoardNode[,] grid;
    Vector2Int endGoal;


    public void StartFindPath(Vector2Int startPos, Vector2Int targetPos, BoardNode[,] grid, Action<Vector2Int[], bool> callback)
    {
        this.grid = grid;
        StartCoroutine(FindPath(startPos, targetPos));
        endGoal = targetPos;
    }


    IEnumerator FindPath(Vector2Int start, Vector2Int target)
    {
        Vector2Int[] waypoints = new Vector2Int[0];
        bool pathSuccess = false;
        BoardNode startNode = grid[start.x, start.y];
        BoardNode targetNode = grid[target.x, target.y];
        //if (targetNode.unitObj == null)
        //{
            Heap<BoardNode> openSet = new Heap<BoardNode>(Data.instance.currBoard.boardSize.x* Data.instance.currBoard.boardSize.y); // to be evaluated
            HashSet<BoardNode> closedSet = new HashSet<BoardNode>(); // already evaluated
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                BoardNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;

                }
                foreach (BoardNode neighbour in GetNeighbours(currentNode))
                {

                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                    int weighting = 0;
                    if (neighbour.unitObj != null)
                    {
                        weighting = 10;
                    }
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode) + weighting;
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);

                        }

                    }

                }
            }
            //print("Tick");
            yield return null;
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
            }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

        //}
    }

    Vector2Int[] RetracePath(BoardNode start, BoardNode end)
    {
        List<BoardNode> path = new List<BoardNode>();
        BoardNode currentNode = end;
        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //simplify here--------VVVVVV
        Vector2Int[] waypoints;
        //waypoints = SimplifyPath (path);
        waypoints = RegurgitatePath(path);

        Array.Reverse(waypoints);
        return waypoints;

    }
    
    Vector2Int[] RegurgitatePath(List<BoardNode> path)
    {
        
        List<Vector2Int> waypoints = new List<Vector2Int>();

        // adds the actual point of contact!
        waypoints.Add( endGoal);
        //
        for (int i = 1; i < path.Count; i++)
        {
            waypoints.Add(path[i].gridIndex);
        }
        return waypoints.ToArray();
        
    }
    
    /*
    Vector2Int[] SimplifyPath(List<BoardNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        // adds the actual point of contact!
        waypoints.Add(grid.NodeFromWorldPoint(endGoal).worldPosition);
        //
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);

            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    //*/

    int GetDistance(BoardNode nodeA, BoardNode nodeB)
    {

        int distX = Mathf.Abs(nodeA.gridIndex.x - nodeB.gridIndex.x);
        int distY = Mathf.Abs(nodeA.gridIndex.y - nodeB.gridIndex.y);

        if (distX > distY)
        {

            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }


    public List<BoardNode> GetNeighbours(BoardNode node)
    {

        List<BoardNode> neighbours = new List<BoardNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) { continue; }
                //Remove for diagonals
                if (y == x * -1 || x == y * -1 || x == y) { continue; }


                int checkX = node.gridIndex.x + x;
                int checkY = node.gridIndex.y + y;

                if (checkX >= 0 && checkX < Data.instance.currBoard.boardSize.x && checkY >= 0 && checkY < Data.instance.currBoard.boardSize.y)
                {

                    neighbours.Add(grid[checkX, checkY]);

                }
            }
        }
        return neighbours;

    }
}
