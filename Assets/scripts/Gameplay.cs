using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class Gameplay : MonoBehaviour {

    public Transform[,] grid = new Transform[15, 20];

    public static GameObject previewFigure;
    public static GameObject nextFigure;

    public Vector2 previewFigurePosition = new Vector2(10, 0);
    public static bool gameStarted = false;

    public static int rowsScored;
    public static int score = 0;
    public Text scoreText { get; set; }

    public static bool figurejustSpawned = false;

    public static Generator generator = new Generator();
    //delegate
    private delegate void Del(ref Vector2 position);
    //anonymous method
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

    // Use this for initialization
    void Start () {
        spawnNewFigure();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //Dependency Injection
    public void UpdateGrid(Figure figure, ILog logger)
    {
        for (int y = -8; y < 9; ++y)
        {
            for (int x = -5; x < 6; ++x)
            {
                try
                {
                    if (grid[x + 5, y + 8].parent == figure.transform)
                        grid[x + 5, y + 8] = null;
                }
                catch (System.NullReferenceException ex)
                {
                    //anonymous method
                    Thread thread = new Thread(delegate ()
                    {
                        logger.Log(ex.Message);
                    }); 
                }
                /*
                if (grid[x + 5, y + 8] != null)
                {
                    if (grid[x + 5, y + 8].parent == figure.transform)
                        grid[x + 5, y + 8] = null;
                }*/
            }
        }
        foreach (Transform tetrisFigure in figure.transform)
        {
            //original
            //Vector2 position = Round(tetrisFigure.position);
            //anoniminis metodas
            Vector2 position = tetrisFigure.position;
            round(ref position);
            if (position.y < 9)
            {
                grid[(int)(position.x) + 5, (int)(position.y) + 8] = tetrisFigure;
            }
        }
    }
    //unusable
    /*
    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
    */
    public bool IsFullRowAt(int y)
    {
        for (int x = -5; x < 6; x++)
        {
            if (grid[x + 5, y + 8] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DeleteRowAt(int y)
    {
        for (int x = -5; x < 6; x++)
        {
            Destroy(grid[x + 5, y + 8].gameObject);
            grid[x + 5, y + 8] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = -5; x < 6; x++)
        {
            if (grid[x + 5, y + 8] != null)
            {
                grid[x + 5, y + 8 - 1] = grid[x + 5, y + 8];
                grid[x + 5, y + 8] = null;
                grid[x + 5, y + 8 - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < 6; i++)
            MoveRowDown(i);
    }

    public void DeleteRowsIfNeeded()
    {
        RowsScored = 0;
        for (int y = -8; y < 10; y++)
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
        for (int x = -5; x < 6; x++)
        {
            foreach (Transform piece in figure.transform)
            {
                //Vector2 position = Round(piece.position);
                Vector2 position = piece.position;
                round(ref position);
                if (position.y > 9)
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
                new Vector2(0, 8), Quaternion.identity);
            previewFigure = (GameObject)Instantiate(Resources.Load(generator.getRandomFigure(), typeof(GameObject)),
                previewFigurePosition, Quaternion.identity);
            previewFigure.GetComponent<Figure>().enabled = false;
        }
        else
        {
            previewFigure.transform.localPosition = new Vector2(0, 8);
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
            //Vector2 position = FindObjectOfType<Gameplay>().Round(tetrisFigure.position); old
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
        return (position.x < 6 && position.x > -6 && position.y > -9);
    }

    public Text GetTextObjectByName(string name)
    {
        var canvas = GameObject.Find("Canvas");
        var texts = canvas.GetComponentsInChildren<Text>();
        return texts.FirstOrDefault(textObject => textObject.name == name); //lambda
    }

    public Transform GetTransformAtGridPosition(Vector2 position)
    {
        if (position.y > 8)
            return null;
        else
            return grid[(int)(position.x) + 5, (int)(position.y) + 8];
    }
}
