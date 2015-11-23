using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        InsertBoard();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void InsertBoard()
    {
        Application.LoadLevel(1);
        Application.LoadLevelAdditive("GameBoard");
    }
}
