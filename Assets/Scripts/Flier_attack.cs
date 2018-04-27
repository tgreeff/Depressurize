using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flier_attack : MonoBehaviour {

    public Transform target;
    public Transform bullet;

    public float distance;
    public float LOS_Distance;
    public float attack_Distance;
    public float min_Distance;

    public float rotDamp = 2;
    public float shots = 0.5f;
    public float shotTime = 0;


    // Use this for initialization
    void Start () {
        LOS_Distance = 30;
        attack_Distance = 20;
        min_Distance = 3;
	}
	
	// Update is called once per frame
	void Update () {
        distance = Vector3.Distance(target.position, transform.position);

        if (distance > LOS_Distance)
        {
            scopeIn();

            if (distance <= attack_Distance && (Time.time - shotTime) > shots)
            {
                shoot();
            }
        }
    }

    void scopeIn()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotDamp);
    }

    void shoot()
    {
        shotTime = Time.time;
        Instantiate(bullet, transform.position + (target.position - transform.position).normalized, Quaternion.LookRotation(target.position - transform.position));
    }
}
