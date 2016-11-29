using UnityEngine;
using System.Collections;

public class ScoreTracker : MonoBehaviour {
    public delegate void RowDeletedHandler(object sender, RowDeletedArgs e);
    public ScoreTracker()
    {

    }
    public ScoreTracker(Figure figure)
    {
        figure.OnDeletedRow += new RowDeletedHandler(Figure_OnRowDeleted);
    }
    public void UpdateScore()
    {
        FindObjectOfType<Gameplay>().scoreText.text = FindObjectOfType<Gameplay>().Score.ToString();
    }
    public void SetScore()
    {
        FindObjectOfType<Gameplay>().Score += FindObjectOfType<Gameplay>().RowsScored * 100;
        FindObjectOfType<Gameplay>().scoreText = FindObjectOfType<Gameplay>().GetTextObjectByName("playerScore");
    }
    public void SetScore(int count)
    {
        FindObjectOfType<Gameplay>().Score += count * 100;
        FindObjectOfType<Gameplay>().scoreText = FindObjectOfType<Gameplay>().GetTextObjectByName("playerScore");
    }
    private void Figure_OnRowDeleted(object sender, RowDeletedArgs e)
    {
        SetScore(e.Count);
        UpdateScore();
    }
}