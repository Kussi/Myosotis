using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameFieldCtrl {

    private static readonly string RegularFieldPrefix = "Field";
    private static readonly string StairFieldPrefix = "StairField";
    private static readonly string HomeFieldPrefix = "HomeField";
    private static readonly string GoalFieldPrefix = "GoalField";

    public static readonly int GoalFieldIndex = 999;

    public static readonly int NofRegularFields = 68;
    public static readonly int NofStairFieldsEachPlayer = 7;
    public static readonly int NofPlayers = 4;

    /// <summary>
    /// 0: HomeBench / 1: StairBench / 2: FirstStairStep / 3: HomeField
    /// </summary>
    public static readonly List<KeyValuePair<string, int[]>> PlayerFields = new List<KeyValuePair<string, int[]>>
    {
        new KeyValuePair<string, int[]>("Red", new int[] { 4, 67, 100, 110 }),
        new KeyValuePair<string, int[]>("Yellow", new int[] { 21, 16, 200, 210 }),
        new KeyValuePair<string, int[]>("Blue", new int[] { 38, 33, 300, 310 }),
        new KeyValuePair<string, int[]>("Green", new int[] { 55, 50, 400, 410 })
    };

    public static Dictionary<int, GameFieldBase> fields;

    public static RegularField GetHomeBench(Player player)
    {
        RegularField result = GetPlayerField(player, 0) as RegularField;
        if (result == null) throw new InvalidGameStateException();
        return result;
    }

    public static RegularField GetStairBench(Player player)
    {
        RegularField result = GetPlayerField(player, 1) as RegularField;
        if (result == null) throw new InvalidGameStateException();
        return result;
    }

    public static StairField GetFirstStairStep(Player player)
    {
        StairField result = GetPlayerField(player, 2) as StairField;
        if (result == null) throw new InvalidGameStateException();
        return result;
    }

    public static HomeField GetHomeField(Player player)
    {
        HomeField result = GetPlayerField(player, 3) as HomeField;
        if (result == null) throw new InvalidGameStateException();
        return result;
    }

    private static GameFieldBase GetPlayerField(Player player, int index)
    {
        GameFieldBase result;
        foreach (KeyValuePair<string, int[]> playerField in PlayerFields)
        {
            if (playerField.Key.Equals(player.Color))
            {
                fields.TryGetValue(playerField.Value[index], out result);
                return result;
            }
        }
        throw new InvalidGameStateException();
    }

    public static void InitializeGameFields()
    {
        fields = new Dictionary<int, GameFieldBase>();

        // initializing all Regularfields (= Bench and Regular Fields)
        for (int i = 0; i < NofRegularFields; ++i)
        {
            int index = i;
            GameObject field = GameObject.Find(RegularFieldPrefix + index.ToString("D3"));
            field.AddComponent<RegularField>();
            fields[index] = field.GetComponent<RegularField>();
        }

        // initializing all Stairfields (of all players)
        for (int i = 0; i < NofPlayers; ++i)
        {
            for (int j = 0; j < NofStairFieldsEachPlayer; ++j)
            {
                int index = 100 * (i + 1) + j;
                GameObject field = GameObject.Find(StairFieldPrefix + index.ToString("D3"));
                field.AddComponent<StairField>();
                fields[index] = field.GetComponent<StairField>();
            }
        }

        // initializing all Homefields
        for (int i = 0; i < NofPlayers; ++i)
        {
            int index = 100 * (i + 1) + 10;
            GameObject field = GameObject.Find(HomeFieldPrefix + index.ToString("D3"));
            field.AddComponent<HomeField>();
            fields[index] = field.GetComponent<HomeField>();
        }

        // initializing GoalField
        GameObject goal = GameObject.Find(GoalFieldPrefix + GoalFieldIndex.ToString("D3"));
        goal.AddComponent<GoalField>();
        fields[GoalFieldIndex] = goal.GetComponent<GoalField>(); 
    }
}