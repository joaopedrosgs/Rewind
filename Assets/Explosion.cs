using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

	public float Range;
	// Use this for initialization
	void Start () {
		AreaDamageEnemies(Range);
	}
	
	// Update is called once per frame

	void AreaDamageEnemies(float radius)
	{
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
		foreach (Collider col in objectsInRange)
		{
			if (col.CompareTag("Enemy"))
				col.GetComponent<Enemy>().StartAutoDestroy();

		}

	}

}
