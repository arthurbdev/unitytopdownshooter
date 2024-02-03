using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public static Action OnEnemyDeath;

    private Rigidbody2D playerRigidbody;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int health;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float range;
    [SerializeField] private float randomRoamingMaxDistance;
    [SerializeField] private float roamingTimeout;
    [SerializeField] private float waitTime;
    [SerializeField] private float awarenessRadius;
    [SerializeField] private float shootingRadius;
    [SerializeField] private float stopChasingRadius;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Gizmo Parameters")]
    public Color awarenessRadiusColor = Color.green;
    public Color shootingRadiusColor = Color.red;
    public Color stopChasingRadiusColor = Color.blue;
    public bool showGizmos = true;

    private enum State
    {
        Patrolling,
        Roaming,
        ChaseTarget,
        ShootAtTarget,
    }
    private State state;
    private State defaultState;

    private float distanceToPlayer;
    private Rigidbody2D rb;
    private FlashSprite flashSprite;
    private Vector2 wayPoint;
    private float roamingCooldown;
    private float wayPointWaitTime;
    private bool isWaiting;
    private int currentPatrolPointIndex;

    private void Awake()
    {
        playerRigidbody = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashSprite = GetComponent<FlashSprite>();
        SetNewDestination();
        if (patrolPoints.Length > 0) defaultState = State.Patrolling;
        else defaultState = State.Roaming;
        state = defaultState;
        wayPointWaitTime = waitTime;
    }

    private void Update()
    {
        if (playerRigidbody != null)
        {
            distanceToPlayer = Vector3.Distance(playerRigidbody.position, rb.position);
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Patrolling:
                Patrol();
                if (distanceToPlayer < awarenessRadius) state = State.ChaseTarget;
                break;
            case State.Roaming:
                RoamRandomly();
                if (distanceToPlayer < awarenessRadius) state = State.ChaseTarget;
                break;
            case State.ChaseTarget:
                ChasePlayer();
                if (distanceToPlayer > stopChasingRadius)
                {
                    SetNewDestination();
                    state = defaultState;
                }
                if (distanceToPlayer < shootingRadius) state = State.ShootAtTarget;
                break;
            case State.ShootAtTarget:
                RotateTowards(playerRigidbody.position);
                weapon.Shoot();
                if (distanceToPlayer > shootingRadius) state = State.ChaseTarget;
                break;
        }
    }

    private void Patrol()
    {
        Vector2 patrolPoint = (Vector2)patrolPoints[currentPatrolPointIndex].position;
        MoveTowards(patrolPoint);
        if (!isWaiting) RotateTowards(patrolPoint);
        else rb.angularVelocity = 0f;

        if (Vector2.Distance(transform.position, patrolPoint) < range)
        {
            if (wayPointWaitTime <= 0)
            {
                wayPointWaitTime = waitTime;
                currentPatrolPointIndex++;
                currentPatrolPointIndex %= patrolPoints.Length;
                isWaiting = false;
            }
            else
            {
                wayPointWaitTime -= Time.fixedDeltaTime;
                isWaiting = true;
            }
        }

    }

    private void ChasePlayer()
    {
        if (playerRigidbody != null)
        {
            MoveTowards(playerRigidbody.position);
            RotateTowards(playerRigidbody.position);
        }
    }

    private void RoamRandomly()
    {
        MoveTowards(wayPoint);
        if (!isWaiting) RotateTowards(wayPoint);
        else rb.angularVelocity = 0f;
        roamingCooldown += Time.fixedDeltaTime;

        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            if (wayPointWaitTime <= 0)
            {
                SetNewDestination();
                roamingCooldown = 0;
                wayPointWaitTime = waitTime;
                isWaiting = false;
            }
            else
            {
                wayPointWaitTime -= Time.fixedDeltaTime;
                isWaiting = true;
            }
        }

        if (roamingCooldown > roamingTimeout)
        {
            SetNewDestination();
            roamingCooldown = 0;
        }
    }

    private void SetNewDestination()
    {
        wayPoint = rb.position - new Vector2(UnityEngine.Random.Range(-randomRoamingMaxDistance, randomRoamingMaxDistance),
        UnityEngine.Random.Range(-randomRoamingMaxDistance, randomRoamingMaxDistance));
    }

    private void MoveTowards(Vector2 direction)
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, direction, moveSpeed * Time.fixedDeltaTime));
    }

    private void RotateTowards(Vector2 direction)
    {
        Vector2 aimDirection = direction - rb.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        rb.velocity = Vector2.zero;
    }

    public void TakeDamage()
    {
        health--;
        flashSprite.Flash();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = awarenessRadiusColor;
            Gizmos.DrawWireSphere(transform.position, awarenessRadius);
            Gizmos.color = shootingRadiusColor;
            Gizmos.DrawWireSphere(transform.position, shootingRadius);
            Gizmos.color = stopChasingRadiusColor;
            Gizmos.DrawWireSphere(transform.position, stopChasingRadius);
            Gizmos.DrawSphere(wayPoint, .5f);
        }
    }


}
