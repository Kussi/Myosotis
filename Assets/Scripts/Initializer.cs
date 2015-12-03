using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Initializer : MonoBehaviour
{
    private const int NofRegularFields = 68;
    public const int NofStairFieldsEachPlayer = 7;

    private const int MinNofPlayers = 2;
    private const int MaxNofPlayers = 4;

    private const int RedHomeBank = 4;
    private const int RedStairBank = 67;
    private const int RedFirstStairStep = 100;

    private const int YellowHomeBank = 21;
    private const int YellowStairBank = 16;
    private const int YellowFirstStairStep = 200;

    private const int BlueHomeBank = 38;
    private const int BlueStairBank = 33;
    private const int BlueFirstStairStep = 300;

    private const int GreenHomeBank = 55;
    private const int GreenStairBank = 50;
    private const int GreenFirstStairStep = 400;

    public const int GoalFieldIndex = 1000;

    private static Dictionary<int, GameFieldBase> gameFields;
    private static Player[] players;
    private static int nofPlayers;

    // Use this for initialization
    void Start ()
    {
        SetNumberOfPlayers(4); // TODO Magic Number. Will be replaced with the menu call

        gameFields = new Dictionary<int, GameFieldBase>();

        // Adding all Regularfields (= Bench and Regular) to the list
        for (int i = 0; i < NofRegularFields; ++i)
        {
            gameFields[i] = (RegularField)GameObject.Find("Field" + i).GetComponent<MonoBehaviour>();
        }

        // Adding all Stairfields to the list
        for (int i = 0; i < nofPlayers; ++i)
        {
            for (int j = 0; j < NofStairFieldsEachPlayer; ++j)
            {
                int index = 100 * (i + 1) + j;
                gameFields[index] = (StairField)GameObject.Find("StairField" + index).GetComponent<MonoBehaviour>();
            }
        }

        // Adding GoalField to the list
        gameFields[GoalFieldIndex] = (GoalField)GameObject.Find("GoalField").GetComponent<GoalField>();

        // Instanciating Players
        players = new Player[nofPlayers];

        switch (nofPlayers)
        {
            case 1:
                goto case 2;
            case 2:
                players[0] = new Player("red", RedHomeBank, RedStairBank, RedFirstStairStep);
                players[1] = new Player("yellow", YellowHomeBank, YellowStairBank, YellowFirstStairStep);
                break;
            case 3:
                players[2] = new Player("blue", BlueHomeBank, BlueStairBank, BlueFirstStairStep);
                goto case 2;
            case 4:
                players[3] = new Player("green", GreenHomeBank, GreenStairBank, GreenFirstStairStep);
                goto case 3;
            default:
                Console.WriteLine("Default case");
                break;
        }

        GameLogic.Initialize(players, gameFields);
    }

    private static void SetNumberOfPlayers(int number)
    {
        if(number < MinNofPlayers || number > MaxNofPlayers)
        {
            throw new ArgumentException("there can only exist 2, 3 or 4 Player");
        }
        nofPlayers = number;
    }
}