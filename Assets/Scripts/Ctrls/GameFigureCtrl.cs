using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameFigureCtrl
{

    private static readonly string GameFigureParentalSuffix = "Corner";
    private static readonly string GameFigurePrefab = "Prefabs/GameFigure";
    private static readonly string MaterialDirectory = "Materials/";
    private static readonly string MaterialSuffix = "Player";
    private static readonly string GameFigureName = "GameFigure";

    private static Dictionary<GameFigure, Player> figures = new Dictionary<GameFigure, Player>();

    private static ArrayList GetGameFigures(Player player)
    {
        ArrayList result = new ArrayList();
        Player element;

        foreach (GameFigure figure in figures.Keys)
        {
            element = GetPlayer(figure);
            if (element.Equals(player))
            {
                result.Add(figure);
            }
        }
        if (figures.Count % result.Count != 0) throw new InvalidGameStateException();
        return result;
    }

    public static Player GetPlayer(GameFigure figure)
    {
        Player result;

        if (!figures.TryGetValue(figure, out result))
            throw new InvalidGameStateException();
        return result;
    }

    public static void Notify(GameFigure figure)
    {
        GameCtrl.Notify(figure);
    }

    public static void ActivateReleasedFigures(Player player)
    {
        ArrayList figures = GetFiguresOnRegularOrStairField(player);
        foreach (GameFigure figure in figures)
            ActivateFigure(figure);
    }

    public static void ActivateAllFigures(Player player)
    {
        ArrayList figures = GetGameFigures(player);
        foreach (GameFigure figure in figures)
            ActivateFigure(figure);
    }

    private static void ActivateFigure(GameFigure figure)
    {
        figure.SetActive(true);
    }

    public static ArrayList GetFiguresOnGoalField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetGameFigures(player);
        foreach (GameFigure figure in figures)
            if (FieldCtrl.IsGoalField(figure.Field))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static ArrayList GetFiguresOnHomeField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetGameFigures(player);
        foreach (GameFigure figure in figures)
            if (FieldCtrl.IsHomeField(figure.Field, figure))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static ArrayList GetFiguresOnRegularOrStairField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetGameFigures(player);
        foreach (GameFigure figure in figures)
            if (FieldCtrl.IsRegularField(figure.Field)
                || FieldCtrl.IsStairField(figure.Field))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static void PlaceFiguresOnStartPosition()
    {
        if (!GameCtrl.GameIsRunning)
        {
            foreach (GameFigure figure in figures.Keys)
            {
                FieldCtrl.InitiallyPlaceFigure(figure);
            }
        }
    }

    public static void InitializeGameFigures(ArrayList players, int nofGameFigures)
    {
        foreach (Player player in players)
        {
            for (int j = 0; j < nofGameFigures; ++j)
            {
                // editing GameObject
                GameObject figureObject = (GameObject)GameObject.Instantiate(Resources.Load(GameFigurePrefab, typeof(GameObject)));
                figureObject.AddComponent<GameFigure>();
                figureObject.name = player.Color + GameFigureName + j;
                figureObject.transform.parent = GameObject.Find(player.Color + GameFigureParentalSuffix).transform;

                Material material = (Material)Resources.Load(MaterialDirectory + player.Color + MaterialSuffix, typeof(Material));
                figureObject.GetComponent<Renderer>().material = material;

                // editing (script) object
                GameFigure figure = (GameFigure)figureObject.GetComponent<GameFigure>();

                // adding figure to the list
                figures.Add(figure, player);
            }
        }
    }
}