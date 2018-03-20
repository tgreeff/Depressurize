using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector{
	private Transform[,,] map;  //This contains the parts that are setup for the map.
	public bool generated;  //Allows for quicker checking of generation
	public bool instanciated; //Allows for quick checking whether it is in the game

	//-------CONSTANTS/-------
	private const int MAX_TRANSFORM = 16;

	//Constructor for sector struct
	public Sector() {
		this.map = new Transform[MAX_TRANSFORM, MAX_TRANSFORM, MAX_TRANSFORM];
		this.generated = false;
		this.instanciated = false;
	}

	//Set the transform to the block transform
	public void SetMapTransform(int x, int y, int z, Transform block) {
		map[x, y, z] = block;
	}

	//Get the transform located in the map array
	public Transform GetMapTransform(int x, int y, int z) {
		return map[x, y, z];
	}

	//Set the rotation of transform
	public void SetTransformRotation(int x, int y, int z, Vector3 v) {
		map[x, y, z].transform.rotation = v;
	}

	//Get the rotation of transform
	public Vector3 GetTransformRotation(int x, int y, int z) {
		return map[x, y, z].transform.rotation;
	}

	//set the position of transform
	public void SetTransformRotation(int x, int y, int z, Vector3 v) {
		map[x, y, z].transform.position = v;
	}

	//Get the position of transform
	public Vector3 GetTransformPostition(int x, int y, int z) {
		return map[x, y, z].transform.position;
	}

	//Can be used to set the map transforms and clear using null
	public void SetMap(Transform[,,] m) {	
		if(m == null) { //For destroying the transform map
			for (int x = 0; x < MAX_TRANSFORM; x++) {
				for (int y = 0; y < MAX_TRANSFORM; y++) {
					for (int z = 0; z < MAX_TRANSFORM; z++) {
						map[x, y, z] = null;
					}
				}
			}
			map = null;
		}
		else {  //For setting or copying the transform map
			map = m;
			for (int x = 0; x < MAX_TRANSFORM; x++) {
				for (int y = 0; y < MAX_TRANSFORM; y++) {
					for (int z = 0; z < MAX_TRANSFORM; z++) {
						map[x, y, z] = m[x, y, z];
					}
				}
			}
		}
		
	}
}
