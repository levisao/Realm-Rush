using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;

    Transform initialTarget;
    Transform currentTarget;

    void Start()
    {
        FindClosestTarget();
        initialTarget = currentTarget;
    }

    void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
        {
            float targetDistance = Vector3.Distance(transform.position, currentTarget.position);
            weapon.LookAt(currentTarget);

            if (targetDistance < range)
            {
                Attack(true);
            }
            else
            {
                // Target is out of range, find a new target
                FindClosestTarget();
                // Update initial target to the new one if it's not null
                if (currentTarget != null)
                {
                    initialTarget = currentTarget;
                }
                Attack(false);
            }
        }
        else
        {
            // Target is dead or null, find a new target
            FindClosestTarget();
            // Update initial target to the new one if it's not null
            if (currentTarget != null)
            {
                initialTarget = currentTarget;

            }
            Attack(false);
        }
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < maxDistance)
            {
                currentTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}


// codigo do professor
/*
 * 
 * 
 * 
 * 
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] Transform target;
    Transform closestTarget = null;


    [SerializeField] float range = 15f;

    void Start()
    {
        //target = FindAnyObjectByType<Enemy>().transform; //tem que ser assim por que terão inimigos diferentes, então tem q atribuir o valor durante o jogo mesmo
    }

    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget() // ok for small game
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float maxDistance = Mathf.Infinity;



        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
            


        }

        
            target = closestTarget;
        



    }

    void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);

        if (targetDistance < range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission; //fazer essa gambiarra para poder mudar o enabled?
        emissionModule.enabled = isActive;
    }
}


 * 
 * 
 * 
 * 
 */