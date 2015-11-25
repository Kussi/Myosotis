using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private string color;
    private GameFigure[] gameFigures;

    private Dice dice;
    private Vector3 dicePosition;


    public Player(string color)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);
        gameFigures = new GameFigure[4];

        for (int i = 0; i < 4; ++i)
        {
            gameFigures[i] = new GameFigure(GameObject.CreatePrimitive(PrimitiveType.Capsule));
            //gameFigures[i] = new GameFigure((GameObject)Instantiate(Resources.Load("Prefabs/dice_1x1", typeof(GameObject))));

            Vector3 spawnPosition = GameObject.Find(this.color + "SpawnPoint" + (i + 1)).transform.position;
            gameFigures[i].SetPosition(spawnPosition);
            gameFigures[i].SetColor(this.color);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override string ToString()
    {
        return color;
    }

    public void SetActive(bool isActive)
    {
        dice.SetActive(true);
    }
}
