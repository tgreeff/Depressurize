using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generation {
	static System.Random rand = new System.Random();
	private Transform[] tiles;
	public int drawDistance;
	public int spawnX, spawnY;
	public int seed;			
	Sector[,] sectors;

	//-------CONSTANTS/-------
	public const int MAX_SECTOR = 128;


	// Use this for initialization
	public Generation(int drawDist, Transform[] b) {
		seed = rand.Next();

		//Get spawn sector and location
		spawnX = rand.Next(0, 15);
		spawnY = rand.Next(0, 15);
		int sectorX = rand.Next(0, MAX_SECTOR-1);
		int sectorY = rand.Next(0, MAX_SECTOR-1);

		this.tiles = new Transform[b.Length];
		for (int x = 0; x < b.Length; x++) {
			this.tiles[x] = b[x];
		}	

		sectors = new Sector[MAX_SECTOR, MAX_SECTOR];

		for (int x = 0; x < MAX_SECTOR; x++ ) {
			for (int y = 0; y < MAX_SECTOR; y++) {
				sectors[x, y] = new Sector();
			}
		}
		this.generateSector(sectorX, sectorY);
		
	}

	//TODO - Add item spawn generation, add player, enemy spawns, and saving the map. 
	//Generates the sectors based off of the generation requirements
	public void generateSector(int xSector, int ySector) {
		if(this.sectors[xSector, ySector].generated) {
			return;
		}
		else {
			this.sectors[xSector, ySector].generated = true;
		}

		int t = rand.Next(0, tiles.Length-1); //chosen tile value

		sectors[xSector, ySector] = new Sector();
		for (int x = 0; x < 16; x++) { //TODO flip to 16 -> 0
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					Transform prevX = sectors[xSector, ySector].getMapTransform(-x, y, z);
					Transform prevY = sectors[xSector, ySector].getMapTransform(x, -y, z);
					Transform prevZ = sectors[xSector, ySector].getMapTransform(x, y, -z);

					if(prevX == tiles[t]) {
						//TODO change t based on value of tile
					}
					if(prevY == tiles[t]) {

					}

					sectors[xSector, ySector].setMapTransform(x, y, z, tiles[t]);

				}
			}
		}
	}

	//Returns the sector based on coordinates
	public Sector getSector(float x, float y, float z) {
		return sectors[(int) x/5, (int) z/5];
	}
	
	//Returns whether the sector was generated
	public Boolean isGenerated(int xSector, int ySector) {
		return sectors[xSector, ySector].generated;
	}

	//Instantiates the transform of the tile
	public void instanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					GameObject.Instantiate(this.sectors[xSector, ySector].getMapTransform(x, y, z));
				}
			}
		}
	}

	//Instantiates the transform of the tile
	public void deinstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					GameObject.Destroy(this.sectors[xSector, ySector].getMapTransform(x, y, z));
				}
			}
		}
	}

	public void destroySectors() {
		for (int xS = 0; xS < MAX_SECTOR; xS++) {
			for (int yS = 0; yS < MAX_SECTOR; yS++) {
				sectors[xS, yS].setMap(null);
				sectors[xS, yS] = null;
			}
		}
	}
}
