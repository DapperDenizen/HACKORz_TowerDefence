using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BoardSet : State_Base
{
    public override void StateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    //BuildWall(hit.collider.transform.position);
                }

            }

        }
    }
}
