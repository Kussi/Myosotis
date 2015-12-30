using UnityEngine;
using System.Collections.Generic;
using System;

public static class FieldCtrl
{

    private static readonly string RegularFieldPrefix = "Field";
    private static readonly string StairFieldPrefix = "StairField";
    private static readonly string HomeFieldPrefix = "HomeField";
    private static readonly string GoalFieldPrefix = "GoalField";

    public static readonly int GoalFieldIndex = 999;

    public static readonly int NofRegularFields = 68;
    public static readonly int NofStairFieldsEachPlayer = 7;
    public static readonly int NofPlayers = 4;

    public static readonly bool SendHomeEnabled = true;

    /// <summary>
    /// 0: HomeBench / 1: StairBench / 2: FirstStairStep / 3: HomeField
    /// </summary>
    private static readonly Dictionary<string, int[]> PlayerFields = new Dictionary<string, int[]>
    {
        { "Red", new int[] { 4, 67, 100, 110 } },
        { "Yellow", new int[] { 21, 16, 200, 210 } },
        { "Blue", new int[] { 38, 33, 300, 310 } },
        { "Green", new int[] { 55, 50, 400, 410 } }
    };

    private static Dictionary<int, GameFieldBase> fields;

    public static bool IsHomeField(int fieldIndex)
    {
        HomeField field = fields[fieldIndex] as HomeField;
        if (field == null) return false;
        return true;
    }

    public static bool IsGoalField(int fieldIndex)
    {
        GoalField field = fields[fieldIndex] as GoalField;
        if (field == null) return false;
        return true;
    }

    public static bool IsStairBench(int fieldIndex, GameFigure figure)
    {
        Player player = GameFigureCtrl.GetPlayer(figure);
        int[] playerField;
        PlayerFields.TryGetValue(player.Color, out playerField);
        if (playerField == null) throw new InvalidGameStateException();
        return playerField[1] == fieldIndex;
    }

    public static int GetHomeBench(GameFigure figure)
    {
        return GetPlayerField(figure, 0);
    }

    public static int GetFirstStairStep(GameFigure figure)
    {
        return GetPlayerField(figure, 2);
    }

    public static int GetHomeField(GameFigure figure)
    {
        return GetPlayerField(figure, 3);
    }

    private static int GetPlayerField(GameFigure figure, int index)
    {
        Player player = GameFigureCtrl.GetPlayer(figure);
        int[] result;
        if (!PlayerFields.TryGetValue(player.Color, out result))
            throw new InvalidGameStateException();
        return result[index];
    }

    public static bool PlaceFigureOnHomeField(GameFigure figure)
    {
        return PlaceFigure(figure, GetHomeField(figure));
    }

    public static bool PlaceFigureOnHomeBench(GameFigure figure)
    {
        return PlaceFigure(figure, GetHomeBench(figure));
    }

    public static bool PlaceFigure(GameFigure figure, int fieldIndex)
    {
        GameFieldBase field;
        fields.TryGetValue(fieldIndex, out field);
        if (field == null) throw new InvalidGameStateException();

        RegularField regularField = field as RegularField;
        if(regularField != null)
        {
            if(regularField.IsBarrier)
            {
                return false;
            }
            else
            {
                if()
                regularField.PlaceFigure(figure);
            }
        }

        // if stairfield -> go
        // if goalfield -> go
        // if regularfield -> check barrier -> check others -> bendfield


        // check if field is available

        return fields[fieldIndex].PlaceFigure(figure);
    }

    public static void InitializeFields()
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

    //public static RegularField GetHomeBench(Player player)
    //{
    //    RegularField result = GetPlayerField(player, 0) as RegularField;
    //    if (result == null) throw new InvalidGameStateException();
    //    return result;
    //}

    //public static RegularField GetStairBench(Player player)
    //{
    //    RegularField result = GetPlayerField(player, 1) as RegularField;
    //    if (result == null) throw new InvalidGameStateException();
    //    return result;
    //}

    //public static StairField GetFirstStairStep(Player player)
    //{
    //    StairField result = GetPlayerField(player, 2) as StairField;
    //    if (result == null) throw new InvalidGameStateException();
    //    return result;
    //}

    //public static HomeField GetHomeField(Player player)
    //{
    //    HomeField result = GetPlayerField(player, 3) as HomeField;
    //    if (result == null) throw new InvalidGameStateException();
    //    return result;
    //}

    //private static GameFieldBase GetPlayerField(Player player, int index)
    //{
    //    GameFieldBase result;
    //    int[] fieldIndexes;
    //    if (!PlayerFields.TryGetValue(player.Color, out fieldIndexes))
    //        throw new InvalidGameStateException();
    //    if (!fields.TryGetValue(fieldIndexes[index], out result))
    //        throw new InvalidGameStateException();
    //    return result;
    //}
}