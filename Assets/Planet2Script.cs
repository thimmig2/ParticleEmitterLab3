using UnityEngine;
using System;
using System.Collections;

public class Planet2Script : MonoBehaviour {

	public GameObject planet;
	private float mass = (float)Math.Pow(10, 13) * 45F;
	private float speed;	

	// Use this for initialization
	void Start () {
		this.speed = 15F * Time.deltaTime;	
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)) {
			this.mass -= (float)Math.Pow(10, 13);
		}

		if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)) {
			this.mass += (float)Math.Pow(10, 13);
		}

		transform.RotateAround(planet.transform.position, Vector3.up, this.speed);
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
