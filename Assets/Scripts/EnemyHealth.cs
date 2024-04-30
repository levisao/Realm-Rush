using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Enemy))] //para funcionar tem que ter o component Enemy, ent�o quando adicionado esse script a um objeto, tamb�m adiciona o Enemy. N�o adicionar� outro Enemy se j� existir um
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [Tooltip("Adds amount to maxHitpoints when enemy dies.")]
    [SerializeField] int difficultRamp = 1;
    int currentHitPoints = 0;

    Enemy enemy;
    

    bool isDead = false;
    public bool IsDead { get { return isDead; } }
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        
    }
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        currentHitPoints--;

        if (currentHitPoints <= 0)
        {
            //isDead = true;

            
            enemy.RewardGold();
            maxHitPoints += difficultRamp;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
