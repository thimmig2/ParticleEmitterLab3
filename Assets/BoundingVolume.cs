using UnityEngine;
using System.Collections;

/* 
	This class takes a mesh and computes a spherical bounding volume for the vertices. 
*/
public class BoundingVolume {

	public Vector3 center;
	public float radius;

	public BoundingVolume(Transform transform) {
		Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
		
		// get the average of all vertices
		// this will be the center
		Vector3 center = Vector3.zero;
		foreach(Vector3 vertice in mesh.vertices) {
			center += vertice;
		}
		center = center / mesh.vertices.Length;

		// figure out the furthest point from the center
		// this will be the edge of our bounding volume
		float maxDistance = 0;
		foreach(Vector3 vertice in mesh.vertices) {
			float dist = Vector3.Distance(Vector3.Scale(vertice, transform.lossyScale), center);

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
