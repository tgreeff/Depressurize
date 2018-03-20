using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject Player;
	public Transform[] blocks;

	public int drawDistance;
	Generation gen;

	//---World Formation Variables---
	public int mode;
	public int mapSize;
	public int difficulty;
	public int seed;

	public int LimitNumberEnemyFactions, LimitNumberEncounters;
	private int numEnemyFactions, numEncounters;    //Numbers - this is the number generated for number of landmarks  																								 
	private Transform[] enemyFactions, encounters;  //Locations - these contain locations for the varying landmarks. Instantciated with the numbers above	
													// Use this for initialization
	void Start () {
		gen = new Generation(drawDistance, blocks);
	}
	
	// Update is called once per frame
	void Update () {
		int x = (int) Player.transform.position.x;
		int z = (int) Player.transform.position.z;

		if (!gen.IsGenerated(x, z + drawDistance)) { //if z + draw is not generated
			gen.GenerateSector(x, z + drawDistance);
		}
		else if (!gen.IsGenerated(x, z - drawDistance)) { //if z - draw is not generated
			gen.GenerateSector(x, z - drawDistance);
		}
		else if (!gen.IsGenerated(x + drawDistance, z)) { //if x + draw is not generated
			gen.GenerateSector(x + drawDistance, z);
		}
		else if (!gen.IsGenerated(x - drawDistance, z)) { //if x - draw is not generated
			gen.GenerateSector(x - drawDistance, z);
		}
	}

	
}
