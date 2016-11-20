using UnityEngine;
using System.Collections;

public class Figures : MonoBehaviour {

    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    public int gridWidth = 11;
    public int gridHeight = 18;

    public static Transform[,] grid = new Transform[20,30]; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        checkUserInput();
	}

    void checkUserInput ()
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
            fall = Time.time;
            if (!checkIfInsideGrid())
            {
                transform.position += new Vector3(0, 1, 0);
                enabled = false;
                spawnNewFigure();
            }
            else
                UpdateGrid();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                /*transform.Rotate(0, 0, 90);
                if (checkIfInsideGrid())
                {
                    UpdateGrid();
                }
                else if (!checkIfInsideGrid())
                {
                    transform.Rotate(0, 0, -90);
                }
                else if (limitRotation && transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -180);
                    UpdateGrid();
                }*/

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
            }
        }
    }

    public void UpdateGrid()
    {
        for (int y = -8; y < 9; y++)
        {
            for (int x = -5; x <= 5; x++)
            {
                if (grid[x+5,y+9] != null)
                {
                    if (grid[x+5, y+9].parent == transform)
                        grid[x+5, y+9] = null;
                }
            }
        }
        foreach (Transform figure in transform)
        {
            Vector2 position = figure.position;
            if (position.y < 9)
            {
                grid[(int)position.x+5, (int)position.y+9] = figure;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 position)
    {
        if (position.y > 9)
            return null;
        else
            return grid[(int)position.x+5, (int)position.y+9];
    }

    public bool checkIfInsideGrid()
    {
        foreach (Transform figure in transform)
        {
            Vector2 position = figure.position;
            if (!checkLimitation(position))
            {
                return false;
            }
            if (GetTransformAtGridPosition(position) !=null && 
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

    public void spawnNewFigure()
    {
        GameObject figure = (GameObject)Instantiate(Resources.Load(getRandomFigure(), typeof(GameObject)), 
            new Vector2(0, 9), Quaternion.identity);
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
}
