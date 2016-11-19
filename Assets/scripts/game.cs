using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public int gridWidth = 11;
    public int gridHeight = 18;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool checkIfInsideGrid(Vector2 position)
    {
        return ((int)position.x >= -6 && (int)position.x < 6 && (int)position.y >= -9);
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
