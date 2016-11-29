using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Configuration;
using System;

public class Figure : MonoBehaviour
{

    public float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;

    public float count = 0.5f;
    //private int pointsForRow = Convert.ToBoolean(ConfigurationManager.AppSettings["PointsForRow"]);
    public static SceneLoader scene = new SceneLoader();

    //public Gameplay gameplay = new Gameplay();

    //logger instance
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

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkUserInput();
        checkInputWhenHolding();
    }

    void checkUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
                FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!FindObjectOfType<Gameplay>().checkIfInsideGrid(this))
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
                FindObjectOfType<Gameplay>().UpdateGrid(this, logger);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            moveFigureDown(this);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (FindObjectOfType<Gameplay>().FigurejustSpawned == false)
            {
                moveFigureDown(this);
            }
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
        else
            count = 0.5f;
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
            FindObjectOfType<Gameplay>().Score += FindObjectOfType<Gameplay>().RowsScored * 100;
            FindObjectOfType<Gameplay>().scoreText = FindObjectOfType<Gameplay>().GetTextObjectByName("playerScore");
            FindObjectOfType<Gameplay>().scoreText.text = FindObjectOfType<Gameplay>().Score.ToString();
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
}
