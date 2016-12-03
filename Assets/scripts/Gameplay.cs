using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class Gameplay : MonoBehaviour {

    public Transform[,] grid = new Transform[15, 20];

    private readonly int gridBottom = -8;
    private readonly int gridHeight = 8;
    private readonly int gridLeft = -5;
    private readonly int gridRight = 5;

    public static GameObject previewFigure;
    public static GameObject nextFigure;

    public Vector2 previewFigurePosition = new Vector2(10, 0);
    public Vector2 newFigurePosition = new Vector2(0, 12);

    public static bool gameStarted = false;

    public static int rowsScored;
    public static int score = 0;
    public Text scoreText { get; set; }

    public static bool figurejustSpawned = false;

    public static Generator generator = new Generator();
    public ScoreTracker scoreTracker;

    private delegate void Del(ref Vector2 position);

    Del round = delegate (ref Vector2 position)
    {
        position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    };

    public bool FigurejustSpawned
    {
        get
        {
            return figurejustSpawned;
        }

        set
        {
            figurejustSpawned = value;
        }
    }

    public bool GameStarted
    {
        get
        {
            return gameStarted;
        }

        set
        {
            gameStarted = value;
        }
    }

    public int RowsScored
    {
        get
        {
            return rowsScored;
        }

        set
        {
            rowsScored = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    void Start () {
        scoreTracker = new ScoreTracker();
        FindObjectOfType<Gameplay>().scoreText = FindObjectOfType<Gameplay>().GetTextObjectByName("playerScore");
        try
        {
            if (scoreText.text == string.Empty) throw new ScoreNotSetException();
        }
        catch
        {
            scoreTracker.UpdateScore();
        }
        spawnNewFigure();
	}
	
	void Update () {
	
	}
    public void UpdateGrid(Figure figure, ILog logger)
    {
        for (int y = gridBottom; y <= gridHeight; ++y)
        {
            for (int x = gridLeft; x <= gridRight; ++x)
            {
                try
                {
                    if (grid[x + gridRight, y + gridHeight].parent == figure.transform)
                        grid[x + gridRight, y + gridHeight] = null;
                }
                catch (System.NullReferenceException ex)
                {
                    Thread thread = new Thread(new ThreadStart(delegate ()
                    {
                        logger.Log(ex.Message);
                    }));
                }
            }
        }
        foreach (Transform tetrisFigure in figure.transform)
        {
            Vector2 position = tetrisFigure.position;
            round(ref position);
            if (position.y < gridHeight+1)
            {
                grid[(int)(position.x) + gridRight, (int)(position.y) + gridHeight] = tetrisFigure;
            }
        }
    }

    public bool IsFullRowAt(int y)
    {
        for (int x = gridLeft; x <= gridRight; x++)
        {
            if (grid[x + gridRight, y + gridHeight] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DeleteRowAt(int y)
    {
        for (int x = gridLeft; x <= gridRight; x++)
        {
            Destroy(grid[x + gridRight, y + gridHeight].gameObject);
            grid[x + gridRight, y + gridHeight] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = gridLeft; x <= gridRight; x++)
        {
            if (grid[x + gridRight, y + gridHeight] != null)
            {
                grid[x + gridRight, y + gridHeight - 1] = grid[x + gridRight, y + gridHeight];
                grid[x + gridRight, y + gridHeight] = null;
                grid[x + gridRight, y + gridHeight - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i <= gridHeight; i++)
            MoveRowDown(i);
    }

    public void DeleteRowsIfNeeded()
    {
        RowsScored = 0;
        for (int y = gridBottom; y < gridHeight+2; y++)
        {
            if (IsFullRowAt(y))
            {
                DeleteRowAt(y);
                MoveAllRowsDown(y + 1);
                y--;
                RowsScored++;
            }
        }
    }

    public bool checkIfGameOver(Figure figure)
    {
        for (int x = gridLeft; x <= gridRight; x++)
        {
            foreach (Transform piece in figure.transform)
            {
                Vector2 position = piece.position;
                round(ref position);
                if (position.y > gridHeight+1)
                    return true;
            }
        }
        return false;
    }

    public void spawnNewFigure()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            nextFigure = (GameObject)Instantiate(Resources.Load(generator.getRandomFigure(), typeof(GameObject)),
                new Vector2(0, 12), Quaternion.identity);
            previewFigure = (GameObject)Instantiate(Resources.Load(generator.getRandomFigure(), typeof(GameObject)),
                previewFigurePosition, Quaternion.identity);
            previewFigure.GetComponent<Figure>().enabled = false;
        }
        else
        {
            previewFigure.transform.localPosition = new Vector2(0, 12);
            nextFigure = previewFigure;
            nextFigure.GetComponent<Figure>().enabled = true;
            previewFigure = (GameObject)Instantiate(Resources.Load(generator.getRandomFigure(), typeof(GameObject)),
               previewFigurePosition, Quaternion.identity);
            previewFigure.GetComponent<Figure>().enabled = false;
        }
    }

    public bool checkIfInsideGrid(Figure figure)
    {
        foreach (Transform tetrisFigure in figure.transform)
        {
            Vector2 position = tetrisFigure.position;
            round(ref position);
            if (!FindObjectOfType<Gameplay>().checkLimitation(position))
            {
                return false;
            }
            if (FindObjectOfType<Gameplay>().GetTransformAtGridPosition(position) != null &&
                FindObjectOfType<Gameplay>().GetTransformAtGridPosition(position).parent != figure.transform)
            {
                return false;
            }
        }
        return true;
    }

    public bool checkLimitation(Vector2 position)
    {
        return (position.x < gridRight+1 && position.x > gridLeft-1 && position.y > gridBottom-1);
    }

    public Text GetTextObjectByName(string name)
    {
        var canvas = GameObject.Find("Canvas");
        var texts = canvas.GetComponentsInChildren<Text>();
        return texts.FirstOrDefault(textObject => textObject.name == name); //lambda
    }

    public Transform GetTransformAtGridPosition(Vector2 position)
    {
        if (position.y > gridHeight)
            return null;
        else
            return grid[(int)(position.x) + gridRight, (int)(position.y) + gridHeight];
    }
}
