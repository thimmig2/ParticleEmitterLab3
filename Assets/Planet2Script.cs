using UnityEngine;
using System;
using System.Collections;

public class Planet2Script : MonoBehaviour {

	public GameObject planet;
	private float mass = (float)Math.Pow(10, 13) * 500F;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(planet.transform.position, Vector3.up, 10 * Time.deltaTime);
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
