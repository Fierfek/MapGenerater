using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	public Text mapWidth, mapHeight, numSeeds;
	public Toggle toggle, circular;
	bool euclidean, showSeeds;
	float tileSize;
	int width = 1, height = 1, isles;
	GameObject[] tiles;
	GameObject tile;
	List<GameObject> currentSet = new List<GameObject>();
	int[,] array;

	struct Seed {
		public int x, y, type;
		public Seed(int x, int y, int type) {
			this.x = x;
			this.y = y;
			this.type = type;
		}
	}

	Seed[] seeds;

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

		isles = int.Parse (numSeeds.text);
		array = new int[width, height];
		seeds = new Seed[isles * 5];

		for (int i = 0; i < isles; i ++) {
			seeds [i] = new Seed (Random.Range (0, width - 1), Random.Range (0, height - 1), Random.Range (0, tiles.Length - 2));
		}

		if (circular.isOn) {
			for(int i = 0; i < isles; i ++) {
				seeds[i + isles] = new Seed(seeds[i].x + width, seeds[i].y, seeds[i].type);
				seeds[i + isles * 2] = new Seed(seeds[i].x - width, seeds[i].y, seeds[i].type);
				seeds[i + isles * 3] = new Seed(seeds[i].x, seeds[i].y + height, seeds[i].type);
				seeds[i + isles * 4] = new Seed(seeds[i].x, seeds[i].y - height, seeds[i].type);
			}
		}
		

		float dist = int.MaxValue, least = int.MaxValue;
		int type = 0;
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				least = int.MaxValue;
				dist = int.MaxValue;
				if(circular.isOn) {
					for(int k = 0; k < isles * 5; k++) {
						if(euclidean)
							dist = Mathf.Sqrt(Mathf.Pow(i - seeds[k].x, 2) + Mathf.Pow(j - seeds[k].y, 2));
						else
							dist = Mathf.Abs(i - seeds[k].x) + Mathf.Abs(j - seeds[k].y);
						
						if(least > dist) {
							least = dist;
							type = seeds[k].type;
						}
					}
				} else {
					for(int k = 0; k < isles; k++) {
						if(euclidean)
							dist = Mathf.Sqrt(Mathf.Pow(i - seeds[k].x, 2) + Mathf.Pow(j - seeds[k].y, 2));
						else
							dist = Mathf.Abs(i - seeds[k].x) + Mathf.Abs(j - seeds[k].y);
						
						if(least > dist) {
							least = dist;
							type = seeds[k].type;
						}
					}
				}
				array[i, j] = type;
			}
		}

		if (toggle.isOn) {
			DisplaySeeds();
		}

		bool yes = false;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if(array[i, j] == 0) {
					if(i + 1 < width) 
						if(array[i + 1, j] == 1)
							yes = true;
					if(i - 1 >= 0) 
						if(array[i - 1, j] == 1)
							yes = true;
					if(j + 1 < height) 
						if(array[i, j + 1] == 1)
							yes = true;
					if(j - 1 >= 0) 
						if(array[i, j - 1] == 1)
							yes = true;
						
					if(yes) {
						array[i, j] = 2;
					}
				}
				
				yes = false;
			}
		}

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				currentSet.Add (Instantiate (tiles [array [i, j]], new Vector3 (i * tileSize, j * tileSize, 0), Quaternion.identity) as GameObject);
			}
		}
	}

	public void DisplaySeeds() {
		for(int i = 0; i < isles; i ++) {
			array[seeds[i].x, seeds[i].y] = tiles.Length - 1;
		}
	}

	public void euclideanToggle() {
		if (euclidean = true) {
			euclidean = false;
		} else {
			euclidean = true;
		}
	}
}
