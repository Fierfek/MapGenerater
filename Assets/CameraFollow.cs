using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Camera cam;
	public float speed;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		if(cam == null)
			cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		cam.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
	}
}
