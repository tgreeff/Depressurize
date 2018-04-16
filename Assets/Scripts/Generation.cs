using System;
using UnityEngine;

public class Generation {
	public static System.Random rand = new System.Random();
	public Transform[] tiles;
			
	Sector[,] sectors;
	public int spawnPositionX, spawnPositionY, spawnPositionZ;
	public int numTiles = 0;
	private int spawnSectorX, spawnSectorY;

	//-------CONSTANTS/-------
	public const int MAX_SECTOR = 128;
	public const int MAX_SECTOR_TRANSFORM = 16;
	public const float TILE_SIZE = 10.5f;
	public const int EMPTY = 0;
	public const int START = 1;
	public const int HALL = 2;
	public const int HALL_CORNER = 3;
	public const int HALL_TRI = 4;
	public const int HALL_QUAD = 5;
	public const int HALL_DEAD_END = 6;
	public const int HALL_DOOR_END = 7;
	public const int HALL_HATCH_DOWN = 8;
	public const int HALL_LADDER_UP = 9;

	// Use this for initialization
	public Generation(int drawDist, Transform[] b, int sectorX, int sectorY) {
		//Copy Tile table
		tiles = new Transform[b.Length];
		for (int x = 0; x < b.Length; x++) {
			tiles[x] = b[x];
		}

		spawnSectorX = sectorX;
		spawnSectorY = sectorY;

		//Initialize sectors
		sectors = new Sector[MAX_SECTOR, MAX_SECTOR];
		for (int x = 0; x < MAX_SECTOR; x++ ) {
			for (int y = 0; y < MAX_SECTOR; y++) {
				sectors[x, y] = new Sector(x, y);
			}
		}

		//Set spawn position
		spawnPositionX = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);
		spawnPositionY = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);
		spawnPositionZ = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);

		//Generate and Set initial position
		GenerateSector(sectorX, sectorY);
		sectors[sectorX, sectorY].SetMapTransform(spawnPositionX, spawnPositionY, spawnPositionZ, START);
		numTiles++;
	}

	//TODO: Make it so that some sectors don't generate
	//TODO: connect between sectors
	//TODO: Add item spawn generation, add player, enemy spawns, and saving the map. 

	//Generates the sectors based off of the generation requirements and instantiates them
	public void GenerateSector(int xSector, int ySector) {
		if(this.sectors[xSector, ySector].generated) {
			return;
		}

		sectors[xSector, ySector] = new Sector(xSector, ySector);
		SetSector(sectors[xSector, ySector], xSector, ySector, EMPTY);

		if(xSector == spawnSectorX && ySector == spawnSectorY) {
			AddHalls(sectors[xSector, ySector], spawnPositionX, spawnPositionY, spawnPositionZ);
		}
		else {
			int x = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);
			int y = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);
			int z = rand.Next(0, MAX_SECTOR_TRANSFORM - 1);

			AddHalls(sectors[xSector, ySector], x, y, z);
		}

		AddRooms(sectors[xSector, ySector]);
		sectors[xSector, ySector].generated = true;
	}

	//Add special cases as the tiles are added
	//TODO: Add ladders and other rooms with connections
	//TODO: Add vertical connections
	private void AddHalls(Sector s, int xPos, int yPos, int zPos) {
		Vector2[] last = new Vector2[4];

		Vector2 from = new Vector2(spawnPositionX, spawnPositionZ);
		int i = 0;
		for (int x = -1; x < 2; x += 2) { //start from spawn
			for (int z = -1; z < 2; z += 2) {
				from = new Vector2(spawnPositionX + x, spawnPositionZ + z);
				last[i] = GeneratePath(s, from, spawnPositionY);
				i++;
			}
		}
		for (int y = 0; y < 16; y++) { //Make paths on each layer
			if(y != spawnPositionY) {

			}
		}
		//check edge to connect other sectors
	}

	//TODO: Add Rotations, Make recursive function of this for continuing off of left open areas
	//Creates a hallway path between 2 points in a sector
	private Vector2 GeneratePath(Sector s, Vector2 from,  int y) {
		int catchLoop = 0;
		int numberHalls = rand.Next(20, 50);
		
		int x = (int) from.x;
		int z = (int) from.y;
		int lastX = (int)from.x;
		int lastZ = (int)from.y;
		bool movedX = false;
		bool movedZ = false;
		int prevTransform = 0;
		int direction = 0; // 0 = East, 1 = South, 2 = West, 3 = North

		for (int n  = 0; n < numberHalls && catchLoop < 50; n++) {
			int path = rand.Next(0, 99);  
			if (path < 25) {					//x++, east
				if (x >= MAX_SECTOR_TRANSFORM - 1) {
					x = MAX_SECTOR_TRANSFORM - 1;
				}
				else {
					x++;
					direction = 0;
				}
			}
			else if (path >= 25 && path < 50) { //x--, west
				if (x <= 0) {
					x = 0;
				}
				else {
					x--;
					direction = 2;
				}
			}
			else if (path >= 50 && path < 75) { //z++, north
				if (z >= MAX_SECTOR_TRANSFORM - 1) {
					z = MAX_SECTOR_TRANSFORM - 1;
				}
				else {
					z++;
					direction = 3;
				}			
			}
			else if(path >= 75 && path < 100) { //z--, south
				if (z <= 0) {
					z = 0;
				}
				else {
					z--;
					direction = 1;
				}			
			}
			int currentSpot = s.GetMapTransform(x, y, z);
			if (currentSpot == EMPTY || currentSpot == HALL_DEAD_END) {  //transform not taken up
				if(lastZ != z && movedX || lastX != x && movedZ) { //last was in other dir
					s.SetMapTransform(x, y, z, HALL_CORNER);
				}
				else if((lastZ != z && movedZ) || (lastX != x && movedX)) { //last was in same dir
					int type = rand.Next(0, 99);
					if (type < 50) {
						s.SetMapTransform(x, y, z, HALL);
						prevTransform = HALL;
					}
					else if (type >= 50 && type < 80) {
						s.SetMapTransform(x, y, z, HALL_CORNER);
						prevTransform = HALL_CORNER;
					}
					else if(type >= 80 && type < 85) {
						s.SetMapTransform(x, y, z, HALL_TRI);
						prevTransform = HALL_TRI;
					}
					else if(type >= 85 && type < 90) {
						s.SetMapTransform(x, y, z, HALL_QUAD);
						prevTransform = HALL_QUAD;
					}
					else if (type >= 90 && type < 95) {
						s.SetMapTransform(x, y, z, HALL_HATCH_DOWN);
						prevTransform = HALL_HATCH_DOWN;
					}
					else if (type >= 95 && type < 100 && prevTransform != HALL_LADDER_UP) {
						s.SetMapTransform(x, y, z, HALL_LADDER_UP);
						prevTransform = HALL_LADDER_UP;
					}
					s.SetMapRotation(x, y, z, direction);
				}
				else if (lastZ != z) {
					s.SetMapTransform(x, y, z, HALL);
					s.SetMapRotation(x, y, z, direction);
					prevTransform = HALL;
				}
				else if (lastX != x) {
					s.SetMapTransform(x, y, z, HALL);
					s.SetMapRotation(x, y, z, direction);
					prevTransform = HALL;
				}
		
				catchLoop = 0;
				if (lastZ != z) { //update after to retain last
					movedZ = true;
					lastZ = z;
				}
				else if (lastX != x) {
					movedX = true;
					lastX = x;
				}
			}
			else { //When spot is already taken up control when back tracking
				//Cap open ends
				if (lastZ > z ) { //z decrease in value
					int tempZ = ControlCoordinate(++lastZ);
					if (s.GetMapTransform(x, y, tempZ) == EMPTY) { //keep from placing everywhere
						s.SetMapTransform(x, y, tempZ, HALL_DEAD_END);
						s.SetMapRotation(x, y, tempZ, direction);
					}
				}
				else if (lastX > x) { //x decrease in value
					int tempX = ControlCoordinate(++lastX);
					if (s.GetMapTransform(tempX, y, z) == EMPTY) {
						s.SetMapTransform(tempX, y, z, HALL_DEAD_END);
						s.SetMapRotation(tempX, y, z, direction);
					}
				}
				else if (lastZ < z) { //z increase in value					
					int tempZ = ControlCoordinate(--lastZ);
					if (s.GetMapTransform(x, y, tempZ) == EMPTY) {
						s.SetMapTransform(x, y, tempZ, HALL_DEAD_END);
						s.SetMapRotation(x, y, tempZ, direction);
					}					
				}
				else if (lastX < x) { //x increase in value
					int tempX = ControlCoordinate(--lastX);
					if(s.GetMapTransform(tempX, y, z) == EMPTY) {
						s.SetMapTransform(tempX, y, z, HALL_DEAD_END);
						s.SetMapRotation(tempX, y, z, direction);
					}
					
				}
				numberHalls++;
				movedX = false;
				movedZ = false;
				lastX = x;
				lastZ = z;
				catchLoop++;
				prevTransform = 0;
			}

		}	
		return new Vector2(x, z);
	}
	
	private int ControlCoordinate(int value) {
		if(value > MAX_SECTOR_TRANSFORM - 1) {
			return MAX_SECTOR_TRANSFORM - 1;
		}
		else if(value < 0){
			return 0;
		}
		return value;
	}

	//TODO: Adds rooms connected to hallway
	private void AddRooms(Sector s) {

	}

	//TODO: Adds minable asteroids to the sector
	private void AddAsteroids(Sector s) {

	}

	private void AddItems(Sector s) {
		
	}

	//Gets sector based off of index
	public Sector GetSector(int x, int y) {
		return sectors[x, y];
	}

	//Returns the Vec2 of location coordinates
	public Vector2 GetSectorFromPosition(float x, float y, float z) {
		int sectorSize = (int)Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM;
		x = x - (x % sectorSize);
		y = z - (z % sectorSize);
		return new Vector2(x, y);
	}
	
	//Returns whether the sector was generated
	public Boolean IsGenerated(int xSector, int ySector) {
		return sectors[xSector, ySector].generated;
	}

	public Boolean IsInstanciated(int xSector, int ySector) {
		return sectors[xSector, ySector].instanciated;
	}

	public void SetSector(Sector s, int xSector, int ySector, int type) {
		for (int x = 0; x < MAX_SECTOR_TRANSFORM; x++) {
			for (int y = 0; y < MAX_SECTOR_TRANSFORM; y++) {
				for (int z = 0; z < MAX_SECTOR_TRANSFORM; z++) {
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
					int transInt = sectors[xSector, ySector].GetMapTransform(x, y, z);
					if(transInt != EMPTY) {
						Transform transform = tiles[transInt];
						Vector3 position = new Vector3(
							(TILE_SIZE * MAX_SECTOR_TRANSFORM) * xSector + (TILE_SIZE * x) + transform.position.x, 
							TILE_SIZE * y + transform.position.y, 
							(TILE_SIZE * MAX_SECTOR_TRANSFORM) * ySector + (TILE_SIZE * z) + transform.position.z);
						int transRot = sectors[xSector, ySector].GetMapRotation(x, y, z);
						
						Quaternion rotation = Quaternion.identity;
						rotation.eulerAngles = new Vector3(0, 90 * transRot, 0);
						GameObject.Instantiate(transform, position, rotation);

						
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
