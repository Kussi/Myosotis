using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;

public static class FieldCtrl
{
    private static readonly string RegularFieldPrefix = "Field";
    private static readonly string StairFieldPrefix = "StairField";
    private static readonly string HomeFieldPrefix = "HomeField";
    private static readonly string GoalFieldPrefix = "GoalField";

    private static readonly string TexturesDirectory = "Materials/";
    private static readonly string RedTriggerRegularMaterial = "RedTriggerRegular";
    private static readonly string RedTriggerBendMaterial = "RedTriggerBend";
    private static readonly string YellowTriggerRegularMaterial = "YellowTriggerRegular";
    private static readonly string YellowTriggerBendMaterial = "YellowTriggerBend";
    private static readonly string BlueTriggerRegularMaterial = "BlueTriggerRegular";
    private static readonly string BlueTriggerBendMaterial = "BlueTriggerBend";
    private static readonly string GreenTriggerRegularMaterial = "GreenTriggerRegular";
    private static readonly string GreenTriggerBendMaterial = "GreenTriggerBend";
    private static readonly string RainbowTriggerRegularMaterial = "RainbowTriggerRegular";
    private static readonly string RainbowTriggerBendMaterial = "RainbowTriggerBend";
    private static readonly string BendMaterial = "BendField";
    private static readonly string BenchMaterial = "BenchField";
    private static readonly string RegularMaterial = "regularField";

    private static readonly ArrayList SingleEventTriggeredFields = new ArrayList() { 2, 13, 19, 30, 36, 47, 53, 64 };
    private static readonly ArrayList MultiEventTriggeredFields = new ArrayList() { 6, 9, 23, 26, 40, 43, 57, 60 };

    public static readonly int GoalFieldIndex = 999;

    public static readonly int NofRegularFields = 68;
    public static readonly int NofStairFieldsEachPlayer = 7;
    public static readonly int NofPlayers = 4;

    public static readonly int FirstRedFieldIndex = 0;
    public static readonly int LastRedFieldIndex = 16;
    public static readonly int FirstYellowFieldIndex = 17;
    public static readonly int LastYellowFieldIndex = 33;
    public static readonly int FirstBlueFieldIndex = 34;
    public static readonly int LastBlueFieldIndex = 50;
    public static readonly int FirstGreenFieldIndex = 51;
    public static readonly int LastGreenFieldIndex = 67;

    /// <summary>
    /// 0: HomeBench / 1: StairBench / 2: FirstStairStep / 3: HomeField
    /// </summary>
    private static readonly Dictionary<string, int[]> ImportantPlayerFields = new Dictionary<string, int[]>
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

    public static bool IsSingleEventTrigger(int fieldIndex)
    {
        GameFieldBase field = fields[fieldIndex] as GameFieldBase;
        if (field == null) throw new InvalidGameStateException();
        return field.IsSingleEventTrigger;
    }

    public static bool IsMultiEventTrigger(int fieldIndex)
    {
        GameFieldBase field = fields[fieldIndex] as GameFieldBase;
        if (field == null) throw new InvalidGameStateException();
        return field.IsMultiEventTrigger;
    }

    public static bool IsBarrier(int fieldIndex)
    {
        RegularField field = fields[fieldIndex] as RegularField;
        if (field == null) throw new InvalidGameStateException();
        return field.IsBarrier;
    }

    public static bool IsHomeField(int fieldIndex, Figure figure)
    {
        Player player = FigureCtrl.GetPlayer(figure);
        int[] playerField;
        ImportantPlayerFields.TryGetValue(player.Color, out playerField);
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

    public static bool IsStairBench(int fieldIndex, Figure figure)
    {
        Player player = FigureCtrl.GetPlayer(figure);
        int[] playerField;
        ImportantPlayerFields.TryGetValue(player.Color, out playerField);
        if (playerField == null) throw new InvalidGameStateException();
        return playerField[1] == fieldIndex;
    }

    public static bool IsLastStairStep(int fieldIndex, Figure figure)
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
        return nextFieldIndex == NofRegularFields ? 0 : nextFieldIndex;
    }

