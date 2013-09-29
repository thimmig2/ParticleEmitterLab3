using UnityEngine;
using System.Collections;

public class PlaneScript : MonoBehaviour {

	public Vector3 v0, normal;

	// Use this for initialization
	void Start () {
		computeNormal();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void computeNormal() {
		this.v0 = transform.position;
		this.normal = transform.up; 
	}

	public float computeDistance(Vector3 point) {
		// compute the distance from an arbitrary point to the plane
		return Vector3.Dot(this.normal, (point - v0)) / this.normal.magnitude;
	}
}
