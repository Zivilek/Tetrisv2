using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Figures : MonoBehaviour
{

    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public static bool figurejustSpawned = false;

    public static Transform[,] grid = new Transform[13, 19];

    public static GameObject previewFigure;
    public static GameObject nextFigure;

    public Vector2 previewFigurePosition = new Vector2(10, 0);

    public static bool gameStarted = false;

    public static int rowsScored;
    public static int score = 0;
    public Text scoreText;
    // Use this for initialization
    void Start()
    {
        //spawnNewFigure();
    }

    // Update is called once per frame
    void Update()
    {

        checkUserInput();
    }

    void checkUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!checkIfInsideGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
                UpdateGrid();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!checkIfInsideGrid())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
                UpdateGrid();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            moveFigureDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                transform.Rotate(0, 0, 90f);
                if (!checkIfInsideGrid())
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                    UpdateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (figurejustSpawned == false)
            {
                moveFigureDown();
            }
        }
    }

    public void moveFigureDown()
    {
        transform.position += new Vector3(0, -1, 0);

        if (!checkIfInsideGrid())
        {
            transform.position += new Vector3(0, 1, 0);
            DeleteRowsIfNeeded();
            if (checkIfGameOver())
                loadGameOver();
            score += rowsScored * 100;
            scoreText = GetTextObjectByName("playerScore");
            scoreText.text = score.ToString();
            enabled = false;
            spawnNewFigure();
            figurejustSpawned = true;
        }
        else
        {
            UpdateGrid();
            figurejustSpawned = false;
        }

        fall = Time.time;
    }

    public void UpdateGrid()
    {
        for (int y = -8; y < 9; ++y)
        {
            for (int x = -5; x < 6; ++x)
            {
                if (grid[x + 5, y + 8] != null)
                {
                    if (grid[x + 5, y + 8].parent == transform)
                        grid[x + 5, y + 8] = null;
                }
            }
        }
        foreach (Transform figure in transform)
        {
            Vector2 position = Round(figure.position);
            if (position.y < 9)
            {
                grid[(int)(position.x) + 5, (int)(position.y) + 8] = figure;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 position)
    {
        if (position.y > 8)
            return null;
        else
            return grid[(int)(position.x) + 5, (int)(position.y) + 8];
    }

    public bool checkIfInsideGrid()
    {
        foreach (Transform figure in transform)
        {
            Vector2 position = Round(figure.position);
            if (!checkLimitation(position))
            {
                return false;
            }
            if (GetTransformAtGridPosition(position) != null &&
                GetTransformAtGridPosition(position).parent != transform)
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

    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }


    public Text GetTextObjectByName(string name)
    {
        var canvas = GameObject.Find("Canvas");
        var texts = canvas.GetComponentsInChildren<Text>();
        return texts.FirstOrDefault(textObject => textObject.name == name);
    }

    public void spawnNewFigure()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            nextFigure = (GameObject)Instantiate(Resources.Load(getRandomFigure(), typeof(GameObject)), 
                new Vector2(0, 8), Quaternion.identity);
            previewFigure = (GameObject)Instantiate(Resources.Load(getRandomFigure(), typeof(GameObject)),
                previewFigurePosition, Quaternion.identity);
            previewFigure.GetComponent<Figures>().enabled = false;
        }
        else
        {
            previewFigure.GetComponent<Figures>().transform.localPosition = new Vector2(0, 8);
            nextFigure = previewFigure;
            nextFigure.GetComponent<Figures>().enabled = true;
            previewFigure = (GameObject)Instantiate(Resources.Load(getRandomFigure(), typeof(GameObject)),
               previewFigurePosition, Quaternion.identity);
            previewFigure.GetComponent<Figures>().enabled = false;
        }
    }

    public string getRandomFigure()
    {
        string randomFigureName = "CubeForm";
        int randomNumber = Random.Range(1, 8);

        switch (randomNumber)
        {
            case 1:
                randomFigureName = "Prefabs/CubeForm";
                break;
            case 2:
                randomFigureName = "Prefabs/Iform";
                break;
            case 3:
                randomFigureName = "Prefabs/L90form";
                break;
            case 4:
                randomFigureName = "Prefabs/Lform";
                break;
            case 5:
                randomFigureName = "Prefabs/Tform";
                break;
            case 6:
                randomFigureName = "Prefabs/Z90form";
                break;
            case 7:
                randomFigureName = "Prefabs/Zform";
                break;
        }
        return randomFigureName;
    }

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
        rowsScored = 0;
        for (int y = -8; y < 10; y++)
        {
            if (IsFullRowAt(y))
            {
                DeleteRowAt(y);
                MoveAllRowsDown(y + 1);
                y--;
                rowsScored++;
            }
        }
    }

    public bool checkIfGameOver()
    {
        for (int x = -5; x < 6; x++)
        {
            foreach (Transform piece in transform)
            {
                Vector2 position = Round(piece.position);
                if (position.y > 9)
                    return true;
            }
        }
        return false;
    }

    public void loadGameOver()
    {  
        Application.LoadLevel("GameOver");
    }

    public void playAgain()
    {
        gameStarted = false;
#pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel("Tetris");
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
