using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Tower : Unit_Base
{
    [SerializeField] Transform myTurret,guntip;
    [SerializeField] GameObject projectile;
    bool fightTime = true;//false;
    float health = 10;
    float sightRange = 1.5f; //unit radius
    [SerializeField] float fireSpeed = 10f; //per second
    [SerializeField] float bulletSpeed = 4; //per second
    [SerializeField] float bulletDamage = 1; //per second
    public Transform target = null;
    bool targeting = false;
    List<Transform> backUpTargets = new List<Transform>();
    //
    float fireTimeTarget = 0;

    // Start is called before the first frame update
    void start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Target procedure
        if (targeting && (target == null)) { targetRemove(null); }

        //move to target
        if (!targeting)
        {
            LookToTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //myTurret.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(0, 0, 1));
        }
        else
        {
            if (targeting)
            {
                if (target == null) { return; }
                LookToTarget(target.position);
                //myTurret.LookAt(target, new Vector3(0, 0, 1));
            }
        }

        //shooting
        if (!targeting) { return; }

        fireTimeTarget -= Time.deltaTime;

        if (fireTimeTarget <= 0)
        {
            Fire();
            fireTimeTarget = fireSpeed;
        }
        
    }

    void Fire()
    {
        Instantiate(projectile, guntip.position, Quaternion.identity).GetComponent<Unit_Bullet>().Fire(fireDir(),bulletSpeed,bulletDamage);
    }

    Vector2 fireDir()
    {
        //return guntip.position - target.position;
        return target.position - guntip.position;
    }

    void LookToTarget(Vector3 lookTarget)
    {
        if (lookTarget == myTrans.position) { return; }
        float c = Vector2.Distance(myTrans.position, lookTarget);
        float b = 1f; // b is made up to make a triangle
        float a = Vector2.Distance(((Vector2)myTrans.position+Vector2.up), lookTarget);

        float cosA = Mathf.Pow(b, 2) + Mathf.Pow(c, 2) - Mathf.Pow(a, 2);
        cosA = cosA / (2 * b * c);
        cosA = Mathf.Acos(cosA);
        cosA = Mathf.Rad2Deg * cosA;
        if (lookTarget.x > myTrans.position.x) { cosA = cosA * -1; }
        myTurret.rotation = Quaternion.Euler(0, 0, cosA);
    }

    void CheckForNewTarget()
    {
        if (backUpTargets.Count > 0)
        {
            targeting = true;
            target = backUpTargets[0];
            backUpTargets.RemoveAt(0);
        }
    }


    public void targetNew( Transform ntarg)
    {

        backUpTargets.Add(ntarg);
        if (target == null)
        {
            CheckForNewTarget();
        }
            //targeting = true; target = ntarg;
        
    }

    public void targetRemove(Transform xtarg)
    {
        if (xtarg == target)
        {
            targeting = false;
            target = null;
            CheckForNewTarget();
        }
        else
        {
            backUpTargets.Remove(xtarg);
        }
    }


}
