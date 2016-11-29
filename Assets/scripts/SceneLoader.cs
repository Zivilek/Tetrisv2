using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadGameOver()
    {
        Application.LoadLevel("GameOver");
    }

    public void playAgain()
    {
        FindObjectOfType<Gameplay>().GameStarted = false;
#pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel("Tetris");
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
