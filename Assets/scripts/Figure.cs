using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Figure : MonoBehaviour
{
    public event ScoreTracker.RowDeletedHandler OnDeletedRow;
    public float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;

    public float count = 0.5f;

    public static SceneLoader scene = new SceneLoader();
    public ScoreTracker scoreTracker;

    public ILog logger = new FileLog();


    public float Fall
    {
        get
        {
            return fall;
        }

        set
        {
            fall = value;
        }
    }

    void Start()
    {
        scoreTracker = new ScoreTracker(this);
    }

    void Update()
    {
        checkUserInput();
        checkInputWhenHolding();
    }

    void checkUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveFigureRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveFigureLeft();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            moveFigureDown(this);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotateFigure();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (FindObjectOfType<Gameplay>().FigurejustSpawned == false)
            {
                moveFigureDown(this);
            }
        }
    }

    public void moveFigureDown(Figure figure)
    {
        transform.position += new Vector3(0, -1, 0);

        if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
        {
            transform.position += new Vector3(0, 1, 0);
            FindObjectOfType<Gameplay>().DeleteRowsIfNeeded();
            if (FindObjectOfType<Gameplay>().checkIfGameOver(this))
                scene.loadGameOver();
            OnDeletedRow(this, new RowDeletedArgs(FindObjectOfType<Gameplay>().RowsScored));
            enabled = false;
            FindObjectOfType<Gameplay>().spawnNewFigure();
            FindObjectOfType<Gameplay>().FigurejustSpawned = true;
        }
        else
        {
            FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
            FindObjectOfType<Gameplay>().FigurejustSpawned = false;
        }

        fall = Time.time;
    }

    public void moveFigureRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else
            FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
    }

    public void moveFigureLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
        if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else
            FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
    }

    public void rotateFigure()
    {
        if (allowRotation)
        {
            transform.Rotate(0, 0, 90f);
            if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
            {
                foreach (Transform figurePiece in transform)
                {
                    if (figurePiece.position.x >= 6)
                    {
                        transform.position += new Vector3(-1, 0, 0);
                    }
                    else if (figurePiece.position.x <= -6)
                    {
                        transform.position += new Vector3(1, 0, 0);
                    }
                }
                if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
                    transform.Rotate(0, 0, -90f);
            }
            else
                FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
        }
    }

    public void checkInputWhenHolding()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            count -= Time.deltaTime;
            if (count < 0)
            {
                moveFigureDown(this);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            count -= Time.deltaTime;
            if (count < 0)
            {
                moveFigureRight();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            count -= Time.deltaTime;
            if (count < 0)
            {
                moveFigureLeft();
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            count -= Time.deltaTime;
            if (count < 0)
            {
                rotateFigure();
            }
        }
        else
            count = 0.5f;
    }
}
