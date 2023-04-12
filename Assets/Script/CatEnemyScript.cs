using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CatEnemyScript : MonoBehaviour
{
    public GameObject player;
    public GameObject[] patrolPoints;
    private int currentPatrolPointIndex;
    private NavMeshAgent navMeshAgent;

    public float maxAngle = 45;
    public float maxDistance = 2;
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 6.0f;
    public float stoppingDistance = 0.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPatrolPointIndex = 0;
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (SeePlayer())
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.destination = player.transform.position;
        }
        else
        {
            navMeshAgent.speed = patrolSpeed;
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < stoppingDistance)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }



    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        navMeshAgent.destination = patrolPoints[currentPatrolPointIndex].transform.position;
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
    }

    public bool SeePlayer()
    {
        Vector3 vecPlayerTurret = player.transform.position - transform.position;
        if (vecPlayerTurret.magnitude > maxDistance)
        {
            return false;
        }

        Quaternion targetRotation = Quaternion.LookRotation(vecPlayerTurret);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

        Vector3 normVecPlayerTurret = Vector3.Normalize(vecPlayerTurret);
        float dotProduct = Vector3.Dot(transform.forward, normVecPlayerTurret);
        var angle = Mathf.Acos(dotProduct);
        float deg = angle * Mathf.Rad2Deg;
        if (deg < maxAngle)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, vecPlayerTurret);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }
}
