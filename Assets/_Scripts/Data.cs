using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;
    public List<PlayInputs> playInputs;
    public ArtData artData;
    public LevelData levelsData;
    public GameObject tileObj, playerBaseObj, spawnerObj, enemyObj, wallObj, gunObj,turretZoneObj;
    public Vector2Int boardSize { get { return currBoard.boardSize; } }
    public Data_Board currBoard;

    void Awake()
    {

        if (instance != null)
        {
            Destroy(this.gameObject); return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (currBoard == null) { currBoard = levelsData.levels[0]; }
    }
}

[System.Serializable]
public struct PlayInputs
{
    public List<KeyCode> inputs;
}
