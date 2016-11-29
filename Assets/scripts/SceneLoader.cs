using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    private Gameplay game = new Gameplay();
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
        game.GameStarted = false;
        Application.LoadLevel("Tetris");
    }
}
