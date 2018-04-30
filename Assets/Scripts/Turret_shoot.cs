using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_shoot : MonoBehaviour {

	GameObject target;
	float rotationSpeed;
	float attackThreshold;
	float chaseThreshold;
	float giveUpThreshold;
	float attackRepeatTime;
	private float attackTime;
	private bool chasing;
	
	public int gunDamage = 1;
    public float fireRate = 1.5f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
	
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

	private Vector3 shootFrom;

	void Start () {
		rotationSpeed = 3.0f;
		attackThreshold = 10.0f; //May replace with shoot script
		chaseThreshold = 10.0f;
		giveUpThreshold = 20.0f;
		attackRepeatTime = 3.0f;
		attackTime = 0.0f;
		chasing = false;
		target = FindClosestEnemy();
		attackTime = Time.time;

		shootFrom = transform.GetChild(0).position;

		laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
	} 	
	
	void Update () {
		target = FindClosestEnemy();
		float distance = (target.transform.position - transform.position).magnitude;
		if (chasing) {
		 //rotate to look at the enemy
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 100);
			transform.LookAt(target.transform, Vector3.up); 
			if (distance > giveUpThreshold) {
				chasing = false;
		 	}
			/*
			if (distance < attackThreshold && Time.time > attackTime) {
				// Attack! (call whatever attack function you like here)
				attack();
			}*/
			transform.eulerAngles = new Vector3(-transform.eulerAngles.x-90,transform.eulerAngles.y+180, -90);
			//transform.Rotate();
		}
		else {
		 // not currently chasing.
		 // start chasing if target comes close enough
			if (distance < chaseThreshold) {
				chasing = true;
			}
	 	}
		
		//Past the point of weirdness
		if (chasing && Time.time > nextFire)
        {
        	nextFire = Time.time + fireRate;
        	Vector3 rayOrigin = gunEnd.GetChild(0).transform.position;
        	RaycastHit hit;
        	laserLine.SetPosition(0, gunEnd.transform.position);

			StartCoroutine(ShotEffect());

        	if (Physics.Raycast(rayOrigin, -transform.right, out hit, weaponRange))
        	{
        		laserLine.SetPosition(1, hit.point);
            	ShootableBox health = hit.collider.GetComponent<ShootableBox>();

            	if (health != null)
            	{
                	health.Damage(gunDamage);
            	}

            	if (hit.rigidbody != null)
            	{
                	hit.rigidbody.AddForce(-hit.normal * hitForce);
            	}
        	}
        	else
        	{
            	laserLine.SetPosition(1, rayOrigin + (transform.forward * weaponRange));
        	}
			
        }
	}
/*
	public void attack(){
		nextFire = Time.time + fireRate;
        Vector3 rayOrigin = transform.GetChild(0).position;
        RaycastHit hit;
        laserLine.SetPosition(0, target.transform.position);

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, weaponRange))
        {
        	laserLine.SetPosition(1, hit.point);
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();

            if (health != null)
            {
                health.Damage(gunDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (transform.forward * weaponRange));
        }
	}
*/
	public GameObject FindClosestEnemy(){
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Shootable");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos){
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if(curDistance < distance){
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}

/*using UnityEngine;
using System.Collections;

public class RaycastShooter : MonoBehaviour
{

    public int gunDamage = 1;
    public float fireRate = 1.5f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
	
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;


    void Start()
    {
    }
    void Update()
    {
    }


    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
} */