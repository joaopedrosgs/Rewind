using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour {
	public GameObject[] Projectiles;
	public int SelectedProjectile = 0;
	public float Cooldown = 0.5f;
	private DateTime _lastShoot;
	public Image GrenadeUi;

	public Sprite[] GrenadeTexture;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0) && _lastShoot.AddSeconds(Cooldown) < DateTime.Now) {
			var projectile = Instantiate(Projectiles[SelectedProjectile]);	
			projectile.transform.position = transform.position + Camera.main.transform.forward ;
			projectile.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward*13;
			_lastShoot = DateTime.Now;
		}
		if(Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f)
		{
			SelectedProjectile = SelectedProjectile == 0 ? 1 : 0;
			GrenadeUi.sprite = GrenadeTexture[SelectedProjectile];
		}
	}
}
