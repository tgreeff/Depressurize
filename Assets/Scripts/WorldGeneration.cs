using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldGeneration : MonoBehaviour {

	public GameObject Player;

	//---Blocks---
	public Transform playerSpawn;
	public Transform[] blocks;

	//---Updates---
	public int drawDistance;

	//---World Formation Variables---
	public int option;
    public int mode;
	public int mapSize;
	public int difficulty;
	public int seed;
	
	public int LimitNumberEnemyFactions, LimitNumberEncounters;		
	private int numEnemyFactions, numEncounters;	//Numbers - this is the number generated for number of landmarks  																								 
	private Transform[] enemyFactions, encounters;	//Locations - these contain locations for the varying landmarks. Instantciated with the numbers above				
	
	//---Struct for the overlying formation of the world---	
	Sector[,] sectors;
	
	private struct Sector {
		private Transform[,,] map;	//This contains the parts that are setup for the map.
		public bool generated;	//Allows for quicker checking of generation

		public void setMap(int x, int y, int z, Transform block){
			map[x, y, z] = block;
		}

		public Transform getMap(int x, int y, int z){
			return map[x, y, z];
		}

		//Constructor for sector struct
		public void sector() {
			this.map = new Transform[256, 256, 256];
			this.generated = true;
		}
	}

	// Use this for initialization
	void Start() {

		//Seed Var - these will be called when creating a new sector
		System.Random rand = new System.Random();
		seed = rand.Next();

		sectors = new Sector[1024, 1024]; //Top Right

		//Initialize Sectors - can't instance all together. Needs to do it as you go
		sectors[511, 511].sector(); //++
		sectors[511, 512].sector();	//+-
		sectors[512, 512].sector(); //--
		sectors[512, 511].sector(); //-+

		//Randomize Values
		seaLevel = seed % rand.Next(64, 256);
		numMountains = seed % rand.Next();

		//TODO: add landmarks, add other blocks, add player, enemy spawns, and saving the map.
		//Generate base map from (0,0,0)
		for (int x = 0; x < 256; x++)
		{
			for (int y = 0; y < 256; y++)
			{
				for (int z = 0; z < 256; z++)
				{
					sectors[512, 512].setMap(x, y, z, block);
					sectors[512, 511].setMap(x, y, z, block);
					sectors[511, 511].setMap(x, y, z, block);
					sectors[511, 512].setMap(x, y, z, block);					

					if (x < 64 && z < 64  && y < 1) {
						Instantiate(sectors[512, 512].getMap(x, y, z), new Vector3(x, -y, z), Quaternion.identity);
						Instantiate(sectors[511, 512].getMap(x, y, z), new Vector3(x, -y, -z), Quaternion.identity);
						Instantiate(sectors[511, 511].getMap(x, y, z), new Vector3(-x, -y, -z), Quaternion.identity);
						Instantiate(sectors[511, 512].getMap(x, y, z), new Vector3(-x, -y, z), Quaternion.identity);					
					}
				}
			}
		}

		//TODO: Add types of generation to give options
		if (option == 0){ //Generate fixed world
			//use Sector and generate the entire world		
		}
		else if (option == 1){ //Generate dynamic
			//use Sector and spawn as you go
		}
		else if (option == 2){ //spawn blocks behaind other blocks
			//new struct
		}
	}
	
	// Update is called once per frame to check if the Sector is generated 
	void Update () {
		//if z + draw is not generated
		if (!getSector(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + drawDistance).generated) {

		}

		//if z - draw is not generated
		if (!getSector(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z - drawDistance).generated){

		}

		//if x + draw is not generated
		if (!getSector(Player.transform.position.x + drawDistance, Player.transform.position.y, Player.transform.position.z).generated)
		{

		}

		//if x - draw is not generated
		if (!getSector(Player.transform.position.x - drawDistance, Player.transform.position.y, Player.transform.position.z).generated)
		{

		}
	}

	//Returns the sector based on the coordinates
	Sector getSector(float x, float y, float z){
		return sectors[Mathf.CeilToInt(x / 256) + 511 , Mathf.CeilToInt(z / 256) + 511];
	}
}
