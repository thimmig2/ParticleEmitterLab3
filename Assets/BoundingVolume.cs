using UnityEngine;
using System.Collections;

public class BoundingVolume {

	private Mesh mesh;
	private Transform transform;

	public Vector3 center;
	public float radius;

	public BoundingVolume(Transform transform) {
		this.transform = transform;
		this.mesh = transform.GetComponent<MeshFilter>().mesh;
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
			float dist = Vector3.Distance(Vector3.Scale(vertice, this.transform.lossyScale), center);

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
