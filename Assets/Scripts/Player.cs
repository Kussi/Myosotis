using UnityEngine;
using System.Collections;

public class Player {

    private const int nofGameFigures = 4; 

    private string color;
    private Dice dice;
    private GameObject[] gameFigures;
    private AbstractState state;
    private int homeBank;
    private int stairBank;
    private int firstStairStep;

    public string Color
    {
        get { return color; }
    }
    public int HomeBank
    {
        get { return homeBank; }
    }
    public int StairBank
    {
        get { return stairBank; }
    }
    public int FirstStairStep
    {
        get { return firstStairStep; }
    }

    public Player(string color, int homeBank, int stairBank, int firstStairStep)
    {
        this.color = char.ToUpper(color[0]) + color.Substring(1);
        this.homeBank = homeBank;
        this.stairBank = stairBank;
        this.firstStairStep = firstStairStep;
        this.state = new StateAllAtHome();

        // instantiate all gameFigures 
        gameFigures = new GameObject[4];
        for (int i = 0; i < nofGameFigures; ++i)
        {
         
            gameFigures[i] = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //gameFigures[i] = new GameFigure((GameObject)Instantiate(Resources.Load("Prefabs/dice_1x1", typeof(GameObject))));
            gameFigures[i].name = (this.color + "GameFigure" + (i+1));

            Vector3 spawnPosition = GameObject.Find(this.color + "SpawnPoint" + (i+1)).transform.position;
            gameFigures[i].transform.position = spawnPosition;

            Material material = (Material)Resources.Load("Materials/" + color + "Player", typeof(Material));
            gameFigures[i].GetComponent<Renderer>().material = material;
        }

        GameObject dice = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Dice"));
        dice.name = (this.color + "Dice");
        Vector3 dicePosition = GameObject.Find(this.color + "DicePosition").transform.position;
        dice.transform.position = dicePosition;
        dice.tag = this.color;

        this.dice = (Dice)dice.GetComponent<MonoBehaviour>();
        this.dice.Parent = this;
	}

    public void Play()
    {
        Debug.Log(this.color + " begins to play.");
        dice.SetActive(true);
    }
}
