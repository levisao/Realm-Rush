using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates); //changed because it's too similar
    }

    public List<Node> GetNewPath(Vector2Int coordinates) //overloading method
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates) //passar um vector com as coordenadas do inicio do caminho, seja do inicio do jogo ou quando precisar recalcular o caminho
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;
        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers() //Broadcast message: irá notificar todas as classes monobehaviours q tenham o método específicado
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver); //SendMessageOptions.DontRequireReceiver para não precisar que alguém esteja ouvindo para funcionar e não dar erro
                                            //parametro do metodo RecalculatePath(bool) false
    }
}




//Com explicações :

/*
 * 
 * using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates
    {
        get { return startCoordinates; }
    }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates
    {
        get { return destinationCoordinates; }
    }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down}; //array to determine directions you want the tree to go
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null )
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }

        
    }
    
    void Start()
    {
        

        GetNewPath();
    }

    public List<Node> GetNewPath() //will happen during the game when placing towers
    {
        gridManager.ResetNodes();
        BreadthFirstSearch();
        return BuildPath();
    }

    private void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int directions in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + directions; //vai adicionar ao vetor q ta searching cada uma das direções de cada vez

            if(grid.ContainsKey(neighborCoords)) //vendo se tem na grid (dicionario) a coordenada calculada e adicionando na lista
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach(Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) //não adicionar um node duas vezes e checando se ele é válido isWalkable
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch()
    {

        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(startNode); //queue, fila
        reached.Add(startCoordinates, startNode); // se tá começando o start node então já chegou nele

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue(); //frontier.Dequeue() vai remover o primeiro Node e adicionar ao currentSearchNode
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null) //exploring all connected nodes, moving back through tree till is reaches null
        {
            currentNode = currentNode.connectedTo; // going back
            path.Add(currentNode);                 // massa
            currentNode.isPath = true; //isso fará com que o CoordenateLabeler pinte as coordenadas de laranja
        }
        path.Reverse(); //invertendo para dar o caminho

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates) //checando se botar as torres bloqueará todos os caminhos possíveis
    {
        if (grid.ContainsKey(coordinates)) //safeguard
        {
            bool previousState = grid[coordinates].isWalkable; //safeguard, será sempre true no nosso caso

            grid[coordinates].isWalkable = false; //botando o isWalkable para false para simular um Node bloqueado, para assim o GetNewPath() funcionar corretamente
            List<Node> newPath = GetNewPath(); //fazendo uma lista de qual seria o novo caminho para poder simular se terão caminhos válidos
            grid[coordinates].isWalkable = previousState; //botando o isWalkable para o valor que era antes, no nosso caso sempre será true


            if(newPath.Count <= 1) //se só houver 1 ou 0 Node na lista significa que o caminho está bloqueado
            {
                GetNewPath(); //recalcular o caminho, o isWalkable vltou a ser true já
                return true; //retornará true dizendo que o path foi bloqueado. LEMBRANDO QUE SE chegar em um RETURN, SAI DO MÉTODO
            }

        }
            //se não achou grid retornará falso tb, dcidimos isso
            return false; // se o valor for maior que 1 então retoronu um caminho válido

    }
}
 * 
 */
