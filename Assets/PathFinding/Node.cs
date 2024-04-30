using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //serializing class to appear on spector, being called by the GridManager
public class Node //will function basically as a Data Container
{
    public Vector2Int coordinates; //will not have methods and doesn't inherits from MonoBehaviour, so there's no problem making public variables
    public bool isWalkable; //or isSearchable, to know if the node can be added to the tree
    public bool isExplored; 
    public bool isPath;
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
