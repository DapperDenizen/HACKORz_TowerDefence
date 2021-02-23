using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandle : MonoBehaviour
{
    public static GameHandle instance;
    [SerializeField] SpawnHandler spHandler;
    Transform blockerHolder, baseHolder, SpawnerHolder,PathHolder;
    [SerializeField] GameObject debugObj;
    [SerializeField] LayerMask noTurret;
    List<GameObject> debugPath = new List<GameObject>();
    public BoardNode[,] currBoard;
    public BoardNode[,] turretBoard;
    public List<Vector2Int> baseZone= new List<Vector2Int>();
    public List<GameObject> turretZones = new List<GameObject>();
    float turretCost = 5f;
    //
    [SerializeField]TextMeshProUGUI mainTXT, turrTXT, wallTXT;
    float playerCurrency = 10f;
    int numberOfWalls = 0;
    int maxWalls = 8;
    //
    State_Base curState;
    State_BoardSet stateSet = new State_BoardSet();
    State_Game stateGame = new State_Game();

    //bool buyMode = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        instance = this;
        curState = stateSet;
    }


    void Start()
    {
       currBoard =  BoardMaker.MakeBoard(Data.instance.currBoard);
       turretBoard = new BoardNode[currBoard.GetLength(0), currBoard.GetLength(1)];
       BoardMaker.MakeTurretBoard(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)));
        blockerHolder = new GameObject("Blockers").GetComponent<Transform>();
        baseHolder = new GameObject("AllYourBASE").GetComponent<Transform>();
        SpawnerHolder = new GameObject("Spawners").GetComponent<Transform>();
        PathHolder = new GameObject("Pathways").GetComponent<Transform>();
        SetupForTesting();
        UpdateText();
        curState = stateGame;
    }

    void UpdateText()
    {
        mainTXT.text = "_"+playerCurrency + " Cybits_"; 
        turrTXT.text = "<Turret_Cost> $"+ turretCost +"$"; 
       // wallTXT.text = "<# of Walls> "+ numberOfWalls +"/<"+ maxWalls +">"; 
    }

    void SetupForTesting()
    {
        
        //base @ 5,0
        SetupBases(new Vector2Int(5, 0));
        SetupBases(new Vector2Int(5, 4));

        //Walls
        Vector2Int[] walls = { new Vector2Int(1, 4), new Vector2Int(1, 3), new Vector2Int(4, 0), new Vector2Int(4, 1), new Vector2Int(3, 1), new Vector2Int(3, 3), new Vector2Int(2, 3) };
        for (int i = 0; i < walls.Length; i++)
        {
            SetupWall(walls[i]);
        }
        //spawner @ 0,4
        SetupSpawners(new Vector2Int(0, 4));
        SetupSpawners(new Vector2Int(0, 0));
        SetupSpawners(new Vector2Int(0, 2));
    }

    void SetupBases(Vector2Int pos)
    {
        baseZone.Add(pos);
        Unit_Node baseTemp = Instantiate(Data.instance.playerBaseObj, BoardMaker.IndexToPos(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)), pos), Quaternion.identity,baseHolder).GetComponent<Unit_Node>(); 
        currBoard[pos.x, pos.y].unitObj = (Unit_Base)baseTemp;
        currBoard[pos.x, pos.y].unitObj.indexPos = pos;
    }

    void SetupSpawners(Vector2Int pos)
    {
        Unit_Spawn spawnerTemp = Instantiate(Data.instance.spawnerObj, BoardMaker.IndexToPos(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)), pos), Quaternion.identity,SpawnerHolder).GetComponent<Unit_Spawn>();
        currBoard[pos.x,pos.y].unitObj = (Unit_Base)spawnerTemp;
        currBoard[pos.x, pos.y].unitObj.indexPos = pos;
        spawnerTemp.StartUp();
        spHandler.spawners.Add(spawnerTemp);
    }

    void SetupWall(Vector2Int pos)
    {
        Unit_Wall wallTemp = Instantiate(Data.instance.wallObj, BoardMaker.IndexToPos(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)), pos), Quaternion.Euler(0,0,45),blockerHolder).GetComponent<Unit_Wall>();
        wallTemp.indexPos = pos;
        currBoard[pos.x, pos.y].unitObj = (Unit_Base)wallTemp;
        numberOfWalls++;
        UpdateText();
    }

    public void ShowPath(Vector2Int[] path)
    {
        ClearPath();   
        foreach (Vector2Int pos in path)
        {
            debugPath.Add(Instantiate(debugObj, BoardMaker.IndexToPos(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)), pos), Quaternion.identity,PathHolder));
        }
    }

    void ClearPath()
    {
        foreach (GameObject obj in debugPath) { Destroy(obj); }
    }

    public void BuyTurret(GameObject obj)
    {
        if (playerCurrency < 10f) { print("!no dolol!"); return; }
        playerCurrency -= 5f;
        turretCost += turretCost;
        UpdateText();
        GameObject.Instantiate(Data.instance.gunObj, obj.transform.position, Quaternion.identity);
        Destroy(obj);
    }

    public void ReachedEnd(float val)
    {
        playerCurrency -= val;
        if (playerCurrency < 0)
        {
            print("True death");
        }
        UpdateText();
    }

    public void EnemyKilled(float val)
    {
        playerCurrency += val;
        UpdateText();
    }

    /*
        public void RecievePath(Vector2Int[] waypoints, bool succPath)
        {
            if (succPath)
            {
                for (int i = 0; i < waypoints.Length; i++)
                {
                    print(waypoints[i]);

                    Instantiate(debugObj, BoardMaker.IndexToPos(new Vector2Int(currBoard.GetLength(0), currBoard.GetLength(1)), new Vector2Int(waypoints[i].x, waypoints[i].y)), Quaternion.identity);


                }
            }
        }
    //*/

    public void BuildWall(Vector3 pos)
    {
        //get position to position
        Vector2Int newPos = BoardMaker.PositionToIndex(new Vector2Int(currBoard.GetLength(0),currBoard.GetLength(1)),pos);
        //Check its legal
        //# of walls
        if (numberOfWalls >= maxWalls) { return; }       
        //blocks paths
        SetupWall(newPos);
    }

    /*
    bool TestBoard(Vector2Int newWallLocal)
    {
        //make new board
        BoardNode[,] newBoard = currBoard;
        newBoard[newWallLocal.x, newWallLocal.y].unitObj = new Unit_Wall();
        //ask spawners if at least one of them can go for player
        foreach (Unit_Spawn spawn in spHandler.spawners)
        {
            bool canHit = false;
            foreach (Vector2Int playerBase in GameHandle.instance.baseZone)
            {
                PathRequestManager.RequestPath(spawn.indexPos, playerBase, GameHandle.instance.currBoard, OnPathFound);
            }
        }
        //return true false
        return true;
    }//*/

    // Update is called once per frame
    void Update()
    {
        curState.StateUpdate();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
