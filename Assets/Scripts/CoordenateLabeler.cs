using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[ExecuteAlways] //basically called Editor Script
[RequireComponent(typeof(TextMeshPro))]
public class CoordenateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f,0.5f, 0f);

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int(); // mesma coisa do Vector2 só cque com números int
    GridManager gridManager;
    //Waypoint waypoint;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = true;
        //waypoint = GetComponentInParent<Waypoint>(); //Gets a reference to a component of type T on the same GameObject as the component specified, or any parent of the GameObject or it's children.

        DisplayCoordinates(); // chamando aqui para aparecer no jogo também


    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying) //< Editor Mode
        {
            //Debug.Log("HELLOOO "); By my INVESTIGATIONS the update is being called all the time, due to the [ExecuteAlways] in edit mode when we move something
            DisplayCoordinates();
            RenameCoordinate();
        }
        SetLabelColor();
        ToggleLabels();
    }

    void SetLabelColor()
    {
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if (node == null) { return; }

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if(node.isPath)
        {
            label.color = pathColor;
            
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive(); //is gonna set the active state to it's opposite
        }
    }

    private void DisplayCoordinates() //Any code that is played during editor mode, like the onse below, can't be built
                                      // solution: Create a folder in Unity called "Editor", o unity já entende que deve ignorar tudo que tem dentro quando for buildar
                                      // things on that folder cant be in objects, carefull
    {
        if (gridManager == null)
        {
            return;
        }

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);  //outra forma seria cast como int (int)transform.pos... mas ele disse que fez desse jeito porque iria querer fazer coisas com o valor depois
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        // USANDO UnityEditor, não pode usar se for buildar
        /*
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.gridSize.x);  //outra forma seria cast como int (int)transform.pos... mas ele disse que fez desse jeito porque iria querer fazer coisas com o valor depois
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.gridSize.z); // dividindo pelo valor do tamanho da grid lá (não sabia disso)
         * 
         */

        label.text = coordinates.x + ", " + coordinates.y;
    }

    private void RenameCoordinate()
    {
        //transform.parent.name = coordinates.x + ", " + coordinates.y;
        //ou (como o professor fez:
        transform.parent.name = coordinates.ToString();
    }
}


/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Waypoint waypoint;

    void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
    

    ColorCoordinates();
    ToggleLabels();
    }

void ToggleLabels()
{
    if (Input.GetKeyDown(KeyCode.C))
    {
        label.enabled = !label.IsActive();
    }
}

void ColorCoordinates()
{
    if (waypoint.IsPlaceable)
    {
        label.color = defaultColor;
    }
    else
    {
        label.color = blockedColor;
    }
}

void DisplayCoordinates()
{
    coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
    coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

    label.text = coordinates.x + "," + coordinates.y;
}

void UpdateObjectName()
{
    transform.parent.name = coordinates.ToString();
}
}
 */
