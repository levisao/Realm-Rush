using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathfinder;

    void OnEnable()
    {
        RecalculatePath(true); //true porque aqui é no começo do jogo, quando o primeiro node para calcular o path eh o startCoordinates
        ReturnToStart();
    }

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<PathFinder>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines(); //stop the coroutine before getting new path
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath()); //starting the coroutine again to follow the new path
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
        ReturnToStart();
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        Debug.Log("CU DE ZEBRA");
        FinishPath();
    }
}



























/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour //vai pegar o caminho do caminho, fazendo uma lista de cada coordenada dos tales que ele vai andar em ordem
{
    
    [SerializeField] [Range(0f, 5f)] float enemySpeed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;


    void OnEnable() //called whenever an object is enabled
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }
    void FindPath()
    {
        
        path.Clear();

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Path");    // pegando todos os objetos com a tag path (não funcionou pq n da pra garantir a ordem

        foreach (GameObject waypoint in waypoints)
        {
            path.Add(waypoint.GetComponent<Waypoint>());     //correndo pela lista de objetos com path e adicionando a outra lista path
        }
        

        path.Clear();

        path = pathFinder.GetNewPath();
        
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
        //Destroy(gameObject);

    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinated(pathFinder.StartCoordinates); // começando pelo primeiro objeto da path
    }
    IEnumerator FollowPath()
    {
        for(int i = 0; i < path.Count; i ++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinated(path[i].coordinates);
            float travelPercent = 0f; //when it's 1 it's the end of the lerp, so controlling the speed it updates changes the speed of the movement

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * enemySpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame(); //new
            }
        }
    }

    
}
 * 
 */




