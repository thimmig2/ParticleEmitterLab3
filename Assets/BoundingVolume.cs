using UnityEngine;
using System.Collections;

public class BoundingVolume {

	private Mesh mesh;

	public Vector3 center;
	public float radius;

	public BoundingVolume(Mesh mesh) {
		this.mesh = mesh;
		computeBound();
	}

	private void computeBound() {
		Vector3 center = Vector3.zero;

		foreach(Vector3 vertice in mesh.vertices) {
			center += vertice;
		}

		center = center / mesh.vertices.Length;

		float maxDistance = 0;
		foreach(Vector3 vertice in mesh.vertices) {
			float dist = Vector3.Distance(vertice, center);

			if(dist > maxDistance) {
				maxDistance = dist;
			}
		}

		this.center = center;
		this.radius = maxDistance;
	} 

	public string toString() {
		return "center:" + center + " radius:" + radius;
	}

}
