using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public PlayerStatus Player;
	// Use this for initialization
	private void Awake()
	{
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Player.TakeBullet();
			Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject, 2f);
		}
		
	}
}
