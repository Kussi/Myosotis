using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour
{
    private const int nofGameFields = 68;

    private const int redHomeBank = 5;
    private const int redStairBank = 68;
    private const int redFirstStairStep = 101;

    private const int yellowHomeBank = 22;
    private const int yellowStairBank = 17;
    private const int yellowFirstStairStep = 201;

    private const int blueHomeBank = 39;
    private const int blueStairBank = 34;
    private const int blueFirstStairStep = 301;

    private const int greenHomeBank = 56;
    private const int greenStairBank = 51;
    private const int greenFirstStairStep = 401;

    private static ArrayList gameFields;
    private Player[] player;

	// Use this for initialization
	void Start ()
    {
        gameFields = new ArrayList();

        // Adding all Gamefields (except of the stairs) to the list
        for (int i = 1; i <= nofGameFields; ++i)
        {
            gameFields.Add((GameField)GameObject.Find("Field" + i).GetComponent<MonoBehaviour>());
        }

        // Instanciating Players
        player = new Player[4];
        player[0] = new Player("red", redHomeBank, redStairBank, redFirstStairStep);
        player[1] = new Player("yellow", yellowHomeBank, yellowStairBank, yellowFirstStairStep);
        player[2] = new Player("blue", blueHomeBank, blueStairBank, blueFirstStairStep);
        player[3] = new Player("green", greenHomeBank, greenStairBank, greenFirstStairStep);

        GameLogic.Initialize(player, gameFields);
    }
}