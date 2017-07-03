using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour {
	private DateTime _throwAt;
	public GameObject ExplosionPrefab;
	public float Delay =1f;
	// Use this for initialization
	void Start () {
		_throwAt = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		if (_throwAt.AddSeconds(Delay) >= DateTime.Now) return;
		Destroy(Instantiate(ExplosionPrefab, transform.position, Quaternion.identity), 2);
		Destroy(gameObject);
	}
}
