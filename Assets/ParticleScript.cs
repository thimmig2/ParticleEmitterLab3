using UnityEngine;
using System;
using System.Collections;

public class ParticleScript : MonoBehaviour {

	private static int count = 0;
	private static float timeStep = Time.deltaTime * 10;
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

	private Vector3 position, velocity, acceleration, force, lastAcceleration;
	private int age, maxAge;
	private float mass;
	private BoundingVolume bounds;

	public void setup(Transform parent) {
		transform.name = "Particle" + count;
		ParticleScript.count++;

		transform.parent = parent;
		transform.position = generateStartPosition();
		this.age = 0;
		this.maxAge = UnityEngine.Random.Range(2,15);
		this.mass = UnityEngine.Random.Range(massMin, massMax);
		this.velocity = new Vector3(UnityEngine.Random.Range(vXMin, vXMax), UnityEngine.Random.Range(vYMin, vYMax), UnityEngine.Random.Range(vZMin, vZMax));
		this.lastAcceleration = new Vector3(UnityEngine.Random.Range(fXMin, fXMax), UnityEngine.Random.Range(fYMin, fYMax), UnityEngine.Random.Range(fZMin, fZMax));

		renderer.material.color = Color.green;
		this.bounds = new BoundingVolume(transform);
	}

	private Vector3 generateStartPosition() {
		return transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(0F, 10F));		
	}

	private float rand(float min1, float max1, float min2, float max2) {
		if(UnityEngine.Random.Range(0, 2) == 0) {
			return UnityEngine.Random.Range(min1, max1);
		} else {
			return UnityEngine.Random.Range(min2, max2);			
		}
	}

	void Start () {}
	
	// Update is called once per frame
	void Update () {
		ageCheck();

		updateAcceleration();
		updateVelocity();
		updatePosition();
	}

	private void updatePosition() {
		transform.position += this.velocity * ParticleScript.timeStep;
	}

	private void updateVelocity() {
		this.velocity += this.acceleration * ParticleScript.timeStep;
	}

	private void updateAcceleration() {
		this.force = GameObject.Find("Planet").GetComponent<PlanetScript>().gravitationalForce(this.mass, transform.position) + GameObject.Find("Planet2").GetComponent<Planet2Script>().gravitationalForce(this.mass, transform.position);
		// = new Vector3(UnityEngine.Random.Range(fXMin, fXMax), UnityEngine.Random.Range(fYMin, fYMax), UnityEngine.Random.Range(fZMin, fZMax));

		this.acceleration = (this.force/this.mass);
		//((this.force/this.mass) + this.lastAcceleration) / 2;
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

	private void incrementAge() {
		this.age++;	
		float redPart = (float)Math.Round((double)(this.age + 1) / this.maxAge, 1),
				greenPart = (float)Math.Round((decimal)(this.maxAge - this.age) / this.maxAge, 1);
		renderer.material.color = new Color(redPart, greenPart, 0, 1);
	}

	public void checkPlaneCollisions(GameObject plane) {
		PlaneScript planeScript = plane.GetComponent<PlaneScript>();

		if(planeScript.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();
			this.velocity = Vector3.Reflect(this.velocity, planeScript.normal);
			this.velocity *= .95F;
		}
	}

	public void checkPlanetCollisions(GameObject planet) {
		PlanetScript planetScript = planet.GetComponent<PlanetScript>();

		if(planetScript.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();
			Vector3 planetNormal = planet.transform.position - transform.position;
			this.velocity = Vector3.Reflect(this.velocity, planetNormal.normalized);
			this.velocity *= .95F;
		}
	}

	public void checkPlanet2Collisions(GameObject planet2) {
		Planet2Script planet2Script = planet2.GetComponent<Planet2Script>();

		if(planet2Script.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();
			Vector3 planet2Normal = planet2.transform.position - transform.position;
			this.velocity = Vector3.Reflect(this.velocity, planet2Normal.normalized);
			this.velocity *= .95F;
		}
	}

}
