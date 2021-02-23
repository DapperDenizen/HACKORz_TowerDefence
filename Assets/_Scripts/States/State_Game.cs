using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Game : State_Base
{
    public override void StateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 10f, 33))
            {
                if (hit.collider.CompareTag("Spawner"))
                {
                    hit.collider.GetComponent<Unit_Spawn>().ShowPath();
                    //print("Dink");
                }
                else if (hit.collider.CompareTag("EmptyZone"))
                {
                    GameHandle.instance.BuyTurret(hit.collider.gameObject);
                }
            }
            

        }
    }
}
