using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpaceShipTravel : MonoBehaviour {
    public GameObject[] stopPoints;
    
    int currentTarget;
    int endPoint;
    float distance;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        transform.position = stopPoints[0].transform.position;
        currentTarget = 0;
        endPoint = stopPoints.Length - 1;
        FlyToNextPoint();
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        distance = Vector3.Distance(transform.position, stopPoints[currentTarget].transform.position);
        if (distance <= 2)
            FlyToNextPoint();

        agent.SetDestination(stopPoints[currentTarget].transform.position);
    }

    void FlyToNextPoint()
    {
        currentTarget++;
        if (currentTarget > endPoint)
            currentTarget = 1;
    }
}
