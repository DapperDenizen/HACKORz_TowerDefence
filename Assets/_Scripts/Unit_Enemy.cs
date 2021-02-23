using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Enemy : MonoBehaviour
{
    [SerializeField] Transform myTrans;
    bool movin = true;
    public float speed = 5f;
    public float health = 1f;
    Queue<Vector2Int> path;
    //Vector2Int[] path;
    //movement stuff
    Vector2Int target;


    public void GiveDirections(Vector2Int[] pathway)
    {
        path = new Queue<Vector2Int>(pathway);
        StartCoroutine("Follow");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!movin)
        {
            //We reached the end!
            GameHandle.instance.ReachedEnd(1f);
            Die();
        }
    }

    IEnumerator Follow()
    {
        target = path.Dequeue();
        float startTime = Time.time;
        while (true)
        {

            //move towards current target
            float lerpNumb = (Time.time - startTime) / speed;
            Vector2 newPos = Vector2.Lerp(myTrans.position, BoardMaker.IndexToPos(Data.instance.boardSize, target), lerpNumb);
            myTrans.position = new Vector3(newPos.x,newPos.y,myTrans.position.z);
            //check reset
            if (Vector3.Distance(myTrans.position, BoardMaker.IndexToPos(Data.instance.boardSize, target)) < 0.1f)
            {
                if (path.Count == 0) { break; }
                //increment
                target = path.Dequeue();
                startTime = Time.time;

            }
            yield return null;
        }
        movin = false;
    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameHandle.instance.EnemyKilled(1f);
            Die();
        }
    }

    void Die() { Destroy(gameObject); }

}
