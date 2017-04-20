using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	public GameObject MonsterCandidate;
	public List<Transform> SpawnPoint;
	public GameObject initFollowTarget;

	public float SpawnMonsterTime = 10;
	private float spawnCounter = 0;

	
	// Update is called once per frame
	void Update () {
		spawnCounter += Time.deltaTime;

		if (spawnCounter >= SpawnMonsterTime) {
			spawnCounter = 0;

			GameObject newMonster = GameObject.Instantiate (MonsterCandidate);
			newMonster.GetComponent<MonsterScript> ().FollowTarget = initFollowTarget;
			newMonster.transform.position = SpawnPoint [Random.Range (0, SpawnPoint.Count)].position;
		}
	}
}
