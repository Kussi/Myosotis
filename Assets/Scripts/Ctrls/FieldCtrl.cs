using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

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

    private static bool IsLastRegularFieldIndex(int fieldIndex)
    {
        return fieldIndex == NofRegularFields - 1;
    }

    public static bool IsBarrier(int fieldIndex)
    {
        RegularField field = fields[fieldIndex] as RegularField;
        if (field == null) throw new InvalidGameStateException();
        return field.IsBarrier;
    }

    public static bool IsHomeField(int fieldIndex, GameFigure figure)
    {
        Player player = GameFigureCtrl.GetPlayer(figure);
        int[] playerField;
        PlayerFields.TryGetValue(player.Color, out playerField);
        if (playerField == null) throw new InvalidGameStateException();
        return playerField[3] == fieldIndex;
    }

    public static bool IsRegularField(int fieldIndex)
    {
        RegularField field = fields[fieldIndex] as RegularField;
        if (field == null) return false;
        return true;
    }

    public static bool IsStairField(int fieldIndex)
    {
        StairField field = fields[fieldIndex] as StairField;
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

    public static bool IsLastStairStep(int fieldIndex, GameFigure figure)
    {
        return fieldIndex == GetLastStairStep(figure);
    }

    public static bool IsGoalField(int fieldIndex)
    {
        GoalField field = fields[fieldIndex] as GoalField;
        if (field == null) return false;
        return true;
    }

    public static int GetNextRegularFieldIndex(int actualFieldIndex)
    {
        if (actualFieldIndex < 0 || actualFieldIndex >= NofRegularFields)
            throw new ArgumentOutOfRangeException();
        int nextFieldIndex = actualFieldIndex + 1;
        return nextFieldIndex == NofRegularFields - 1 ? 0 : nextFieldIndex;
    }

    public static int GetNextStairFieldIndex(GameFigure figure, int actualIndex, bool hasToGoBackwards)
    {
        int nextFieldIndex;

        if (actualIndex < GetFirstStairStep(figure) || actualIndex > GetLastStairStep(figure))
            throw new InvalidGameStateException();
        if (hasToGoBackwards) nextFieldIndex = actualIndex - 1;
        else nextFieldIndex = actualIndex + 1;

        return nextFieldIndex;
    }

    private static GameFigure GetFigureToSendHome(RegularField field, GameFigure arrivingFigure)
    {
        return field.GetFigureToSendHome(arrivingFigure);
    }

    private static int GetHomeBench(GameFigure figure)
    {
        return GetPlayerField(figure, 0);
    }

    private static int GetFirstStairStep(GameFigure figure)
    {
        return GetPlayerField(figure, 2);
    }

    private static int GetLastStairStep(GameFigure figure)
    {
        return GetFirstStairStep(figure) + NofStairFieldsEachPlayer - 1;
    }

    private static int GetHomeField(GameFigure figure)
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

    public static bool PlaceFigureOnHomeBench(GameFigure figure)
    {
        if (!IsHomeField(figure.Field, figure))
            throw new InvalidGameStateException();
        int newFieldIndex = GetHomeBench(figure);
        if (IsBarrier(newFieldIndex)) return false;
        MoveFigureRegular(figure, newFieldIndex);
        return true;
    }

    public static GameFigure PlaceFigureOnNextRegularField(GameFigure figure)
    {
        if (!IsRegularField(figure.Field))
            throw new InvalidGameStateException();
        int newFieldIndex = GetNextRegularFieldIndex(figure.Field);
        if (!IsRegularField(newFieldIndex)) throw new InvalidGameStateException();
        if (IsBarrier(newFieldIndex)) throw new InvalidGameStateException();
        MoveFigureRegular(figure, newFieldIndex);
        return ((RegularField)fields[newFieldIndex]).GetFigureToSendHome(figure);
    }

    public static void PlaceFigureOnFirstStairStep(GameFigure figure)
    {
        if (!IsStairBench(figure.Field, figure))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetFirstStairStep(figure));
    }

    public static void PlaceFigureOnNextRegularStairStep(GameFigure figure, bool hasToGoBackwards)
    {
        if (!IsStairField(figure.Field))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetNextStairFieldIndex(figure, figure.Field, hasToGoBackwards));
    }

    public static void PlaceFigureInGoal(GameFigure figure)
    {
        if (!IsLastStairStep(figure.Field, figure))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GoalFieldIndex);
    }

    public static void PlaceFigureOnLastStairStep(GameFigure figure)
    {
        if (!IsGoalField(figure.Field))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetLastStairStep(figure));
    }

    public static void PlaceFigureOnHomeField(GameFigure figure)
    {
        MoveFigureHome(figure);
    }

    private static void MoveFigureRegular(GameFigure figure, int fieldIndex)
    {
        fields[figure.Field].RemoveFigure(figure);
        fields[fieldIndex].PlaceFigure(figure);
    }

    public static void MoveFigureHome(GameFigure figure)
    {
        if (!IsRegularField(figure.Field) || ((RegularField)fields[figure.Field]).IsBarrier)
            throw new InvalidGameStateException();
        fields[figure.Field].RemoveFigure(figure);
        fields[GetHomeField(figure)].PlaceFigure(figure);
    }

    public static void InitiallyPlaceFigure(GameFigure figure)
    {
        if (GameCtrl.GameIsRunning)
            throw new InvalidGameStateException();
        HomeField field = fields[GetHomeField(figure)] as HomeField;
        if (field == null) throw new InvalidGameStateException();
        field.InitiallyPlaceFigure(figure);
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
            fields[index].Index = index;
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
                fields[index].Index = index;
            }
        }

        // initializing all Homefields
        for (int i = 0; i < NofPlayers; ++i)
        {
            int index = 100 * (i + 1) + 10;
            GameObject field = GameObject.Find(HomeFieldPrefix + index.ToString("D3"));
            field.AddComponent<HomeField>();
            fields[index] = field.GetComponent<HomeField>();
            fields[index].Index = index;
        }

        // initializing GoalField
        GameObject goal = GameObject.Find(GoalFieldPrefix + GoalFieldIndex.ToString("D3"));
        goal.AddComponent<GoalField>();
        fields[GoalFieldIndex] = goal.GetComponent<GoalField>();
        fields[GoalFieldIndex].Index = GoalFieldIndex;
    }
}