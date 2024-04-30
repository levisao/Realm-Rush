using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;


    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }


    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();
    /*
     * a prorpiedade em cima faz exatamenteo que um getter method faz:

    public bool GetIsPlaceable()
    {
        return isPlaceable;
    }
    */

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position); 

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
    private void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates)) //pegando o node pela key coordinates e checando o isWalkable e o valor retornado do WillBlockPath para ver se não bloqueará o caminho
        {
        
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);

            //Instantiate(towerPrefab,transform.position, Quaternion.identity);

            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates); //bloqueando, botando o isWalkable par false
                pathFinder.NotifyReceivers();
            }
        }
    }
}
