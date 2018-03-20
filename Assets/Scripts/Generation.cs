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
	public const int EMPTY = 0;
	public const int START = 1;
	public const int HALL = 2;

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
		this.GenerateSector(sectorX, sectorY);
		this.InstanciateSector(sectorX, sectorY);
		
	}

	//TODO - Add item spawn generation, add player, enemy spawns, and saving the map. 
	//Generates the sectors based off of the generation requirements
	public void GenerateSector(int xSector, int ySector) {
		if(this.sectors[xSector, ySector].generated) {
			return;
		}

		int t = rand.Next(0, tiles.Length-1); //chosen tile value

		sectors[xSector, ySector] = new Sector();
		for (int x = 0; x >= 16; x++) { //TODO flip to 16 -> 0
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					Transform prevX = null;
					Transform prevY = null;
					Transform prevZ = null;
					if (x != 0) {
						 prevX = sectors[xSector, ySector].GetMapTransform(x - 1, y, z);
					}
					if(y != 0) {
						prevY = sectors[xSector, ySector].GetMapTransform(x, y - 1, z);
					}
					if(z != 0) {
						prevZ = sectors[xSector, ySector].GetMapTransform(x, y, z - 1);
					}

					CheckTiles(prevX, prevY, prevZ);
					

					if(sectors[xSector, ySector].GetMapTransform(x, y , z) != null) {
						sectors[xSector, ySector].SetMapTransform(x, y, z, tiles[t]);
					}
				}
			}
		}

		this.sectors[xSector, ySector].generated = true;
	}

	//Add special cases as the tiles are added
	public void CheckTiles(Transform x, Tranform y, Transform z, int t) {
		Boolean xMatch = x == tiles[t];
		Boolean yMatch = y == tiles[t];
		Boolean zMatch = z == tiles[t];

		if (xMatch) {
			//TODO change t based on value of tile
		}
		if (yMatch) {

		}
		if (zMatch) {

		}

		switch (t) {
			case 0:

				break;

			case 1:

				break;

			case 2:

				break;

			default:

				break;
		}
	}

	//Returns the sector based on coordinates
	public Sector GetSector(float x, float y, float z) {
		return sectors[(int) x/5, (int) z/5];
	}
	
	//Returns whether the sector was generated
	public Boolean IsGenerated(int xSector, int ySector) {
		return sectors[xSector, ySector].generated;
	}

	public Boolean IsInstanciated(int xSector, int ySector) {
		return sectors[xSector, ySector].instanciated;
	}

	//Instantiates the transform of the tile
	public void InstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					GameObject.Instantiate(this.sectors[xSector, ySector].GetMapTransform(x, y, z));
				}
			}
		}
		sectors[xSector, ySector].instanciated = true;
	}

	//Instantiates the transform of the tile
	public void DeinstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					GameObject.Destroy(this.sectors[xSector, ySector].GetMapTransform(x, y, z));
				}
			}

			sectors[xSector, ySector].instanciated = false;
		}
	}

	public void DestroySectors() {
		for (int xS = 0; xS < MAX_SECTOR; xS++) {
			for (int yS = 0; yS < MAX_SECTOR; yS++) {
				sectors[xS, yS].SetMap(null);
				sectors[xS, yS] = null;
			}
		}
		sectors = null;
	}
}
