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

	}

	//TODO: Change hall vertexes with corners or other connectors
	//TODO: Instantiate every time a tile gets picked.
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
		int numVertex = rand.Next(56, 320); //Should be at least 4 on 14 layers.
		for(int v = 0; v < numVertex ; v++) {
			int x = rand.Next(0, MAX_TRANSFORM - 1);
			int y = rand.Next(0, MAX_TRANSFORM - 1);
			int z = rand.Next(0, MAX_TRANSFORM - 1);
			int rot = rand.Next(0, 3); //randomize rotation

			//tiles[HALL].position = new Vector3(16 * 5 * xSector + 5 * x, y, 16 * 5 * ySector + 5 * z);
			//tiles[HALL].rotation = Quaternion.Euler(0, rot * 90, 0);
			sectors[xSector, ySector].SetMapTransform(x, y, z, HALL);
			sectors[xSector, ySector].SetNumHallVertex(sectors[xSector, ySector].GetNumHallVertex()+1);
		}

		ConnectTiles(sectors[xSector, ySector]);
		AddRooms(sectors[xSector, ySector]);
		sectors[xSector, ySector].generated = true;
	}

	//Add special cases as the tiles are added
	//TODO: Add ladders and other rooms with connections
	//TODO: Add vertical connections
	public void ConnectTiles(Sector s) {
		int i = 0;
		Vector3[] halls = new Vector3[s.GetNumHallVertex()];
		int[] numHalls = new int[MAX_TRANSFORM];
		for (int y = 0; y < MAX_TRANSFORM; y++) {
			numHalls[y] = 0;
		}

		//Get the number of halls in Sector
		int xSector =(int) s.coordinates.x;
		int ySector = (int)s.coordinates.y;
		for (int y = 0; y < MAX_TRANSFORM; y++) {
			for (int x = 0; x < MAX_TRANSFORM; x++) {
				for (int z = 0; z < MAX_TRANSFORM; z++) {
					int transform = s.GetMapTransform(x, y, z);
					if(transform == HALL) {
						numHalls[y]++; //add number of halls as you progress
						halls[i] = new Vector3(x, y, z);
					}
				}
			}
		}

		//TODO: Figure out why it will not connect.
		//Connect halls on the same level
		i = 0; //index for current hall	
		for (int y = 0; y < MAX_TRANSFORM; y++) {

			//While y level has more than 1 hall and current hall is still on same level
			while ((i < numHalls.Length) && (numHalls[y] > 1) && (y == (int) halls[i].y)) {
				int placeTile = 0;
				int lastX = (int) halls[i].x; //Last x place hall was placed
				int lastZ = (int) halls[i].z; //Last y place hall was placed
				bool passed = false;
				int h = 0;  //number of halls placed
				int maxHalls = 0; //TODO: max number of halls to place before forcing

				int pathX = rand.Next(0, 1); //Multiple traversals to get multiple paths
				int pathZ = rand.Next(0, 1); //TODO: add for multiple pathing
				int numPaths = rand.Next(1, 4);				

				for (int x = (int) halls[i].x; x < MAX_TRANSFORM; x++) { //Start from current halls
					for (int z = (int)halls[i].z; z < MAX_TRANSFORM; z++) {

						if ((x == lastX + 1 ^ z == lastZ + 1) && (y == (int)halls[i++].y)) { //x or z are in pos for placement
							int distToNextX = (int)halls[i++].x - x;
							int distToNextZ = (int)halls[i++].z - z;
							//maxHalls = rand.Next((distToNextX + distToNextZ)/2 - 1, 
								//distToNextX + distToNextZ);

							if (!passed && distToNextX != 0 && distToNextZ != 0) {
								placeTile = rand.Next(0, 1); //false or true
								passed = true;
							}
							else if((distToNextX == 0 ^ distToNextZ == 0) || (passed && placeTile == 0)) {
								placeTile = 1;
								passed = false;
							}

							if ((placeTile == 1) && s.GetMapTransform(x, y, z) == EMPTY) { //TODO: add checking for maxHalls: || maxHalls == h
								s.SetMapTransform(x, y, z, HALL);
								h++;
								lastX = x;
								lastZ = z;
								if(!passed) {
									placeTile = 0;
								}
							}
						}


					}
				}	
				i++; //Done. Move to next hall
			}
		}
	}

	//TODO: Adds rooms connected to hallway
	public void AddRooms(Sector s) {

	}

	//TODO: Adds minable asteroids to the sector
	public void AddAsteroids(Sector s) {

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
	public void InstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					int transform = sectors[xSector, ySector].GetMapTransform(x, y, z);
					if(transform != EMPTY) {
						Vector3 v = new Vector3(80 * xSector + (5 * x), 5 * y, 80 * ySector + (5 * z));
						Transform t = tiles[transform];
						GameObject.Instantiate(t, v, Quaternion.identity);
					}						
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
