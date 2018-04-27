using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning {
	private int totalEnemies;
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

	public void SpawnEnemies(Sector s, int type, int count) {
		if(totalEnemies >= MAX_ENEMY_COUNT) {
			return;
		}

		currentSector = s;
		Vector3Int[] tileLocations = new Vector3Int[16 * 16 * 16];

		int index = 0;
		for(int x = 0; x < 16; x++){
			for (int y = 0; y < 16; y++) {
				for (int z = 0; z < 16; z++) {
					tileLocations[index] = new Vector3Int(x, y, z);
					index++;
				}
			}
		}

		for (int t = 0; t < count; t++) {
			index = Random.Range(0, 16 * 16 * 16);
			int x = tileLocations[index].x;
			int y = tileLocations[index].y;
			int z = tileLocations[index].z;
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
