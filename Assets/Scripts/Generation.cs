using System;
using UnityEngine;

public class Generation {
	public static System.Random rand = new System.Random();
	public Transform[] tiles;
	public int drawDistance;
	public int spawnX, spawnY;
	public int seed;			
	Sector[,] sectors;

	//-------CONSTANTS/-------
	public const int MAX_SECTOR = 128;
	public const int MAX_TRANSFORM = 16;
	public const int EMPTY = 0;
	public const int START = 1;
	public const int HALL = 2;

	// Use this for initialization
	public Generation(int drawDist, Transform[] b, int sectorX, int sectorY) {
		seed = rand.Next();

		//Copy Tile table
		tiles = new Transform[b.Length];
		for (int x = 0; x < b.Length; x++) {
			tiles[x] = b[x];
		}

		//Get spawn sector and location
		int spawnX = rand.Next(0, MAX_TRANSFORM - 1);
		int spawnY = rand.Next(0, MAX_TRANSFORM - 1);
		int spawnZ = rand.Next(0, MAX_TRANSFORM - 1);

		int rot = rand.Next(0, 3);

		//Initialize sectors
		sectors = new Sector[MAX_SECTOR, MAX_SECTOR];
		for (int x = 0; x < MAX_SECTOR; x++ ) {
			for (int y = 0; y < MAX_SECTOR; y++) {
				sectors[x, y] = new Sector(x, y);
			}
		}

		GenerateSector(sectorX, sectorY);
		//setSector(sectors[sectorX, sectorY], sectorX, sectorY, 1); //For testing spawn distance

		tiles[START].position = new Vector3(spawnX, spawnY, spawnZ);
		tiles[START].rotation = Quaternion.Euler(0, rot * 90, 0);
		sectors[sectorX, sectorY].SetMapTransform(spawnX, spawnY, spawnZ, START);

		InstanciateSector(sectorX, sectorY);
	}

	//TODO: Add item spawn generation, add player, enemy spawns, and saving the map. 
	//Generates the sectors based off of the generation requirements and instantiates them
	public void GenerateSector(int xSector, int ySector) {
		if(this.sectors[xSector, ySector].generated) {
			return;
		}
		//int t = rand.Next(0, tiles.Length-1); //chosen tile value
		sectors[xSector, ySector] = new Sector(xSector, ySector);
		setSector(sectors[xSector, ySector], xSector, ySector, EMPTY);

		//Add Hallway Crossroads
		int numVertex = rand.Next(6, 16);
		for(int v = 0; v < numVertex ; v++) {
			int x = rand.Next(0, MAX_TRANSFORM - 1);
			int y = rand.Next(0, MAX_TRANSFORM - 1);
			int z = rand.Next(0, MAX_TRANSFORM - 1);
			int rot = rand.Next(0, 3); //randomize rotation

			//tiles[HALL].position = new Vector3(16 * 5 * xSector + 5 * x, y, 16 * 5 * ySector + 5 * z);
			//tiles[HALL].rotation = Quaternion.Euler(0, rot * 90, 0);
			sectors[xSector, ySector].SetMapTransform(x, y, z, HALL);	
		}

		ConnectTiles(sectors[xSector, ySector]);
		AddRooms(sectors[xSector, ySector]);
		sectors[xSector, ySector].generated = true;
	}

	//Add special cases as the tiles are added
	//TODO: Add ladders and other rooms with connections
	public void ConnectTiles(Sector s) {
		
	}

	//TODO: Adds rooms connected to hallways
	public void AddRooms(Sector s) {

	}

	//Returns the Vec2 of location coordinates
	public Vector2 GetSector(float x, float y, float z) {
		int xSector = (int) Math.Floor(x / 80);
		int ySector = (int) Math.Floor(z / 80);
		return new Vector2(xSector, ySector);
	}
	
	//Returns whether the sector was generated
	public Boolean IsGenerated(int xSector, int ySector) {
		return sectors[xSector, ySector].generated;
	}

	public Boolean IsInstanciated(int xSector, int ySector) {
		return sectors[xSector, ySector].instanciated;
	}

	public void setSector(Sector s, int xSector, int ySector, int type) {
		for (int x = 0; x < MAX_TRANSFORM; x++) {
			for (int y = 0; y < MAX_TRANSFORM; y++) {
				for (int z = 0; z < MAX_TRANSFORM; z++) {
					s.SetMapTransform(x, y, z, type);
				}
			}
		}
	}
	//Instantiates the transform of the tile 
	//TODO: Have objects rotate to allign 
	//TODO: Avoid instancing empty transforms
	public void InstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					Vector3 v = new Vector3(80 * xSector + (5 * x), 5 * y, 80 * ySector + (5 * z));
					Transform t = tiles[sectors[xSector, ySector].GetMapTransform(x, y, z)];
					GameObject.Instantiate(t, v, Quaternion.identity);			
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
					GameObject.Destroy(tiles[sectors[xSector, ySector].GetMapTransform(x, y, z)]);
				}
			}
			sectors[xSector, ySector].instanciated = false;
		}
	}

	//Cleans up the memory of the sectors 
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
