using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector{
	private Transform[,,] map;  //This contains the parts that are setup for the map.
	public bool generated;  //Allows for quicker checking of generation+-

	//Set the transform to the block transform
	public void setMapTransform(int x, int y, int z, Transform block) {
		map[x, y, z] = block;
	}

	//Get the transform located in the map array
	public Transform getMapTransform(int x, int y, int z) {
		return map[x, y, z];
	}

	//Can be used to set the map transforms and clear using null
	public void setMap(Transform[,,] m) {
		
		if(m == null) {
			for (int x = 0; x < 16; x++) {
				for (int y = 0; y < 16; y++) {
					for (int z = 0; z < 16; z++) {
						map[x, y, z] = null;
					}
				}
			}
			map = null;
		}
		else {
			map = m;
			for (int x = 0; x < 16; x++) {
				for (int y = 0; y < 16; y++) {
					for (int z = 0; z < 16; z++) {
						map[x, y, z] = m[x, y, z];
					}
				}
			}
		}
		
	}

	//Constructor for sector struct
	public Sector() {
		this.map = new Transform[16, 16, 16];
		this.generated = false;
	}
}
