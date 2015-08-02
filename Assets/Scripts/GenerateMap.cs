using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	public Text mapWidth, mapHeight, numSeeds;
	public Toggle toggle, circular;
	bool euclidean, showSeeds;
	float tileSize;
	int width = 1, height = 1, isles, circleMod = 1;
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
		if (Input.GetKey (KeyCode.Space)) {
			Generate ();
		}
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

		if (circular.isOn)
			circleMod = 9;
		else
			circleMod = 1;

		seeds = new Seed[isles * circleMod];

		for (int i = 0; i < isles; i ++) {
			if(Random.Range(0, 100) < 60)
				seeds [i] = new Seed (Random.Range (0, width - 1), Random.Range (0, height - 1), 0);
			else
				seeds [i] = new Seed (Random.Range (0, width - 1), Random.Range (0, height - 1), 1);
		}

		if (circular.isOn) {
			for(int i = 0; i < isles; i ++) {
				seeds[i + (isles)] = new Seed(seeds[i].x + width, seeds[i].y, seeds[i].type);
				seeds[i + (isles * 2)] = new Seed(seeds[i].x - width, seeds[i].y, seeds[i].type);
				seeds[i + (isles * 3)] = new Seed(seeds[i].x, seeds[i].y + height, seeds[i].type);
				seeds[i + (isles * 4)] = new Seed(seeds[i].x, seeds[i].y - height, seeds[i].type);
				seeds[i + (isles * 5)] = new Seed(seeds[i].x + width, seeds[i].y + height, seeds[i].type);
				seeds[i + (isles * 6)] = new Seed(seeds[i].x - width, seeds[i].y + height, seeds[i].type);
				seeds[i + (isles * 7)] = new Seed(seeds[i].x + width, seeds[i].y - height, seeds[i].type);
				seeds[i + (isles * 8)] = new Seed(seeds[i].x - width, seeds[i].y - height, seeds[i].type);
			}
		}

		float dist = int.MaxValue, least = int.MaxValue;
		int type = 0;
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				least = int.MaxValue;
				dist = int.MaxValue;

				for(int k = 0; k < isles * circleMod; k++) {
					if(euclidean)
						dist = Mathf.Sqrt(Mathf.Pow(i - seeds[k].x, 2) + Mathf.Pow(j - seeds[k].y, 2));
					else
						dist = Mathf.Abs(i - seeds[k].x) + Mathf.Abs(j - seeds[k].y);
					
					if(least > dist) {
						least = dist;
						type = seeds[k].type;
					}
				}

				array[i, j] = type;
			}
		}

		if (toggle.isOn) {
			DisplaySeeds();
		}

		beach();

		biomes();

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
		if (euclidean == true) {
			euclidean = false;
		} else {
			euclidean = true;
		}
	}

	private void beach() {

		bool yes = false;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (array [i, j] == 0) {
					if (i + 1 < width)
						if (array [i + 1, j] == 1)
							yes = true;
					if (i - 1 >= 0) 
						if (array [i - 1, j] == 1)
							yes = true;
					if (j + 1 < height) 
						if (array [i, j + 1] == 1)
							yes = true;
					if (j - 1 >= 0) 
						if (array [i, j - 1] == 1)
							yes = true;

					if(!yes && (i == 0 || i == width - 1 || j == 0 || j == height - 1)) {
						if(i == 0)
							if(array[i + width - 1, j] == 1)
								yes = true;
						if(j == 0)
							if(array[i, j + height - 1] == 1)
								yes = true;
						if(i == width - 1)
							if(array[i - width + 1, j] == 1)
								yes = true;
						if(j == height - 1)
							if(array[i, j - height + 1] == 1)
								yes = true;
					}
					
					if (yes) {
						array [i, j] = 2;
					}
				}
				
				yes = false;
			}
		}
	}

	public void biomes() {
		for (int i = 0; i < isles; i ++) {
			if(seeds[i].type == 0) {
				if(Random.Range(0, 101) <= 20) {
					river(seeds[i].x, seeds[i].y, false);
				}
				if(Random.Range(0, 101) <= 30) {
					convertBiome(seeds[i], 3);
					continue;
				}
				if(Random.Range(0, 101) <= 30) {
					convertBiome(seeds[i], 4);
					continue;
				}
			}
		}
	}

	private void convertBiome(Seed s, int type) {
		float dist = int.MaxValue, dist2 = int.MaxValue;
		bool closest = true;
		for (int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(array[i,j] != 2 && array[i,j] != 1) {
					dist2 = Mathf.Sqrt(Mathf.Pow(i - s.x, 2) + Mathf.Pow(j - s.y, 2));
					
					for(int k = 0; k < isles * circleMod; k++) {
						if(!s.Equals(seeds[k]))
							dist = Mathf.Sqrt(Mathf.Pow(i - seeds[k].x, 2) + Mathf.Pow(j - seeds[k].y, 2));

						if(dist2 > dist) {
							closest = false;
							break;
						}
					}

					if(closest && Random.Range (0, 101) <= 80) {
						array[i, j] = type;
					}
					closest = true;
				}
			}
		}
	}

	public void river(int x, int y, bool fork) {

		float dist = int.MaxValue, least = int.MaxValue;
		Seed seed = new Seed(), currentPoint;
		bool more = true, longest = false;
		int tempX = 0, tempY = 0, choice = 0;
		Queue<Seed> points = new Queue<Seed>();

		for (int i = 0; i < isles * circleMod; i ++) {
			if (seeds[i].type == 1) {
				dist = Mathf.Sqrt (Mathf.Pow (x - seeds [i].x, 2) + Mathf.Pow (y - seeds [i].y, 2));
				
				if (least > dist) {
					least = dist;
					seed = seeds[i];
				}
			}
			dist = int.MaxValue;
		}
		least = int.MaxValue;

		if (Random.Range (0, 101) <= 5 && !fork) {
			longest = true;
		}


		while (more) {

			if (Random.Range (0, 101) <= 5) {
				if(longest)
					river (x, y, false);
				else
					river (x, y, true);
			}

			if(!longest) {
				dist = Mathf.Sqrt (Mathf.Pow (x + 1 - seed.x, 2) + Mathf.Pow (y - seed.y, 2));
				if(least > dist) {
					choice = 1;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - 1 - seed.x, 2) + Mathf.Pow (y - seed.y, 2));
				if(least > dist) {
					choice = 2;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - seed.x, 2) + Mathf.Pow (y + 1 - seed.y, 2));
				if(least > dist) {
					choice = 3;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - seed.x, 2) + Mathf.Pow (y - 1 - seed.y, 2));
				if(least > dist) {
					choice = 4;
				}
			} else {
				dist = 0;
				least = 0;
				dist = Mathf.Sqrt (Mathf.Pow (x + 1 - seed.x, 2) + Mathf.Pow (y - seed.y, 2));
				if(least < dist) {
					choice = 1;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - 1 - seed.x, 2) + Mathf.Pow (y - seed.y, 2));
				if(least < dist) {
					choice = 2;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - seed.x, 2) + Mathf.Pow (y + 1 - seed.y, 2));
				if(least < dist) {
					choice = 3;
					least = dist;
				}
				
				dist = Mathf.Sqrt(Mathf.Pow (x - seed.x, 2) + Mathf.Pow (y - 1 - seed.y, 2));
				if(least < dist) {
					choice = 4;
				}
			}

			switch(choice) {
			case 1: tempX += 1; break;
			case 2: tempX -= 1; break;
			case 3: tempY += 1; break;
			case 4: tempY -= 1; break;
			}

			if(tempX != 0 && Random.Range(0,101) <= 20) {
				tempX = 0;
				tempY = Random.Range(-1, 1);
				if(tempY == 0)
					tempY = 1;
			} else if(Random.Range(0,101) <= 20) {
				tempY = 0;
				tempX = Random.Range(-1, 1);
				if(tempX == 0)
					tempX = 1;
			}


			if(x + tempX > width - 1)
				tempX -= width;
			if(x + tempX < 0)
				tempX += width;
			if(y + tempY > height - 1)
				tempY -= height;
			if(y + tempY < 0)
				tempY += height;

			currentPoint = new Seed(x + tempX, y + tempY, 1);

			if(array[x + tempX, y + tempY] == 1) {
				if(!points.Contains(currentPoint)) {
					more = false;
				}
			} else {
				array[x + tempX, y + tempY] = 1;
			}

			points.Enqueue(currentPoint);

			x += tempX;
			y += tempY;
			tempX = tempY = 0;
			choice = 0;
			least = int.MaxValue;
		}
	}
}