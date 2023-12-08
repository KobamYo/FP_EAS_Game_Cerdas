using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool collpased;
    public Tile[] tileOptions;

    public void CreateCell(bool collapseState, Tile[] tiles)
    {
        collpased = collapseState;
        tileOptions = tiles;
    }

    public void RecreateCell(Tile[] tiles)
    {
        tileOptions = tiles;
    }
}
