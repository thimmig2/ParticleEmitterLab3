﻿using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {

	private static int count = 0;
	private static float timeStep = Time.deltaTime * 50;
	private static float massMin = 1.0F,
							massMax = 10000.0F;
	
	private static float vXMin = -10.0F,
							vXMax = 10.0F,
							vYMin = -10.0F,
							vYMax = 10.0F,
							vZMin = -10.0F,
							vZMax = 10.0F;

	private static float fXMin = -1000.0F,
							fXMax = 1000.0F,
							fYMin = -1000.0F,
							fYMax = 1000.0F,
							fZMin = -1000.0F,
							fZMax = 1000.0F;

	private Vector3 position, velocity, acceleration, force;
	private int age, maxAge;
	private float mass;
	private BoundingVolume bounds;

	public void setup(Transform parent) {
		transform.name = "Particle" + count;
		ParticleScript.count++;

		transform.parent = parent;
		transform.position = generateStartPosition();
		this.age = 0;
		this.maxAge = 1000;
		this.mass = Random.Range(massMin, massMax);
		this.velocity = new Vector3(Random.Range(vXMin, vXMax), Random.Range(vYMin, vYMax), Random.Range(vZMin, vZMax));
		
		this.bounds = new BoundingVolume(transform);
	}

	private Vector3 generateStartPosition() {
		return transform.position + (Random.onUnitSphere * Random.Range(5.5F, 8F));		
	}

	private float rand(float min1, float max1, float min2, float max2) {
		if(Random.Range(0, 2) == 0) {
			return Random.Range(min1, max1);
		} else {
			return Random.Range(min2, max2);			
		}
	}

	void Start () {}
	
	// Update is called once per frame
	void Update () {
		ageCheck();

		updateAcceleration();
		updateVelocity();
		updatePosition();
		age++;
	}

	private void updatePosition() {
		transform.position += this.velocity * ParticleScript.timeStep;
	}

	private void updateVelocity() {
		this.velocity += this.acceleration * ParticleScript.timeStep;
	}

	private void updateAcceleration() {
		this.force = GameObject.Find("Planet").GetComponent<PlanetScript>().gravitationalForce(this.mass, transform.position);
		// = new Vector3(Random.Range(fXMin, fXMax), Random.Range(fYMin, fYMax), Random.Range(fZMin, fZMax));

		this.acceleration = this.force/this.mass;
	}

	public void applyForce(Vector3 force) {
		this.force += force;
	}

	private void ageCheck() {
		if(this.age >= this.maxAge) {
			ParticleScript.count--;
			Destroy(transform.gameObject);
		}
	}

	public void checkPlaneCollisions(GameObject plane) {
		PlaneScript planeScript = plane.GetComponent<PlaneScript>();

		if(planeScript.computeDistance(transform.position) <= bounds.radius) {
			this.velocity = Vector3.Reflect(this.velocity, planeScript.normal);
		}
	}

	public void checkPlanetCollisions(GameObject planet) {
		PlanetScript planetScript = planet.GetComponent<PlanetScript>();

		if(planetScript.computeDistance(transform.position) <= bounds.radius) {
			Vector3 planetNormal = planet.transform.position - transform.position;
			this.velocity = Vector3.Reflect(this.velocity, planetNormal.normalized);
		}
	}


}
