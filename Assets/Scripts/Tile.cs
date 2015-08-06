using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public SpriteRenderer rend;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!rend.isVisible)
			ObjectPool.instance.PoolObject(gameObject);
	}
}
