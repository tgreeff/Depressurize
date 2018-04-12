using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject Player;
	public Transform[] blocks;
	public int currentSectorX;
	public int currentSectorY;

	private Generation worldGeneration;
	public int drawDistance; //TODO: could posibly be changed to next closest sector, instead of draw distance
	private float timer = 0;
	private float timeLimit = 30;
	private int spawnSectorX, spawnSectorY;

	void Start () {
		spawnSectorX = currentSectorX = Generation.rand.Next(drawDistance, Generation.MAX_SECTOR - (drawDistance + 1));
		spawnSectorY = currentSectorY = Generation.rand.Next(drawDistance, Generation.MAX_SECTOR - (drawDistance + 1));
		worldGeneration = new Generation(drawDistance, blocks, currentSectorX, currentSectorY);

		float tSize = Generation.TILE_SIZE;
		int max = Generation.MAX_SECTOR_TRANSFORM;
		float xPos = (tSize * max) * spawnSectorX + (tSize * worldGeneration.spawnPositionX);
		float yPos = (tSize * worldGeneration.spawnPositionY);
		float zPos = (tSize * max) * spawnSectorY + (tSize * worldGeneration.spawnPositionZ);
		Player.transform.position = new Vector3( xPos, yPos, zPos);
	
		if(drawDistance == 0) {
			drawDistance = 1;
		}
		for (int x = -drawDistance; x <= drawDistance; x++) {
			for (int y = -drawDistance; y <= drawDistance; y++) {
				worldGeneration.GenerateSector(currentSectorX + x, currentSectorY + y);
				worldGeneration.InstanciateSector(currentSectorX + x, currentSectorY + y);
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
			timer = 0;
		}
		else {
			timer += Time.deltaTime;
		}
	}

	private void CheckDrawDistance() {
		//if z + draw is not generated //TODO - make values 
		int distYPlus = currentSectorY + drawDistance;
		int distYMinus = currentSectorY - drawDistance;
		int distXPlus = currentSectorX + drawDistance;
		int distXMinus = currentSectorX - drawDistance;

		//if z + draw is not generated
		if (!worldGeneration.IsGenerated(currentSectorX, distYPlus)) { 
			worldGeneration.GenerateSector(currentSectorX, distYPlus);
			worldGeneration.InstanciateSector(currentSectorX, distYPlus);
		}
		//if z - draw is not generated
		else if (!worldGeneration.IsGenerated(currentSectorX, distYMinus)) {
			worldGeneration.GenerateSector(currentSectorX, distYMinus);
			worldGeneration.InstanciateSector(currentSectorX, distYMinus);
		}
		//if x + draw is not generated
		else if (!worldGeneration.IsGenerated(distXPlus, currentSectorY)) { 
			worldGeneration.GenerateSector(distXPlus, currentSectorY);
			worldGeneration.InstanciateSector(distXPlus, currentSectorY);
		}
		//if x - draw is not generated
		else if (!worldGeneration.IsGenerated(distXMinus, currentSectorY)) { 
			worldGeneration.GenerateSector(distXMinus, currentSectorY);
			worldGeneration.InstanciateSector(distXMinus, currentSectorY);
		}
	}

	//TODO
	private void UpdateCurrentSector() {
		int x = (int) Player.transform.position.x;
		int y = (int) Player.transform.position.z;
		int sectorSize = (int) Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM;

		currentSectorX = x - (x % sectorSize);
		currentSectorY = y - (y % sectorSize);
	}
}
