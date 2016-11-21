using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FindObjectOfType<Figures>().spawnNewFigure();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
