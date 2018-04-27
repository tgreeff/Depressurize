using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public GameObject Player;
	public Transform[] blocks;
	public Transform[] enemies;
	public int currentSectorX;
	public int currentSectorY;
	public int currentBlockX;
	public int currentBlockY;

	private Generation worldGeneration;
	public int drawDistance; //TODO: could posibly be changed to next closest sector, instead of draw distance
	private float timer = 0;
	private float timeLimit = 30;
	public int spawnSectorX, spawnSectorY;

	void Start () {
		timer = 30;

		spawnSectorX = Generation.rand.Next(drawDistance, Generation.MAX_SECTOR - (drawDistance + 1));
		spawnSectorY = Generation.rand.Next(drawDistance, Generation.MAX_SECTOR - (drawDistance + 1));
		currentSectorX = spawnSectorX;
		currentSectorY = spawnSectorY;
		worldGeneration = new Generation(drawDistance, blocks, enemies, currentSectorX, currentSectorY);

		float tSize = Generation.TILE_SIZE;
		int max = Generation.MAX_SECTOR_TRANSFORM;
		float xPos = (tSize * max) * spawnSectorX + (tSize * worldGeneration.spawnPositionX);
		float yPos = (Generation.TILE_HEIGHT * worldGeneration.spawnPositionY);
		float zPos = (tSize * max) * spawnSectorY + (tSize * worldGeneration.spawnPositionZ);
		Player.transform.position = new Vector3( xPos, yPos+1, zPos);
	
		if(drawDistance == 0) {
			drawDistance = 1;
		}

		worldGeneration.GenerateSector(currentSectorX, currentSectorY);
		worldGeneration.InstanciateSector(currentSectorX, currentSectorY);

		/*
		for (int x = -drawDistance; x <= drawDistance; x++) {
			for (int y = -drawDistance; y <= drawDistance; y++) {
				worldGeneration.GenerateSector(currentSectorX + x, currentSectorY + y);
				worldGeneration.InstanciateSector(currentSectorX + x, currentSectorY + y);
			}
		}
		*/
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
		else if(timer >= 5 && timer != timeLimit) {
			worldGeneration.SpawnEnemies(currentSectorX, currentSectorY);
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

		//TODO check values
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

	private void UpdateCurrentSector() {
		float x = Player.transform.position.x;
		float y = Player.transform.position.z;
		float sectorSize = Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM;

		currentSectorX = (int) Mathf.Floor(x / sectorSize);
		currentSectorY = (int) Mathf.Floor(y / sectorSize);
	}
}
