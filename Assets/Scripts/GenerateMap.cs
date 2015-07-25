using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	public Text mapWidth, mapHeight;
	public float tileSize;
	int width = 1, height = 1;
	GameObject[] tiles;
	GameObject tile;
	List<GameObject> currentSet = new List<GameObject>();

	// Use this for initialization
	void Start () {
		tiles = Resources.LoadAll<GameObject>("tiles");
		tileSize = tiles[0].GetComponent<Renderer>().bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Generate() {
		foreach(GameObject thing in currentSet) {
			Destroy(thing);
		}

		if (mapWidth.text.Length > 0 || mapHeight.text.Length > 0) {
			width = int.Parse (mapWidth.text);
			height = int.Parse (mapHeight.text);
		}

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(i == 0 || j== 0 || i == width - 1 || j == height -1) {
					tile = tiles[1];
				} else {
					tile = tiles[0];
				}
				currentSet.Add(Instantiate(tile	, new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity) as GameObject);
			}
		}
	}
}
