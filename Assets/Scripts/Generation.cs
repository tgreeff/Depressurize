using System;
using System.Collections;
using UnityEngine;

public class Generation {
	public static System.Random rand = new System.Random();
	public Transform[] tiles;

	public EnemySpawning enemySpawner;

	Sector[,] sectors;
	public int spawnPositionX, spawnPositionY, spawnPositionZ;
	public int numTiles = 0;
	private int spawnSectorX, spawnSectorY;

	//-------CONSTANTS/-------
	public const int MAX_SECTOR = 128;
	public const int MAX_SECTOR_TRANSFORM = 16;
	public const float TILE_HEIGHT = 4.21f;
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
	public Generation(int drawDist, Transform[] b, Transform[] e, int sectorX, int sectorY) {
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
		spawnPositionX = rand.Next(2, MAX_SECTOR_TRANSFORM - 3);
		spawnPositionY = rand.Next(2, MAX_SECTOR_TRANSFORM - 3);
		spawnPositionZ = rand.Next(2, MAX_SECTOR_TRANSFORM - 3);

		//Generate and Set initial position
		GenerateSector(sectorX, sectorY);
		sectors[sectorX, sectorY].SetMapTransform(spawnPositionX, spawnPositionY, spawnPositionZ, START);
		numTiles++;
		enemySpawner = new EnemySpawning(e);
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
	//TODO: Add coroutines
	private void AddHalls(Sector s, int xPos, int yPos, int zPos) {
		Vector4[] last = new Vector4[120];
		int i = 0;
		int rotation = 0;

		for (int x = -1; x < 2; x++) { //start from spawn
			for (int z = -1; z < 2; z++) {
				if (x == 0 ^ z == 0 ) {
					Vector2 from = new Vector2(xPos + x, zPos + z);
					s.SetMapTransform((int) from.x, yPos, (int) from.y, HALL);
					Vector4[] continueLast = GeneratePath(s, from, yPos, rotation);
					rotation -= 2;
					if(rotation < 0) {
						rotation = 4 - rotation;
					}

					for(int t = 0; t < continueLast.Length; t++ ) {
						if(continueLast[t] != null) {
							last[i] = continueLast[t];
							i++;
						}
					}
				}
			}
		}	
		s.SetMapRotation(xPos+1, yPos, zPos, 0);
		s.SetMapRotation(xPos-1, yPos, zPos, 2);
		s.SetMapRotation(xPos, yPos, zPos+1, 3);
		s.SetMapRotation(xPos, yPos, zPos-1, 1);

		//Continue with last parts
		for (int x = 0; x < last.Length; x++) { 
			if (last[x] != null) {
				Vector2 from = new Vector2(last[x].x, last[x].z);
				GeneratePath(s, from, (int) last[x].y, (int) last[x].w);
			}
		}
		//check edge to connect other sectors
	}

	//Creates a hallway path between 2 points in a sector
	//TODO: Add Rotations, Make recursive function of this for continuing off of left open areas
	private Vector4[] GeneratePath(Sector s, Vector2 from,  int y, int dir) {
		int catchLoop = 0;
		int numberHalls = rand.Next(20, 50);

		Vector4[] continueLater = new Vector4[30];
		int w = 0;
		
		int x = (int) from.x;
		int z = (int) from.y;
		int lastX = (int)from.x;
		int lastZ = (int)from.y;
		int prevTransform = s.GetMapTransform((int) from.x, y, (int) from.y);
		int direction = dir; // 0 = East, 1 = South, 2 = West, 3 = North

		for (int n  = 0; n < numberHalls && catchLoop < 1000; n++) {
			int path = rand.Next(0, 99); //Calculate direction
			bool choosePath = true;
			while(choosePath) {
				if (prevTransform == HALL) {
					if (direction == 0) {
						x++;
					}
					else if (direction == 1) {
						z--;
					}
					else if (direction == 2) {
						x--;
					}
					else {
						z++;
					}
					choosePath = false;
				}
				else if (prevTransform == HALL_CORNER) {
					direction = (direction + 1) % 4;
					if(direction == 0) {
						x++;
					}
					else if (direction == 1) {
						z--;
					}
					else if(direction == 2) {
						x--;
					}
					else {
						z++;
					}
					choosePath = false;
				}
				else if(prevTransform == HALL_TRI) {
					int r = rand.Next(0, 1);
					if(r == 0) { // Right
						direction = (direction + 1) % 4;
					}
					else { //Left
						direction--;
						if(direction < 0) {
							direction = 3;
						}
					}
					if (direction == 0) {
						x++;
						continueLater[w] = new Vector4(x-2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 1) {
						z--;
						continueLater[w] = new Vector4(x, y, z+2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 2) {
						x--;
						continueLater[w] = new Vector4(x+2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else {
						z++;
						continueLater[w] = new Vector4(x, y, z-2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					choosePath = false;
				}
				else if(prevTransform == HALL_QUAD) {
					int r = rand.Next(0, 2);
					if (r == 0) { //Right
						direction = (direction + 1) % 4;
					}
					else if (r == 1) { //Straight
						
					}
					else { //Left
						direction--;
						if (direction < 0) {
							direction = 3;
						}
					}
					if (direction == 0) {
						x++;
						continueLater[w] = new Vector4(x-2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 1) {
						z--;
						continueLater[w] = new Vector4(x, y, z+2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else if (direction == 2) {
						x--;
						continueLater[w] = new Vector4(x+2, y, z, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					else {
						z++;
						continueLater[w] = new Vector4(x, y, z-2, direction);
						w++;
						if (w >= continueLater.Length) w = continueLater.Length - 1;
					}
					choosePath = false;
				}
				else if(prevTransform == HALL_HATCH_DOWN) {
					if(y > 0) {
						for (int i = 0; i < 2; i++) {
							direction = (direction + 1) % 4;
						}
						//y--;
						if (direction == 0) {
							//x++;
							continueLater[w] = new Vector4(x-1, y-1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 1) {
							//z--;
							continueLater[w] = new Vector4(x, y-1, z+1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 2) {
							//x--;
							continueLater[w] = new Vector4(x+1, y-1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else {
							//z++;
							continueLater[w] = new Vector4(x, y-1, z-1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
					}
					else {
						prevTransform = HALL;
					}
					choosePath = false;
				}
				else if (prevTransform == HALL_LADDER_UP) {
					if (y < MAX_SECTOR_TRANSFORM - 1) {
						for (int i = 0; i < 2; i++) {
							direction = (direction + 1) % 4;
						}
						//y++;
						if (direction == 0) {
							x++;
							continueLater[w] = new Vector4(x-1, y+1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 1) {
							z--;
							continueLater[w] = new Vector4(x, y+1, z+1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else if (direction == 2) {
							x--;
							continueLater[w] = new Vector4(x+1, y+1, z, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
						else {
							z++;
							continueLater[w] = new Vector4(x, y+1, z-1, direction);
							w++;
							if (w >= continueLater.Length) w = continueLater.Length - 1;
						}
					}
					else {
						prevTransform = HALL;
					}
					choosePath = false;
				}
				else if (path < 25) {                   //x++, east
					if (lastX < x) { //prevent backtracking
						continue;
					}
					else {
						if (x >= MAX_SECTOR_TRANSFORM - 1) {
							x = MAX_SECTOR_TRANSFORM - 1;
						}
						else {
							x++;
							direction = 0;
						}
						choosePath = false;
					}
				}
				else if (path >= 25 && path < 50) { //x--, west
					if (lastX > x) { //prevent backtracking
						continue;
					}
					else {
						if (x <= 0) {
							x = 0;
						}
						else {
							x--;
							direction = 2;
						}
						choosePath = false;
					}
				}
				else if (path >= 50 && path < 75) { //z++, north
					if (lastZ < z) { //prevent backtracking
						continue;
					}
					else {
						if (z >= MAX_SECTOR_TRANSFORM - 1) {
							z = MAX_SECTOR_TRANSFORM - 1;
						}
						else {
							z++;
							direction = 3;
						}
						choosePath = false;
					}
				}
				else if (path >= 75 && path < 100) { //z--, south
					if (lastZ > z) { //prevent backtracking
						continue;
					}
					else {
						if (z <= 0) {
							z = 0;
						}
						else {
							z--;
							direction = 1;
						}
						choosePath = false;
					}
				}
			}

			int currentSpot = s.GetMapTransform(x, y, z);
			if (currentSpot == EMPTY || currentSpot == HALL_DEAD_END) {  //transform not taken up
				bool chooseTile = true;
				while (chooseTile) {

					int type = rand.Next(0, 99);
					if (type < 20) {
						s.SetMapTransform(x, y, z, HALL);
						prevTransform = HALL;
						chooseTile = false;
					}
					else if (type >= 20 && type < 50) {
						s.SetMapTransform(x, y, z, HALL_CORNER);
						prevTransform = HALL_CORNER;
						chooseTile = false;
					}
					else if (type >= 50 && type < 75) {
						s.SetMapTransform(x, y, z, HALL_TRI);
						prevTransform = HALL_TRI;
						chooseTile = false;
					}
					else if (type >= 85 && type < 90) {
						s.SetMapTransform(x, y, z, HALL_QUAD);
						prevTransform = HALL_QUAD;
						chooseTile = false;
					}
					else if (type >= 85 && type < 95 && prevTransform != HALL_HATCH_DOWN) {
						try {
							if (s.GetMapTransform(x, y - 1, z) == 0) {
								s.SetMapTransform(x, y, z, HALL_HATCH_DOWN);
								prevTransform = HALL_HATCH_DOWN;
								chooseTile = false;
							}
						}
						catch (Exception e) { }
					}
					else if (type >= 95 && type < 100 && prevTransform != HALL_LADDER_UP) {
						try {
							if (s.GetMapTransform(x, y + 1, z) == 0) {
								s.SetMapTransform(x, y, z, HALL_LADDER_UP);
								prevTransform = HALL_LADDER_UP;
								chooseTile = false;
							}
						}
						catch (Exception e) { }
					}
				}
				s.SetMapRotation(x, y, z, direction);				
			}		
			else { //When spot is already taken up control when back tracking				
				numberHalls++;
				lastX = x;
				lastZ = z;
				catchLoop++;
				prevTransform = EMPTY;
			}
		}	
		return continueLater;
	}

	//TODO: Adds rooms connected to hallway
	private IEnumerator AddRooms(Sector s) {
		yield return null;
	}

	//TODO: Adds minable asteroids to the sector
	private IEnumerator AddAsteroids(Sector s) {
		yield return null;
	}

	private IEnumerator AddItems(Sector s) {
		yield return null;
	}

	public void SpawnEnemies(int sectorX, int sectorY) {
		int type = rand.Next(1, EnemySpawning.ENEMY_PREFAB_COUNT);
		int count = rand.Next(1, 5);
		enemySpawner.SpawnEnemies(sectors[sectorX,sectorY], type, count);
	}
	//Check if next to spawn
	private bool CheckForSpawn(Sector s, int x, int y, int z) {
		bool xP = false;
		bool xN = false;
		bool zP = false;
		bool zN = false;

		if (x < MAX_SECTOR_TRANSFORM-1) {
			xP = s.GetMapTransform(x + 1, y, z) == START;
		}
		if (x > 0) {
			xN = s.GetMapTransform(x - 1, y, z) == START;
		}
		if (z < MAX_SECTOR_TRANSFORM-1) {
			zP = s.GetMapTransform(x, y, z+1) == START;
		}
		if (z > 0) {
			zN = s.GetMapTransform(x, y, z - 1) == START;
		}

		return xP || xN || zP || zN;
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
	//TODO: Add coroutines
	public void InstanciateSector(int xSector, int ySector) {
		for (int x = 0; x < 16; x++) {
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					int transInt = sectors[xSector, ySector].GetMapTransform(x, y, z);
					int transBelow = sectors[xSector, ySector].GetMapTransform(x, y - 1, z);
					int transAbove = sectors[xSector, ySector].GetMapTransform(x, y + 1, z);
					if (transInt != EMPTY) {
						Transform transform = tiles[transInt];
						Vector3 position = new Vector3(
							(TILE_SIZE * MAX_SECTOR_TRANSFORM) * xSector + (TILE_SIZE * x) + transform.position.x, 
							TILE_HEIGHT * y + transform.position.y, 
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
