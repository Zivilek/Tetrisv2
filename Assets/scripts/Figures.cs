using UnityEngine;
using System.Collections;

public class Figures : MonoBehaviour
{

    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    //public bool limitRotation = false;

    public int gridWidth = 11;
    public int gridHeight = 18;

    public static Transform[,] grid = new Transform[20, 30];

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
            transform.position += new Vector3(0, -1, 0);

            if (!checkIfInsideGrid())
            {
                transform.position += new Vector3(0, 1, 0);
                DeleteRowsIfNeeded();
                if (checkIfGameOver())
                    loadGameOver();
                enabled = false;
                spawnNewFigure();
            }
            else
                UpdateGrid();
            fall = Time.time;
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


                /*if (limitRotation && transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -180);
                    if (checkIfInsideGrid())
                    {
                        UpdateGrid();
                    }
                    else
                        transform.Rotate(0, 0, -90);
                }
                else if (checkIfInsideGrid())
                {
                    UpdateGrid();
                }
                else if (!checkIfInsideGrid())
                {
                    transform.Rotate(0, 0, -90);
                }*/

                /*if (limitRotation)
                {
                    if(transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                if (checkIfInsideGrid())
                {
                    UpdateGrid();
                }
                else
                {
                    if (limitRotation)
                    {
                        if(transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }*/
            }
        }
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

    public void spawnNewFigure()
    {
        GameObject figure = (GameObject)Instantiate(Resources.Load(getRandomFigure(), typeof(GameObject)),
            new Vector2(0, 8), Quaternion.identity);
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
        for (int y = -8; y < 10; y++)
        {
            if (IsFullRowAt(y))
            {
                DeleteRowAt(y);
                MoveAllRowsDown(y + 1);
                y--;
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
                if (position.y > 6)
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
        Application.LoadLevel("Tetris");
    }
}
