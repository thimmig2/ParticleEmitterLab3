using UnityEngine;
using System;
using System.Collections;

public class ParticleScript : MonoBehaviour {

	// keeps track of all current particles for naming purposes
	private static int count = 0;
	// the timestep for updates
	private static float timeStep = Time.deltaTime * 100;

/* ------------------ Constants ------------------ */	
	
	// mass initialization ranges
	private const float massMin = 1.0F,
							massMax = 10000.0F;
	
	// velocity initialization ranges
	private const float vXMin = -10.0F,
							vXMax = 10.0F,
							vYMin = -10.0F,
							vYMax = 10.0F,
							vZMin = -10.0F,
							vZMax = 10.0F;

	// acceleration initialization ranges
	private const float fXMin = -100.0F,
							fXMax = 100.0F,
							fYMin = -100.0F,
							fYMax = 100.0F,
							fZMin = -100.0F,
							fZMax = 100.0F;

/* --------------- End Constants ----------------- */

	// a cool mesh to change particles to at runtime
	public Transform agedParticle;

	// particle variables that keep track of physics
	private Vector3 position, velocity, acceleration, force, lastAcceleration;
	private float mass;

	// age is based on how many collisions the particle has endured
	private int age, maxAge;
	
	// current color of the particle
	private Color color;
	
	// bounding volume for the particle to calculate collisions
	private BoundingVolume bounds;

	void Start () {}

	public void setup(Transform parent) {
		transform.name = "Particle" + count;
		ParticleScript.count++;

		transform.parent = parent;
		transform.position += (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(0F, 25F));
		this.age = 0;
		this.maxAge = UnityEngine.Random.Range(2,15);
		this.mass = UnityEngine.Random.Range(massMin, massMax);
		this.velocity = new Vector3(UnityEngine.Random.Range(vXMin, vXMax), UnityEngine.Random.Range(vYMin, vYMax), UnityEngine.Random.Range(vZMin, vZMax));
		this.lastAcceleration = new Vector3(UnityEngine.Random.Range(fXMin, fXMax), UnityEngine.Random.Range(fYMin, fYMax), UnityEngine.Random.Range(fZMin, fZMax));

		this.setColor(Color.green);
		this.bounds = new BoundingVolume(transform);
	}

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

		//this.acceleration = (this.force/this.mass);
		this.lastAcceleration = this.acceleration;
		this.acceleration = ((this.force/this.mass) + this.lastAcceleration) / 2;
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
	}

	private void setColor(Color color) {
		this.color = color;
		renderer.material.color = color;
	}

	public void checkPlaneCollisions(GameObject plane) {
		PlaneScript planeScript = plane.GetComponent<PlaneScript>();

		// collision detected
		if(planeScript.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();

			// particles will stick to the plane
			this.velocity = Vector3.zero;
			

			// create a cool explosion effect slowly changing color to red and incrementing size
			this.maxAge = 75;
			float redPart = (float)Math.Round((double)(this.age + 1) / this.maxAge, 1),
				greenPart = (float)Math.Round((decimal)(this.maxAge - this.age) / this.maxAge, 1);
			this.setColor(new Color(redPart, greenPart, 0, 1));
			
			// scale and recompute the bounds
			transform.localScale += new Vector3(.09F, .09F, .09F);
			this.bounds = new BoundingVolume(transform);
		}
	}

	public void checkPlanetCollisions(GameObject planet) {
		PlanetScript planetScript = planet.GetComponent<PlanetScript>();

		// collision detected
		if(planetScript.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();
			Vector3 planetNormal = planet.transform.position - transform.position;
			this.velocity = Vector3.Reflect(this.velocity, planetNormal.normalized);
			
			// slow particles down when they hit the plannet
			this.velocity *= .75F;

			// check if this particle has already hit something
			if((Vector4)this.color == (Vector4)Color.green) {
				// we'll make double hit particles cubes
				this.setColor(Color.cyan);
			} else if((Vector4)this.color == (Vector4)Color.cyan) {
				// we'll make double hit particles cubes
				this.changeParticleMesh();
			} else {
				// if it has hit multiple obejects, turn it yellow
				this.setColor(Color.yellow);
			}
		}
	}

	public void checkPlanet2Collisions(GameObject planet2) {
		Planet2Script planet2Script = planet2.GetComponent<Planet2Script>();

		// collision detected
		if(planet2Script.computeDistance(transform.position) <= bounds.radius) {
			incrementAge();

			// calculate the point to sphere vector (normal)
			Vector3 planet2Normal = planet2.transform.position - transform.position;
			this.velocity = Vector3.Reflect(this.velocity, planet2Normal.normalized);
			
			// knock particles away from the spinning plannet
			this.velocity *= 1.3F;
			
			// check if this particle has already hit something
			if((Vector4)this.color == (Vector4)Color.green) {
				this.setColor(Color.magenta);
			} else if((Vector4)this.color == (Vector4)Color.magenta) {
				// we'll make double hit particles cubes
				this.changeParticleMesh();
			} else {
				// if it has hit multiple obejects, turn it yellow
				this.setColor(Color.yellow);
			}
		}
	}

	private void changeParticleMesh() {
		Mesh newMesh = new Mesh(),
			oldMesh = agedParticle.GetComponent<MeshFilter>().sharedMesh;
		
		newMesh.vertices = oldMesh.vertices;
		newMesh.normals = oldMesh.normals;
		newMesh.uv = oldMesh.uv;
		newMesh.triangles = oldMesh.triangles;

		GetComponent<MeshFilter>().mesh = newMesh;
	}

}
