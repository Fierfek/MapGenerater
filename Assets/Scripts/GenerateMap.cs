using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	public Text mapWidth, mapHeight, seeds;
	public Toggle toggle;
	bool euclidean, showSeeds;
	float tileSize;
	int width = 1, height = 1, isles;
	GameObject[] tiles;
	GameObject tile;
	List<GameObject> currentSet = new List<GameObject>();
	int[,] array;

	struct Point {
		public int x, y;
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	Point[] seed;

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

		isles = int.Parse (seeds.text);

		array = new int[width, height];

		seed = new Point[isles];

		for(int i = 0; i < isles; i ++) {
			seed[i] = new Point(Random.Range(0, width - 1), Random.Range(0, height - 1));
			array[seed[i].x, seed[i].y] = Random.Range(0, 2);
		}

		float dist = int.MaxValue, least = int.MaxValue;
		int type = 0;
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				least = int.MaxValue;
				dist = int.MaxValue;
				for(int k = 0; k < isles; k++) {
					if(euclidean)
						dist = Mathf.Sqrt(Mathf.Pow(i - seed[k].x, 2) + Mathf.Pow(j - seed[k].y, 2));
					else
						dist = Mathf.Abs(i - seed[k].x) + Mathf.Abs(j - seed[k].y);
					
					if(least > dist) {
						least = dist;
						type = array[seed[k].x, seed[k].y];
					}
				}
				array[i, j] = type;
			}
		}

		if (toggle.isOn) {
			DisplaySeeds();
		}

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				currentSet.Add (Instantiate (tiles [array [i, j]], new Vector3 (i * tileSize, j * tileSize, 0), Quaternion.identity) as GameObject);
			}
		}
	}

	public void DisplaySeeds() {
		for(int i = 0; i < isles; i ++) {
			array[seed[i].x, seed[i].y] = 3;
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
