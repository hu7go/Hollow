using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirections
{
    public bool isWater;

    public bool up = false;
    public bool right = false;
    public bool down = false;
    public bool left = false;

    public bool edge = false;
    public bool upEdge = false;
    public bool rightEdge = false;
    public bool downEdge = false;
    public bool leftEdge = false;

    public int wallNeighbours;
}
