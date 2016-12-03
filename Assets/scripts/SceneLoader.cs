using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    private Gameplay game = new Gameplay();

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
