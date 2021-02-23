using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Bullet : Unit_Base
{
    public Vector2 dir;
    public float speed;
    public float dam;
    float lifetime = 1.5f;
    float startTime;
    /*
    public override void Move(Vector3 newPos, Vector2Int newIndex)
    {
        base.Move(newPos, newIndex);
    }
    //*/

    public void Fire(Vector2 dir, float speed, float dam)
    {
        this.dir = dir;
        this.speed = speed;
        this.dam = dam;
        startTime = Time.time;
    }

    private void Update()
    {
        Vector3 newPos = myTrans.position;
        Vector2 movement = dir * Time.deltaTime * speed;
        newPos.x += movement.x;
        newPos.y += movement.y;
        myTrans.position = newPos;
        if (Time.time > (lifetime + startTime)) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Die
            other.GetComponent<Unit_Enemy>().Hit(dam);
            Destroy(gameObject);
        }
    }
}
