using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	// Use this for initialization
	
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.name == "Main Camera")
			GenerateMap.instance.AddPoint(gameObject.transform.position.x, gameObject.transform.position.y);
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.name == "Boxy")
			GenerateMap.instance.RemovePoint(gameObject.transform.position.x, gameObject.transform.position.y);
	}
}
