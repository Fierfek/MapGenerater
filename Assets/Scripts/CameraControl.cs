using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	Camera cam;
	float x, y, scroll, speed, minZoom = 5, maxZoom = 20;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.GetAxisRaw ("Horizontal");
		y = Input.GetAxisRaw ("Vertical");
		scroll = Input.GetAxisRaw ("Mouse ScrollWheel") * 10;

		speed = (maxZoom - minZoom) / (Mathf.Clamp(cam.orthographicSize - scroll, minZoom, maxZoom) - minZoom);
		speed = 1/speed;
		speed = Mathf.Clamp (speed, .2f, 1.0f);

		cam.orthographicSize = Mathf.Clamp (cam.orthographicSize - scroll, minZoom, maxZoom);
		cam.transform.position = new Vector3 (cam.transform.position.x + (x * speed), cam.transform.position.y + (y * speed), cam.transform.position.z);
	}
}
