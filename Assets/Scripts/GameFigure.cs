using UnityEngine;
using System.Collections;

public class GameFigure {

    GameObject figure;

    public GameFigure(GameObject figure)
    {
        this.figure = figure;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPosition(Vector3 position)
    {
        figure.transform.position = position;
    }

    public void SetColor(string color)
    {
        Material material = (Material)Resources.Load("Materials/" + color + "Player", typeof(Material));
        figure.GetComponent<Renderer>().material = material;

    }

}
