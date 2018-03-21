using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject Player;
	public Transform[] blocks;
	public int currentSectorX;
	public int currentSectorY;

	public int drawDistance;
	private Generation gen;

	//---World Formation Variables---
	public int mapSize;
	public int seed;

	public int LimitNumberEnemyFactions, LimitNumberEncounters;
	private int numEnemyFactions, numEncounters;    //Numbers - this is the number generated for number of landmarks  																								 
	private Transform[] enemyFactions, encounters;  //Locations - these contain locations for the varying landmarks. Instantciated with the numbers above	
													// Use this for initialization
	void Start () {
		int sectorX = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);
		int sectorY = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);

		gen = new Generation(drawDistance, blocks, sectorX, sectorY);

		for (int x = -drawDistance; x < drawDistance; x++) {
			for (int y = -drawDistance; y < drawDistance; y++) {
				if((sectorX + x >= 0 && sectorX + x < Generation.MAX_SECTOR) || (sectorY + y >= 0 && sectorY + y < Generation.MAX_SECTOR)) {
					gen.GenerateSector(sectorX + x, sectorY + y);
					gen.InstanciateSector(sectorX + x, sectorY + y);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		int x = (int) Player.transform.position.x;
		int z = (int) Player.transform.position.z;

		//TODO: Add the correct location of the sector based on getSector()
		if (!gen.IsGenerated(x, z + drawDistance)) { //if z + draw is not generated
			gen.GenerateSector(x, z + drawDistance);
			gen.InstanciateSector(x , z + drawDistance);
		}
		else if (!gen.IsGenerated(x, z - drawDistance)) { //if z - draw is not generated
			gen.GenerateSector(x, z - drawDistance);
			gen.InstanciateSector(x , z - drawDistance);
		}
		else if (!gen.IsGenerated(x + drawDistance, z)) { //if x + draw is not generated
			gen.GenerateSector(x + drawDistance, z);
			gen.InstanciateSector(x + drawDistance, z);
		}
		else if (!gen.IsGenerated(x - drawDistance, z)) { //if x - draw is not generated
			gen.GenerateSector(x - drawDistance, z);
			gen.InstanciateSector(x - drawDistance, z);
		}
	}

	
}
