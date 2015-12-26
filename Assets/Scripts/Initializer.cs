using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Initializer : MonoBehaviour
{
    private const string RegularFieldPrefix = "Field";
    private const string StairFieldPrefix = "StairField";
    private const string HomeFieldPrefix = "HomeField";
    private const string GoalFieldPrefix = "GoalField";
    private const string Light = "Sun";

    public const int GoalFieldIndex = 999;

    private const int NofRegularFields = 68;
    public const int NofStairFieldsEachPlayer = 7;

    private const int MinNofPlayers = 2;
    private const int MaxNofPlayers = 4;

    private const int RedHomeBench = 4;
    private const int RedStairBench = 67;
    private const int RedFirstStairStep = 100;
    private const int RedHomeField = 110;
    private const int RedLightAngle = 90;

    private const int YellowHomeBench = 21;
    private const int YellowStairBench = 16;
    private const int YellowFirstStairStep = 200;
    private const int YellowHomeField = 210;
    private const int YellowLightAngle = 0;

    private const int BlueHomeBench = 38;
    private const int BlueStairBench = 33;
    private const int BlueFirstStairStep = 300;
    private const int BlueHomeField = 310;
    private const int BlueLightAngle = 270;

    private const int GreenHomeBench = 55;
    private const int GreenStairBench = 50;
    private const int GreenFirstStairStep = 400;
    private const int GreenHomeField = 410;
    private const int GreenLightAngle = 180;

    private static Dictionary<int, GameFieldBase> gameFields;
    private static Player[] players;
    private static int nofPlayers;

    // Use this for initialization
    void Start ()
    {

        GameObject.Find("Scene").AddComponent<BackgroundMusic>();
        

        nofPlayers = 4; // TODO Magic Number. Will be replaced with the menu call
        InitializeGameFields();
        InitializePlayers(nofPlayers);

        

        //string path = @"D:\Desktop\test.txt";
        //// Open the file to read from.
        //using (StreamReader sr = File.OpenText(path))
        //{
        //    string s = "";
        //    while ((s = sr.ReadLine()) != null)
        //    {
        //        Debug.Log(s);
        //    }
        //}

        GameLogic.Initialize(players, gameFields);
    }

    /// <summary>
    /// Initializing all Gamefields
    /// </summary>
    private static void InitializeGameFields()
    {
        gameFields = new Dictionary<int, GameFieldBase>();

        // initializing all Regularfields (= Bench and Regular Fields)
        for (int i = 0; i < NofRegularFields; ++i)
        {
            int index = i;
            GameObject field = GameObject.Find(RegularFieldPrefix + index.ToString("D3"));
            field.AddComponent<RegularField>();
            gameFields[index] = field.GetComponent<RegularField>();
        }

        // initializing all Stairfields (of all players)
        for (int i = 0; i < nofPlayers; ++i)
        {
            for (int j = 0; j < NofStairFieldsEachPlayer; ++j)
            {
                int index = 100 * (i + 1) + j;
                GameObject field = GameObject.Find(StairFieldPrefix + index.ToString("D3"));
                field.AddComponent<StairField>();
                gameFields[index] = field.GetComponent<StairField>();
            }
        }

        // initializing all Homefields
        for (int i = 0; i < nofPlayers; ++i)
        {
            int index = 100 * (i + 1) + 10;
            GameObject field = GameObject.Find(HomeFieldPrefix + index.ToString("D3"));
            field.AddComponent<HomeField>();
            gameFields[index] = field.GetComponent<HomeField>();
        }

        // initializing GoalField
        GameObject goal = GameObject.Find(GoalFieldPrefix + GoalFieldIndex.ToString("D3"));
        goal.AddComponent<GoalField>();
        gameFields[GoalFieldIndex] = goal.GetComponent<GoalField>();

        // initializing Light
        GameObject.Find(Light).AddComponent<Sun>();
    }

    /// <summary>
    /// Initializing the given number of Players
    /// </summary>
    /// <param name="NumberOfPlayers">number of players, that will participate the game</param>
    private static void InitializePlayers(int NumberOfPlayers)
    {
        if (NumberOfPlayers < MinNofPlayers || NumberOfPlayers > MaxNofPlayers)
        {
            throw new ArgumentException("there can only exist 2, 3 or 4 Player");
        }

        players = new Player[nofPlayers];

        switch (nofPlayers)
        {
            case 1:
                goto case 2;
            case 2:
                players[0] = new Player("red", RedHomeField, RedHomeBench, RedStairBench, RedFirstStairStep, RedLightAngle);
                players[1] = new Player("yellow", YellowHomeField, YellowHomeBench, YellowStairBench, YellowFirstStairStep, YellowLightAngle);
                break;
            case 3:
                players[2] = new Player("blue", BlueHomeField, BlueHomeBench, BlueStairBench, BlueFirstStairStep, BlueLightAngle);
                goto case 2;
            case 4:
                players[3] = new Player("green", GreenHomeField, GreenHomeBench, GreenStairBench, GreenFirstStairStep, GreenLightAngle);
                goto case 3;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }
}