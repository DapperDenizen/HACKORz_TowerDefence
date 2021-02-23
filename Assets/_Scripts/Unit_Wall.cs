using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit_Wall : Unit_Base
{
    public int health = 5;
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        type = UnitType.Wall;
        text.text = "" + health;
    }

    public void Upgrade()
    {
        health += 1;
        text.text = "" + health;
    }

    public void Damage()
    {
        health -= 1;
        text.text = "" + health;
    }
}
