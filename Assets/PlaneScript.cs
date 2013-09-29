using UnityEngine;
using System.Collections;

public class PlaneScript : MonoBehaviour {

	private Vector3 v0, normal;

	// Use this for initialization
	void Start () {
		computeNormal();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void computeNormal() {
		Mesh planeMesh = transform.GetComponent<MeshFilter>().mesh;

		this.v0 = planeMesh.vertices[0];
		this.normal = planeMesh.normals[0];
	}

	public float computeDistance(Vector3 point) {
		// compute the distance from an arbitrary point to the plane
		return Vector3.Dot(this.normal, (point - v0));
	}
}
