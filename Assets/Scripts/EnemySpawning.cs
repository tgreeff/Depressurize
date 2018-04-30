using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning {
	public int totalEnemies;
	private int numberSpider;
	private int numberFlying;
	private Sector currentSector;

	private Transform[] prefabs;

	//Constants for enemies
	public const int MAX_ENEMY_COUNT = 15;
	public const int ENEMY_PREFAB_COUNT = 2;
	public const int ENEMY_EMPTY = 0;
	public const int ENEMY_SPIDER = 1;
	public const int ENEMY_FLYING = 2;

	public EnemySpawning(Transform[] enemies) {

		prefabs = new Transform[enemies.Length];
		for (int x = 0; x < enemies.Length; x++) {
			prefabs[x] = enemies[x];
		}

		totalEnemies = 0;
		numberFlying = 0;
		numberSpider = 0;
	}

	public void SpawnEnemies(Sector s, int type, int count, int playerX, int playerY, int playerZ) {
		if(totalEnemies >= MAX_ENEMY_COUNT) {
			return;
		}

		currentSector = s;
		Vector3Int[] tileLocations = new Vector3Int[100];

		int index = 0;
		for(int x = -5; x < 4; x++){
			for (int z = -5; z < 4; z++) {
				try {
					if (s.GetMapTransform(playerX + x, playerY, playerZ + z) != 0) {
						tileLocations[index] = new Vector3Int(playerX + x , playerY, playerZ + z);
						index++;
					}
				} catch (Exception e) {
					Debug.Log(e.ToString());
				}
			}			
		}

		for (int t = 0; t < count; t++) {
			int i = UnityEngine.Random.Range(0, index);
			int x = tileLocations[i].x;
			int y = tileLocations[i].y;
			int z = tileLocations[i].z;
			if (s.GetMapTransform(x, y, z) != 0 ) {
				Instantiate(type, x, y, z);
				count--;
				totalEnemies++;
				if(type == 1) {
					numberSpider++;
				}
				else if(type == 2) {
					numberFlying++;
				}
			}
			//yield return null; // new WaitForSeconds(0.1f);
		}
	}

	//Instantiates the transform of the tile 
	public void Instantiate(int type, int x, int y, int z) {
		Transform transform = prefabs[type];
		Vector3 position = new Vector3(
			(Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM) * currentSector.coordinates.x + (Generation.TILE_SIZE * x) + transform.position.y,
			Generation.TILE_HEIGHT * y + transform.position.y,
			(Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM) * currentSector.coordinates.y + (Generation.TILE_SIZE * z) + transform.position.z);
		int transRot = currentSector.GetMapRotation(x, y, z);

		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3(0, 90 * transRot, 0);
		GameObject.Instantiate(transform, position, rotation);
		currentSector.instanciated = true;
	}
}
