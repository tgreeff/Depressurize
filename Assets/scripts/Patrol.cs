using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {


    public Transform[] points;
    private int destPoint = 0;
    //private NavMeshAgent agent;
	private Vector3 secondLastDestination;


    void Start()
    {
		GameObject[] objects = gameObject.scene.GetRootGameObjects();
        //agent = GetComponent<NavMeshAgent>();
		secondLastDestination = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // finds nearest point in world if no points have been set up
		if (points.Length == 0) {
			float samePositionLeeway = 0.5f;
			GameObject[] worldNodes = GameObject.FindGameObjectsWithTag ("Waypoint");
			int[] pastWaypointIndices = { -1, -1 };
			int nearestWaypointIndex = 0;
			for (int i = 0; i < worldNodes.Length; i++) {
				//if (pastWaypointIndices[0] == -1 && (worldNodes [i].transform.position - (agent.destination)).sqrMagnitude < samePositionLeeway) {
					//pastWaypointIndices [0] = i;
					//if (i == 0) {
					//	nearestWaypointIndex = (nearestWaypointIndex + 1) % worldNodes.Length;
					//}
				//}
				if (pastWaypointIndices[1] == -1 && (worldNodes [i].transform.position - (secondLastDestination)).sqrMagnitude < samePositionLeeway) {
					pastWaypointIndices [1] = i;
					if (i == 0) {
						nearestWaypointIndex = (nearestWaypointIndex + 1) % worldNodes.Length;
					}
				}
				if (i != pastWaypointIndices[0] && i != pastWaypointIndices[1]) {
					//NavMeshPath oldPath = new NavMeshPath();
					//agent.CalculatePath (worldNodes [nearestWaypointIndex].transform.position, oldPath);
					float oldPathLength = 0.0f;
					//for (int j = 1; j < oldPath.corners.Length; j++) {
						//oldPathLength += Mathf.Abs(Vector3.Distance (oldPath.corners [j - 1], oldPath.corners [j]));
					//}

					//NavMeshPath newPath = new NavMeshPath();
					//agent.CalculatePath (worldNodes [i].transform.position, newPath);
					float newPathLength = 0.0f;
					//for (int j = 1; j < newPath.corners.Length; j++) {
						//newPathLength += Mathf.Abs(Vector3.Distance (newPath.corners [j - 1], newPath.corners [j]));
					//}

					if (newPathLength < oldPathLength) {
						nearestWaypointIndex = i;
					}
				}
			}
			if (worldNodes.Length == 0 || worldNodes.Length == 1 && pastWaypointIndices [0] != -1) { //if no "Waypoint" (besides current), do nothing
				return;
			} else if (worldNodes.Length == 1 && pastWaypointIndices [0] == -1) { //if 1 "Waypoint" and not current, go there
				//secondLastDestination = agent.destination;
				//agent.destination = worldNodes [0].transform.position;
			} else if (worldNodes.Length == 2 && pastWaypointIndices [0] != -1) { //if 2 "Waypoint" and 1 is current, go to other
				//secondLastDestination = agent.destination;
				//agent.destination = worldNodes [1 - pastWaypointIndices[0]].transform.position;
			} else { //go to closest "Waypoint" that's not current or secondLast
				//secondLastDestination = agent.destination;
				//agent.destination = worldNodes [nearestWaypointIndex].transform.position;
			}
		} else {
			// Set the agent to go to the currently selected destination.
			//agent.destination = points [destPoint].position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			destPoint = (destPoint + 1) % points.Length;
		}
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //GotoNextPoint();
    }
}
