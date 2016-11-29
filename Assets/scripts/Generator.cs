using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
