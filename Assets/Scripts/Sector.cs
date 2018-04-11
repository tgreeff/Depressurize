using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector{
	//TODO: remove map array at a later date
	//private Transform[,,] map;  //This contains the parts that are setup for the map.
	private int[,,] mapInt;
	private int[,,] mapRot;
	public Vector2 coordinates;
	public bool generated;  //Allows for quicker checking of generation
	public bool instanciated; //Allows for quick checking whether it is in the game
	public int numHallVertex; //number of hall vertexes placed in this sector

	//-------CONSTANTS/-------
	public const int MAX_TRANSFORM = 16;

	//Constructor for sector struct
	public Sector(int x, int y) {
		//Needs to be transform to hold the direction to rotate and location
		//map = new Transform[MAX_TRANSFORM, MAX_TRANSFORM, MAX_TRANSFORM];
		mapInt = new int[MAX_TRANSFORM, MAX_TRANSFORM, MAX_TRANSFORM];
		coordinates = new Vector2(x, y); //Coords in array
		generated = false;
		instanciated = false;
		numHallVertex = 0;
	}

	//Set the transform to the block transform
	public void SetMapTransform(int x, int y, int z, int type) {
		if (x > MAX_TRANSFORM - 1 || x < 0) {
			return;
		}
		if (y > MAX_TRANSFORM - 1 || y < 0) {
			return;
		}
		if (z > MAX_TRANSFORM - 1 || z < 0) {
			return;
		}

		mapInt[x, y, z] = type;
	}

	//Get the transform located in the map array
	public int GetMapTransform(int x, int y, int z) {
		return mapInt[x, y, z];
	}

	//Set the transform to the block transform
	public void SetMapRotation(int x, int y, int z, int type) {
		if (x > MAX_TRANSFORM - 1 || x < 0) {
			return;
		}
		if (y > MAX_TRANSFORM - 1 || y < 0) {
			return;
		}
		if (z > MAX_TRANSFORM - 1 || z < 0) {
			return;
		}

		mapInt[x, y, z] = type;
	}

	//Get the transform located in the map array
	public int GetMapRotation(int x, int y, int z) {
		return mapRot[x, y, z];
	}

	//Can be used to set the map transforms and clear using null
	public void SetMap(int[,,] mI) {	
		if(mI == null) { //For destroying the transform map
			for (int x = 0; x < MAX_TRANSFORM; x++) {
				for (int y = 0; y < MAX_TRANSFORM; y++) {
					for (int z = 0; z < MAX_TRANSFORM; z++) {
						mapInt[x, y, z] = 0;
					}
				}
			}
			mapInt = null;
		}
		else {  //For setting or copying the transform map

			mapInt = mI;
			for (int x = 0; x < MAX_TRANSFORM; x++) {
				for (int y = 0; y < MAX_TRANSFORM; y++) {
					for (int z = 0; z < MAX_TRANSFORM; z++) {
						mapInt[x, y, z] = mI[x, y, z];
					}
				}
			}
		}
		
	}

	//Get the transform located in the map array
	public int GetMap(int x, int y, int z) {
		return mapInt[x, y, z];
	}

	public Vector3 FindFirstInstance(int transform) {
		for (int x = 0; x < MAX_TRANSFORM; x++) {
			for(int y= 0; y < MAX_TRANSFORM; y++) {
				for(int z = 0; z < MAX_TRANSFORM; z++) {
					if(mapInt[x,y,z] == transform) {
						return new Vector3(x, y, z);
					}
				}
			}
		}
		return new Vector3(0,0,0);
	}
}
