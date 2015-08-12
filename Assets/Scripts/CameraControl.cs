using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	Camera cam;
	float x, y, scroll, speed, minZoom = 5, maxZoom = 10;
	float width = 0, height = 0;
	public BoxCollider2D boxCol, boxCol2;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.GetAxisRaw ("Horizontal");
		y = Input.GetAxisRaw ("Vertical");
		scroll = Input.GetAxisRaw ("Mouse ScrollWheel") * 5;

		speed = (maxZoom - minZoom) / (Mathf.Clamp(cam.orthographicSize - scroll, minZoom, maxZoom) - minZoom);
		speed = 1/speed;
		speed = Mathf.Clamp (speed, .2f, .5f);

		cam.orthographicSize = Mathf.Clamp (cam.orthographicSize - scroll, minZoom, maxZoom);
		cam.transform.position = new Vector3 (cam.transform.position.x + (x * speed), cam.transform.position.y + (y * speed), cam.transform.position.z);

		height = cam.orthographicSize * 2;
		width = height * cam.aspect;
		boxCol.size = new Vector2 (width, height);
		height = (cam.orthographicSize + 2) * 2;
		width = height * cam.aspect;
		boxCol2.size = new Vector2 (width, height);
	}
}
