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
	int[,] array;

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

		array = new int[width, height];

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				array[i,j] = 1;
			}
		}

		for(int islands = 0; islands < 8; islands ++) {
			array[Random.Range(2, width - 2), Random.Range(2, height - 2)] = 0;
		}

		/*for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(Random.value > .5)
					array[i,j] = 1;
				else
					array[i,j] = 0;

				if(i <= 1 || j<= 1 || i >= width - 2 || j >= height -2) {
					array[i, j] = 1;
				}
			}
		}*/

		for (int i = 2; i < width - 2; i++) {
			for(int j = 2; j < height - 2; j++) {
				if(array[i,j] == 0) {
					array[i - 1,j - 1] = 2;
					array[i - 1,j] = 2;
					array[i - 1,j + 1] = 2;
					array[i,j - 1] = 2;
					array[i,j + 1] = 2;
					array[i + 1,j - 1] = 2;
					array[i + 1,j] = 2;
					array[i + 1,j + 1] = 2;
				}
			}
		}

		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				currentSet.Add(Instantiate(tiles[array[i,j]], new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity) as GameObject);
			}
		}
	}
}
