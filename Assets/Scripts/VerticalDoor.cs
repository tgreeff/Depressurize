﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoor : MonoBehaviour {
	public float speed;
	private float start;
	private float stop;
	private GameObject door;
	bool left;
	public AudioClip sound; //TODO

	// Use this for initialization
	void Start () {
		if(speed == 0f) {
			speed = 2.5f;
		}
		start = gameObject.transform.position.y;
		stop = start + 5f;
		door = gameObject;
		left = false;
		sound = new AudioClip();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y >= start && left) {
			Vector3 doorPos = door.transform.position;
			door.transform.position = new Vector3(doorPos.x, Time.deltaTime * -speed + doorPos.y, doorPos.z);

		}
		if(gameObject.transform.position.y < start) {
			Vector3 doorPos = door.transform.position;
			door.transform.position = new Vector3(doorPos.x, doorPos.y, doorPos.z);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag.Equals("Player")) {
			//Debug.Log("Player entered the door");
			if(gameObject.transform.position.y <= stop) {
				Vector3 doorPos = door.transform.position;
				door.transform.position = new Vector3(doorPos.x, Time.deltaTime * speed + doorPos.y, doorPos.z);
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag.Equals("Player")) {
			//Debug.Log("Player stayed at door");
			if (gameObject.transform.position.y <= stop) {
				Vector3 doorPos = door.transform.position;
				door.transform.position = new Vector3(doorPos.x, Time.deltaTime * speed + doorPos.y, doorPos.z);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag.Equals("Player")) {
			//Debug.Log("Player left the door");
			left = true;
		}
	}
}
