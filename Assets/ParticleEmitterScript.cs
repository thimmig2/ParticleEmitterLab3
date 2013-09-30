using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* 
	This class instantiates and tracks particles. 
*/
public class ParticleEmitterScript : MonoBehaviour {

/* ------------------ Constants ------------------ */	

	private const int particleCount = 75;

/* --------------- End Constants ----------------- */		


	// objects that particles can collide with
	public GameObject plane, planet, planet2;
	// the particle prefab
	public Transform myParticle;

	// will hold all of the current living particles
	private List<GameObject> particles;


	// initializes particles
	void Start () {
		particles = new List<GameObject>();
		
		for(int i=0; i<particleCount; i++) {
			particles.Add(createParticle());			
		}
	}
	
	// Update is called once per frame
	void Update () {
		// remove all particles that have died
		particles.RemoveAll(item => item == null);
		
		// respawn new particles
		for(int i = particles.Count; i < particleCount; i++) {
			particles.Add(createParticle());
		}

		foreach(GameObject particle in particles) {
			particle.GetComponent<ParticleScript>().checkPlaneCollisions(plane);
			particle.GetComponent<ParticleScript>().checkPlanetCollisions(planet);
			particle.GetComponent<ParticleScript>().checkPlanet2Collisions(planet2);
		}
	}

	// create a new particle based on the location of the emitter and return it
	private GameObject createParticle() {
		Component newParticle = (Component)Instantiate(myParticle, transform.position, transform.rotation);
		newParticle.GetComponent<ParticleScript>().setup(transform);
		return newParticle.gameObject;
	}
}
