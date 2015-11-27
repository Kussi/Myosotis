using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Initializer : MonoBehaviour
{
    private const int nofGameFields = 68;
    private const int nofPlayers = 4;
    private const int nofStairFieldsEachPlayer = 7;

    private const int redHomeBank = 4;
    private const int redStairBank = 67;
    private const int redFirstStairStep = 101;

    private const int yellowHomeBank = 21;
    private const int yellowStairBank = 16;
    private const int yellowFirstStairStep = 201;

    private const int blueHomeBank = 38;
    private const int blueStairBank = 33;
    private const int blueFirstStairStep = 301;

    private const int greenHomeBank = 55;
    private const int greenStairBank = 50;
    private const int greenFirstStairStep = 401;

    private static ArrayList gameFields;
    private static Dictionary<int, StairField> stairFields;
    private static GoalField goalField;
    private Player[] player;

	// Use this for initialization
	void Start ()
    {
        gameFields = new ArrayList();
        stairFields = new Dictionary<int, StairField>();

        // Adding all Gamefields (except of the stairs) to the list
        for (int i = 0; i < nofGameFields; ++i)
        {
            gameFields.Add((GameField)GameObject.Find("Field" + i).GetComponent<MonoBehaviour>());
        }

        // Adding all Stairfields to the list
        for (int i = 0; i < nofPlayers; ++i)
        {
            for (int j = 0; j < nofStairFieldsEachPlayer; ++j)
            {
                int index = 100 * (i + 1) + (j + 1);
                StairField field = (StairField)GameObject.Find("SF" + index).GetComponent<MonoBehaviour>();
                stairFields[index] = field;
            }
            
        }

        // Getting GoalField
        goalField = (GoalField)GameObject.Find("GoalField").GetComponent<GoalField>();

        // Instanciating Players
        player = new Player[4];
        player[0] = new Player("red", redHomeBank, redStairBank, redFirstStairStep);
        player[1] = new Player("yellow", yellowHomeBank, yellowStairBank, yellowFirstStairStep);
        player[2] = new Player("blue", blueHomeBank, blueStairBank, blueFirstStairStep);
        player[3] = new Player("green", greenHomeBank, greenStairBank, greenFirstStairStep);

        GameLogic.Initialize(player, gameFields, stairFields, goalField);
    }
}