using System;
using UnityEngine;

public class Generation {
	public static System.Random rand = new System.Random();
	public Transform[] tiles;
	public int drawDistance;
	public int spawnX, spawnY;
	public int seed;			
	Sector[,] sectors;
	public  int numTiles = 0;
	
	//-------CONSTANTS/-------
	public const int MAX_SECTOR = 128;
	public const int MAX_TRANSFORM = 16;
	public const int EMPTY = 0;
	public const int START = 1;
	public const int HALL = 2;
	public const int HALL_CORNER = 3;

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

		//Add spawn
		sectors[sectorX, sectorY].SetMapTransform(spawnX, spawnY, spawnZ, START);
		
		numTiles++;
	}

	//TODO: Make it so that some sectors don't generate
	//TODO: connect between sectors
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
		SetSector(sectors[xSector, ySector], xSector, ySector, EMPTY);

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
			numTiles++;
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

		//Connect halls on the same level
		int lastX = (int)halls[0].x; //Last x place hall was placed
		int lastZ = (int)halls[0].z; //Last y place hall was placed
		int hallsPlaced = 0;
		int pathX; //Multiple traversals to get multiple paths
		int pathZ; //TODO: add for multiple pathing vertical and horizontal
		int numPaths = rand.Next(1, 4);

		for (int h = 1; h < halls.Length; h++) {
			while(halls[h].y != halls[h - 1].y && h < halls.Length) {
				h++;	
			}
			if (h >= halls.Length) {
				break;
			}

			pathX = rand.Next(0, 1); //decide if placing path for x
			pathZ = rand.Next(0, 1); //decide if placing path for z
			int x = (int)halls[h - 1].x;
			int z = (int)halls[h - 1].z;
			bool lastDirectionX = false;
			bool lastDirectionZ = false;
			while(x != (int) halls[h].x && z != (int) halls[h].z) {
				if(x == (int)halls[h].x) { //on the edge
					//TODO: corners
					s.SetMapTransform(x, (int)halls[h].y, z, HALL); //set hall
					z++;
				}
				else if(z == (int)halls[h].z) {
					s.SetMapTransform(x, (int)halls[h].y, z, HALL); //set hall
					x++;
				}
				else if(pathX == 1 && pathZ == 0) { //Add path to x
					if (lastDirectionX) {
						s.SetMapTransform(x, (int)halls[h].y, z, HALL); //set hall
					}
					else {
						s.SetMapTransform(x, (int)halls[h].y, z, HALL_CORNER); //set hall corner
					}
					x++;
					lastDirectionX = true;
					lastDirectionZ = false;
					hallsPlaced++;
				}
				else if(pathX == 0 && pathZ == 1) { //Add path to z
					if (lastDirectionZ) {
						s.SetMapTransform(x, (int)halls[h].y, z, HALL); //set hall
					}
					else {
						s.SetMapTransform(x, (int)halls[h].y, z, HALL_CORNER); //set hall corner
					}
					z++;
					lastDirectionX = false;
					lastDirectionZ = true;
					hallsPlaced++;
				}
				else { //if pathx == pathz
					if(pathX == 1) {
						//TODO
						x++;
						z++;
					}
					else {
						//TODO
						x++;
						z++;
					}
				}
			}
		}
	}

	//TODO: Adds rooms connected to hallway
	public void AddRooms(Sector s) {

	}

	//TODO: Adds minable asteroids to the sector
	public void AddAsteroids(Sector s) {

	}

	//Gets sector based off of index
	public Sector GetSector(int x, int y) {
		return sectors[x, y];
	}

	//Returns the Vec2 of location coordinates
	public Vector2 GetSectorFromPosition(float x, float y, float z) {
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

	public void SetSector(Sector s, int xSector, int ySector, int type) {
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
						Vector3 v = new Vector3(80 * xSector + (4.5f * x), 5 * y, 80 * ySector + (4.5f * z));
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
