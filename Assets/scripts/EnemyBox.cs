using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour {

    int health = 2;
    public GameObject dude;

    public void Damage(int amt)
    {
        health -= amt;

        if (health <= 0)
        {
            dude.SetActive(false);
            Invoke("Show", 6);
        }
    }

    public Transform[] points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;


    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

       
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Show()
    {

        dude.SetActive(true);
    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dude.SetActive(false);
            Invoke("Show", 6);
        }
    }

}