    public static int GetNextStairFieldIndex(Figure figure, int actualIndex, bool hasToGoBackwards)
    {
        int nextFieldIndex;

        if (actualIndex < GetFirstStairStep(figure) || actualIndex > GetLastStairStep(figure))
            throw new InvalidGameStateException();
        if (hasToGoBackwards) nextFieldIndex = actualIndex - 1;
        else nextFieldIndex = actualIndex + 1;

        return nextFieldIndex;
    }

    private static Figure GetFigureToSendHome(RegularField field, Figure arrivingFigure)
    {
        return field.GetFigureToSendHome(arrivingFigure);
    }

    private static int GetHomeBench(Figure figure)
    {
        return GetPlayerField(figure, 0);
    }

    private static int GetFirstStairStep(Figure figure)
    {
        return GetPlayerField(figure, 2);
    }

    private static int GetLastStairStep(Figure figure)
    {
        return GetFirstStairStep(figure) + NofStairFieldsEachPlayer - 1;
    }

    private static int GetHomeField(Figure figure)
    {
        return GetPlayerField(figure, 3);
    }

    private static int GetPlayerField(Figure figure, int index)
    {
        Player player = FigureCtrl.GetPlayer(figure);
        int[] result;
        if (!ImportantPlayerFields.TryGetValue(player.Color, out result))
            throw new InvalidGameStateException();
        return result[index];
    }

    public static bool PlaceFigureOnHomeBench(Figure figure)
    {
        if (!IsHomeField(figure.Field, figure))
            throw new InvalidGameStateException();
        int newFieldIndex = GetHomeBench(figure);
        if (IsBarrier(newFieldIndex)) return false;
        MoveFigureRegular(figure, newFieldIndex);
        return true;
    }

    public static Figure PlaceFigureOnNextRegularField(Figure figure, bool isLastStep)
    {
        if (!IsRegularField(figure.Field))
            throw new InvalidGameStateException();
        int newFieldIndex = GetNextRegularFieldIndex(figure.Field);
        if (!IsRegularField(newFieldIndex)) throw new InvalidGameStateException();
        if (IsBarrier(newFieldIndex)) throw new InvalidGameStateException();
        MoveFigureRegular(figure, newFieldIndex);
        Figure figureToSendHome = GetFigureToSendHome((RegularField)fields[newFieldIndex], figure);
        return figureToSendHome;
    }

