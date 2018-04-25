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
	public const int MAX_ENEMY_COUNT = 10;
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
		currentSector = s;

		while(totalEnemies != MAX_ENEMY_COUNT) {
			int x = Generation.rand.Next(0, Generation.MAX_SECTOR_TRANSFORM);
			int y = Generation.rand.Next(0, Generation.MAX_SECTOR_TRANSFORM);
			int z = Generation.rand.Next(0, Generation.MAX_SECTOR_TRANSFORM);

			if (s.GetMapTransform(x, y, z) != 0 ) {
				Instantiate(type, x, y, z);
			}
		}
	}

	//Instantiates the transform of the tile 
	public void Instantiate( int type, int x, int y, int z) {
		Transform transform = prefabs[type];
		Vector3 position = new Vector3(
			(Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM) * currentSector.coordinates.x + (Generation.TILE_SIZE * x) + transform.position.y,
			Generation.TILE_SIZE * y + transform.position.y,
			(Generation.TILE_SIZE * Generation.MAX_SECTOR_TRANSFORM) * currentSector.coordinates.y + (Generation.TILE_SIZE * z) + transform.position.z);
		int transRot = currentSector.GetMapRotation(x, y, z);

		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3(0, 90 * transRot, 0);
		GameObject.Instantiate(transform, position, rotation);
		currentSector.instanciated = true;
	}
}
