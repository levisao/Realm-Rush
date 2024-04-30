using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Enemy))] //para funcionar tem que ter o component Enemy, então quando adicionado esse script a um objeto, também adiciona o Enemy. Não adicionará outro Enemy se já existir um
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
