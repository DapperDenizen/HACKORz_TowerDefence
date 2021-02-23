using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ArtData", menuName = "Data/Art", order = 0)]
public class ArtData : ScriptableObject
{
    public Sprite dead, corner, middle, edge;
    public Color tileWhite, tileBlack, tileBlock;
    
}

