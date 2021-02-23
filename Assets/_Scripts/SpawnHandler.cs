using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public List<Unit_Spawn> spawners = new List<Unit_Spawn>();
    Queue<SpawnInstruct> spawns = new Queue<SpawnInstruct>();
    SpawnInstruct currSpawns;
    float spawned = 0;
    bool spawnActive = false;
    //timer
    float targetTime = 0f;

    //Base Game stuff to be removed by better stuff
    float amount = 3f;

    // Update is called once per frame
    void Update()
    {
        if (!spawnActive) { return; }
        if (spawned >= currSpawns.spawnNumber) { IncrementDifficulty(); spawned = 0; spawnActive = false; return; }

        if (Time.time >= targetTime)
        {
            //spawn thing
            foreach (Unit_Spawn spwnr in spawners)
            {
                spawned++;
                spwnr.SpawnEnemies(currSpawns.spawning);
            }


            targetTime = Time.time + currSpawns.timeBetween;
        }

    }

    void IncrementDifficulty()
    {
        amount += 3;
    }

    public void OnTestRun()
    {

        currSpawns = new SpawnInstruct(0.3f, amount, Data.instance.enemyObj);
        spawnActive = true;
    }

}

struct SpawnInstruct
{
    public float timeBetween;
    public float spawnNumber;
    public GameObject spawning;
    public SpawnInstruct(float time, float number, GameObject obj)
    {
        timeBetween = time;
        spawnNumber = number;
        spawning = obj;
    }
}