    public static void PlaceFigureOnFirstStairStep(Figure figure)
    {
        if (!IsStairBench(figure.Field, figure))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetFirstStairStep(figure));
    }

    public static void PlaceFigureOnNextRegularStairStep(Figure figure, bool hasToGoBackwards)
    {
        if (!IsStairField(figure.Field))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetNextStairFieldIndex(figure, figure.Field, hasToGoBackwards));
    }

    public static void PlaceFigureInGoal(Figure figure)
    {
        if (!IsLastStairStep(figure.Field, figure))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GoalFieldIndex);
    }

    public static void PlaceFigureOnLastStairStep(Figure figure)
    {
        if (!IsGoalField(figure.Field))
            throw new InvalidGameStateException();
        MoveFigureRegular(figure, GetLastStairStep(figure));
    }

    public static void PlaceFigureOnHomeField(Figure figure)
    {
        MoveFigureHome(figure);
    }

    private static void MoveFigureRegular(Figure figure, int fieldIndex)
    {
        fields[figure.Field].RemoveFigure(figure);
        fields[fieldIndex].PlaceFigure(figure);
    }

    public static void MoveFigureHome(Figure figure)
    {
        if (!IsRegularField(figure.Field))
            throw new InvalidGameStateException();
        fields[figure.Field].RemoveFigure(figure);
        fields[GetHomeField(figure)].PlaceFigure(figure);
    }

    public static void InitiallyPlaceFigure(Figure figure)
    {
        if (GameCtrl.GameIsRunning)
            throw new InvalidGameStateException();
        HomeField field = fields[GetHomeField(figure)] as HomeField;
        if (field == null) throw new InvalidGameStateException();
        field.InitiallyPlaceFigure(figure);
    }

    private static void SetAsSingleEventTrigger(GameFieldBase field, bool value)
    {
        field.IsSingleEventTrigger = value;
        RegularField regularField = field as RegularField;

        if (regularField != null && !regularField.IsBench)
        {
            StringBuilder materialSource = new StringBuilder(TexturesDirectory);
            if (regularField.Index >= FirstRedFieldIndex && regularField.Index <= LastRedFieldIndex)
            {
                if (regularField.IsBend)
                    materialSource.Append(RedTriggerBendMaterial);
                else
                    materialSource.Append(RedTriggerRegularMaterial);
            }
            else if (regularField.Index >= FirstYellowFieldIndex && regularField.Index <= LastYellowFieldIndex)
            {
                if (regularField.IsBend)
                    materialSource.Append(YellowTriggerBendMaterial);
                else
                    materialSource.Append(YellowTriggerRegularMaterial);
            }
            else if (regularField.Index >= FirstBlueFieldIndex && regularField.Index <= LastBlueFieldIndex)
            {
                if (regularField.IsBend)
                    materialSource.Append(BlueTriggerBendMaterial);
                else
                    materialSource.Append(BlueTriggerRegularMaterial);
            }
            else if (regularField.Index >= FirstGreenFieldIndex && regularField.Index <= LastGreenFieldIndex)
            {
                if (regularField.IsBend)
                    materialSource.Append(GreenTriggerBendMaterial);
                else
                    materialSource.Append(GreenTriggerRegularMaterial);
            }
            Material material = Resources.Load(materialSource.ToString(), typeof(Material)) as Material;
            if (material != null)
                GameObject.Find(regularField.gameObject.name + "/" + "default").GetComponent<MeshRenderer>().material = material;
        }
    }

    private static void SetAsMultiEventTrigger(GameFieldBase field, bool value)
    {
        field.IsMultiEventTrigger = value;
        RegularField regularField = field as RegularField;

        if (regularField != null && !regularField.IsBench)
        {
            StringBuilder materialSource = new StringBuilder(TexturesDirectory);
            if (regularField.IsBend)
            {
                materialSource.Append(RainbowTriggerBendMaterial);
            }
            else
            {
                materialSource.Append(RainbowTriggerRegularMaterial);
            }

            Material material = Resources.Load(materialSource.ToString(), typeof(Material)) as Material;
            if (material != null)
                GameObject.Find(regularField.gameObject.name + "/" + "default").GetComponent<MeshRenderer>().material = material;
        }
    }

    public static void SetEventTriggers()
    {
        if (ImageCtrl.IsAvailable)
        {
            foreach (GameFieldBase field in fields.Values)
                if (SingleEventTriggeredFields.Contains(field.Index))
                    SetAsSingleEventTrigger(field, true);
                else if (MultiEventTriggeredFields.Contains(field.Index)
                    && !field.IsSingleEventTrigger)
                    SetAsMultiEventTrigger(field, true);
        }
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

    public static void DestroyFields()
    {
        foreach (GameFieldBase field in fields.Values)
        {
            RegularField regularField = field as RegularField;
            HomeField homeField = field as HomeField;
            GoalField goalField = field as GoalField;
            StairField stairField = field as StairField;
            if (regularField != null)
            {
                StringBuilder material = new StringBuilder(TexturesDirectory);
                if (regularField.IsBend)
                    material.Append(BendMaterial);
                else if (regularField.IsBench)
                    material.Append(BenchMaterial);
                else material.Append(RegularMaterial);
                regularField.gameObject.GetComponentInChildren<MeshRenderer>().material = 
                    Resources.Load(material.ToString(), typeof(Material)) as Material;
                Debug.Log(material.ToString());
                GameObject.Destroy(field.gameObject.GetComponent<RegularField>());
            }   
            else if (homeField != null)
                GameObject.Destroy(field.gameObject.GetComponent<HomeField>());
            else if (goalField != null)
                GameObject.Destroy(field.gameObject.GetComponent<GoalField>());
            else if (stairField != null)
                GameObject.Destroy(field.gameObject.GetComponent<StairField>());
        }
        fields = null;
    }
}