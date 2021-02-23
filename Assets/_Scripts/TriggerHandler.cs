using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{

    [SerializeField] Unit_Tower myTower;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            myTower.targetNew(other.transform);

            // if (myTower.target == null) { print("AQUIRED"); myTower.targetNew(other.transform); }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            myTower.targetRemove(other.transform);

            // if (myTower.target == null) { print("AQUIRED"); myTower.targetNew(other.transform); }
        }

        //if (other.transform == myTower.target)
       // {
           // print("LEAVE");
       //     myTower.targetRemove();
       // }
    }
}
