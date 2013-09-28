using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleEmitterScript : MonoBehaviour {

	public Transform myParticle;
	
	private List<GameObject> particles;
	private int particleCount = 40;

	// Use this for initialization
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
	}

	private GameObject createParticle() {
		Component newParticle = (Component)Instantiate(myParticle, transform.position, transform.rotation);
		newParticle.GetComponent<ParticleScript>().setup(transform);
		return newParticle.gameObject;
	}
}
