using UnityEngine;
using System.Collections;

public class PlaneScript : MonoBehaviour {

	public Vector3 v0, normal;
	private static float timeStep = Time.deltaTime * 10000;

	// Use this for initialization
	void Start () {
		this.computeNormal();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Rotate(Vector3.forward, timeStep);
			this.computeNormal();
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Rotate(Vector3.back, timeStep);
			this.computeNormal();
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.Rotate(Vector3.up, timeStep);
			this.computeNormal();
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.Rotate(Vector3.down, timeStep);
			this.computeNormal();
		}
	}

	private void computeNormal() {
		this.v0 = transform.position;
		this.normal = transform.up; 
	}

	public float computeDistance(Vector3 center) {
		// compute the distance from an arbitrary point to the plane
		return Vector3.Dot(this.normal, (center - v0)) / this.normal.magnitude;
	}
}
