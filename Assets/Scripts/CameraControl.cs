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

		cam.transform.position = new Vector3 (cam.transform.position.x + x, cam.transform.position.y + y, cam.transform.position.z);
		cam.orthographicSize = Mathf.Clamp (cam.orthographicSize - scroll, minZoom, maxZoom);
	}
}
