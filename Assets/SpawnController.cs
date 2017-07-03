using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{

	public GameObject Enemy;

	public GameObject[] SpawnPlaces;

	private int _maxEnemies;

	public int EnemiesNumber;

	private DateTime _lastSpawn;

	private void Awake()
	{
		_maxEnemies = 10;
	}

	// Use this for initialization
	void Update ()
	{
		if(EnemiesNumber < _maxEnemies && _lastSpawn.AddSeconds(1) < DateTime.Now)
			SpawnRandom();
	}
	
	// Update is called once per frame
	void SpawnRandom()
	{
		var enemy = Instantiate(Enemy);
		enemy.transform.position = SpawnPlaces[Random.Range(0, SpawnPlaces.Length)].transform.position;
		EnemiesNumber++;
		_lastSpawn = DateTime.Now;
		
	}
	




}
