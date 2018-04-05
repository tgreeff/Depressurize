using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject Player;
	public Transform[] blocks;
	public int currentSectorX;
	public int currentSectorY;
	
	public int drawDistance; //TODO: could posibly be changed to next closest sector, instead of draw distance
	public float timeLimit;
	private Generation gen;
	private float timer;

	//---World Formation Variables---
	public int mapSize = Generation.MAX_SECTOR;
	public int seed;

	public int LimitNumberEnemyFactions, LimitNumberEncounters;
	private int numEnemyFactions, numEncounters;    //Numbers - this is the number generated for number of landmarks  																								 
	private Transform[] enemyFactions, encounters;  //Locations - these contain locations for the varying landmarks. Instantciated with the numbers above	
													
	//TODO: update player position and sector
	void Start () {
		timer = 0;
		timeLimit = 30;

		int sectorX = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);
		int sectorY = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);

		currentSectorX = sectorX;
		currentSectorY = sectorY;

		gen = new Generation(drawDistance, blocks, sectorX, sectorY);
		while(gen.numTiles < 10) {
			sectorX = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);
			sectorY = Generation.rand.Next(0, Generation.MAX_SECTOR - 1);
			gen = new Generation(drawDistance, blocks, sectorX, sectorY);
		}

		//TODO
		Vector3 playerPos = gen.GetSector(sectorX, sectorY).FindFirstInstance(Generation.START);
		if(playerPos != new Vector3(0, 0, 0)) {
			Player.transform.position = playerPos;
		}
		
		
		for (int x = -drawDistance; x < drawDistance; x++) {
			for (int y = -drawDistance; y < drawDistance; y++) {
				if((sectorX + x >= 0 && sectorX + x < Generation.MAX_SECTOR) && (sectorY + y >= 0 && sectorY + y < Generation.MAX_SECTOR)) {
					gen.GenerateSector(sectorX + x, sectorY + y);
					gen.InstanciateSector(sectorX + x, sectorY + y);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: update the players current sector.
		//TODO: Add the correct location of the current sector based on getSector()
		if(timer >= timeLimit) {
			CheckDrawDistance();
			UpdateCurrentSector();
		}
		
	}

	private void CheckDrawDistance() {
		//if z + draw is not generated
		if (!gen.IsGenerated(currentSectorX, currentSectorY + drawDistance)) { 
			gen.GenerateSector(currentSectorX, currentSectorY + drawDistance);
			gen.InstanciateSector(currentSectorX, currentSectorY + drawDistance);
		}
		//if z - draw is not generated
		else if (!gen.IsGenerated(currentSectorX, currentSectorY - drawDistance)) {
			gen.GenerateSector(currentSectorX, currentSectorY - drawDistance);
			gen.InstanciateSector(currentSectorX, currentSectorY - drawDistance);
		}
		//if x + draw is not generated
		else if (!gen.IsGenerated(currentSectorX + drawDistance, currentSectorY)) { 
			gen.GenerateSector(currentSectorX + drawDistance, currentSectorY);
			gen.InstanciateSector(currentSectorX + drawDistance, currentSectorY);
		}
		//if x - draw is not generated
		else if (!gen.IsGenerated(currentSectorX - drawDistance, currentSectorY)) { 
			gen.GenerateSector(currentSectorX - drawDistance, currentSectorY);
			gen.InstanciateSector(currentSectorX - drawDistance, currentSectorY);
		}
	}

	//TODO
	private void UpdateCurrentSector() {
		currentSectorX = (int) Player.transform.position.x;
		currentSectorY = (int) Player.transform.position.z;
	}
}
