using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public static class BoardMaker
{
    public enum BoardType { Rectangle, Circle };
    public enum BoardArea { Middle, Edge, Corner, Empty };
    public static float tileSize =1;

    public static BoardNode[,] MakeBoard( Data_Board boardData)
    {
       // Debug.Log("Offset >> " + (-(boardData.boardSize.x / 2f)) + " , " + (-(boardData.boardSize.y / 2f)));
        Transform holder = new GameObject("Board").GetComponent<Transform>();
        BoardNode[,] boardGameplay = new BoardNode[boardData.boardSize.x, boardData.boardSize.y];
        BoardTile[,] boardVisual;

        switch (boardData.boardType)
        {
            
            case BoardType.Circle: boardVisual = Board_Circle.MakeBoard(boardData.boardSize,boardData.secondaryFloat); break;
            default: boardVisual = Board_Default.MakeBoard(boardData.boardSize); break;
        }

        //float xS = -(boardVisual.GetLength(1) / 2f);
       // float yS = -(boardVisual.GetLength(0) / 2f);
       // Vector2 startPos = new Vector2(yS * tileSize, xS * tileSize);

        for (int x = 0; x < boardVisual.GetLength(0); x++)
        {
            for (int y = 0; y < boardVisual.GetLength(1); y++)
            {
                Vector2 location = IndexToPos(new Vector2Int(boardVisual.GetLength(0), boardVisual.GetLength(1)), new Vector2Int(x, y)); //new Vector2(startPos.x + (tileSize * x), startPos.y + (tileSize * y));
                SpriteRenderer temp = GameObject.Instantiate(Data.instance.tileObj, location, Quaternion.Euler(new Vector3(0,0,boardVisual[x,y].rotation)),holder).GetComponent<SpriteRenderer>();
                temp.color = (x + y) % 2 == 0 ? Data.instance.artData.tileBlack : Data.instance.artData.tileWhite;
                if (temp.GetComponentInChildren<TextMeshPro>() != null)
                {
                    temp.GetComponentInChildren<TextMeshPro>().text = x + "," + y;
                }

                //init node
                boardGameplay[x, y] = new BoardNode(x, y);
                boardGameplay[x, y].areaType = boardVisual[x, y].areaType;
                //

                switch (boardVisual[x, y].areaType)
                {
                    case BoardArea.Corner: temp.sprite = Data.instance.artData.corner; break;
                    case BoardArea.Edge: temp.sprite = Data.instance.artData.edge; break;
                    case BoardArea.Middle: temp.sprite = Data.instance.artData.middle; break;
                    case BoardArea.Empty: temp.color = Data.instance.artData.tileBlock; boardGameplay[x, y].unitObj =  temp.gameObject.AddComponent<Unit_Block>(); break;
                }
                

            }
        }


        return boardGameplay;
    }

    public static List<GameObject> MakeTurretBoard(Vector2Int boardSize)
    {
        Transform holder = new GameObject("TurretPositions").GetComponent<Transform>();
        List<GameObject> turretzones = new List<GameObject>();
        for (int x = 0; x < boardSize.x - 1; x++)
        {
            for (int y = 0; y < boardSize.y - 1; y++)
            {
               turretzones.Add(GameObject.Instantiate(Data.instance.turretZoneObj, IndexToTurretPos(boardSize, new Vector2Int(x, y)), Quaternion.identity, holder));
            }
        }
        return turretzones;
    }

    public static Vector2 IndexToPos(Vector2Int boardSize, Vector2Int position)
    {
        
        float xS = -(boardSize.x / 2f);
        float yS = -(boardSize.y / 2f);
        Vector2 startPos = new Vector2(yS * tileSize, xS * tileSize);
        Vector2 toReturn = new Vector2(startPos.x + (tileSize * position.x), startPos.y + (tileSize * position.y));
        //Debug.Log(" Board " + boardSize + " ||> " + position + " == " + toReturn);
        return toReturn;
    }

    public static Vector2Int PositionToIndex(Vector2Int boardSize, Vector3 pos)
    {

        float xS = (boardSize.x / 2f);
        float yS = (boardSize.y / 2f);

        float x = (pos.y+xS);
        float y = (pos.x + yS);

        return new Vector2Int((int)y, (int)x);

    }

    public static Vector2 IndexToTurretPos(Vector2Int boardSize, Vector2Int position)
    {
        Vector2 toReturn = BoardMaker.IndexToPos(boardSize, position);
        return new Vector2(toReturn.x + 0.5f, toReturn.y + 0.5f);
    }
}

public struct BoardTile
{
    public BoardMaker.BoardArea areaType;
    public float rotation;

    public BoardTile(BoardMaker.BoardArea aType, float rot)
    {
        areaType = aType;
        rotation = rot;
    }
}
