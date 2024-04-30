using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class ObjectPool : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 30f)] float waitTime = 1f;
    [SerializeField][Range(0f, 50f)] int poolSize = 5;
    [SerializeField] GameObject enemyPrefab;

    GameObject[] pool;
    PathFinder pathFinder;
    GridManager gridManager;

    void Awake()
    {
        pathFinder = FindObjectOfType<PathFinder>();
        gridManager = FindObjectOfType<GridManager>();
        PopulatePool();   
    }


    //bool playing;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void PopulatePool()
    {
        pool = new GameObject[poolSize];
        Vector2Int startCoordinates = pathFinder.StartCoordinates;

        for (int i = 0; i < pool.Length; i++) //criando os inimigos e os deixando disabled
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
            pool[i].transform.position = gridManager.GetPositionFromCoordinates(startCoordinates);
        }
    }
   
    private void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++) 
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return; //found first object that isn't active and activated it, so leave method
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        //int timePool = 0;
        while (true) // will only stop when game stops
        {
            //Instantiate(enemyPrefab,transform); // there are lots of versions os the instantiate method, this one gets de prefab and the trnasform of the parent
            
            EnableObjectInPool();
            yield return new WaitForSeconds(waitTime); //make sure it is never 0, so it doesnt fall into a endless loop

           // timePool++;
        }
    }


    // My version below, waves:

    /*
     * IEnumerator SpawnEnemies()
    {
        int timePool = 0;
        while (timePool < poolSize) // will only stop when game stops
        {
            EnableObjectInPool();
            //Instantiate(enemyPrefab,transform); // there are lots of versions os the instantiate method, this one gets de prefab and the trnasform of the parent
            yield return new WaitForSeconds(waitTime);
            timePool++;
        }
    }
     */

}
