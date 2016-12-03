using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    private string[] figures = { "Prefabs/CubeForm", "Prefabs/Iform", "Prefabs/L90form", "Prefabs/Lform",
        "Prefabs/Tform", "Prefabs/Z90form", "Prefabs/Zform"};
    
    public string getRandomFigure()
    {
        int randomNumber = Random.Range(0, 7);
        return figures[randomNumber];
    }
}
