using UnityEngine;
using System;
using System.Collections;

public class PlanetScript : MonoBehaviour {

	private float mass = (float)Math.Pow(10, 13) * 45F;

	void Start () {}
	
	void Update () {

		// decrease mass of this plannet if 1 key is down
		if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)) {
			this.mass -= (float)Math.Pow(10, 13);
			Debug.Log(this.mass);
		}

		if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) {
			this.mass += (float)Math.Pow(10, 13);
		}
	}

	public Vector3 gravitationalForce(float objectMass, Vector3 obectPosition) {
		const float gConstant = 0.00000000006673F;

		float forceMagnitude = (gConstant * this.mass * objectMass) / Vector3.Distance(transform.position, obectPosition);
		Vector3 forceDirection = (transform.position - obectPosition);
		
		return forceDirection.normalized * forceMagnitude;
	}

	public float computeDistance(Vector3 center) {
		return Vector3.Distance(center, transform.position) - (transform.lossyScale.x / 2);
	}
}
