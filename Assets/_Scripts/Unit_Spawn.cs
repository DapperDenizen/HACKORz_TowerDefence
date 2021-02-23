using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spawn : Unit_Base
{
    public Vector2 indexPosition;
    Vector2Int[] path;

    public void StartUp()
    {
        foreach (Vector2Int playerBase in GameHandle.instance.baseZone)
        {
            PathRequestManager.RequestPath(indexPos, playerBase, GameHandle.instance.currBoard, OnPathFound);
        }
    }


    public void SpawnEnemies(GameObject toSpawn)
    {
        Instantiate(toSpawn, myTrans.position, Quaternion.identity).GetComponent<Unit_Enemy>().GiveDirections(path);
    }


    public void ShowPath()
    {
        //if (path == null) { print("no path head empty"); } else { print(">>" + path.Length); }
        GameHandle.instance.ShowPath(path);
    }

    public void OnPathFound(Vector2Int[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
           // print("Success");
            if (path == null) { path = newPath; return; }

            if (newPath.Length < path.Length)
            {
                path = newPath;
            }
        }
        else { print("Fail"); }
    }
}