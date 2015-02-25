﻿using UnityEngine;
using System.Collections;

public class ProjectileFollow : MonoBehaviour {

	public Transform projectile;
	public Transform farLeft;
	public Transform farRight;

	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		newPosition.x = projectile.position.x;
		newPosition.x = Mathf.Clamp (newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition;
	}
}
