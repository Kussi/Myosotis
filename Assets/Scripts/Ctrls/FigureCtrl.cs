using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FigureCtrl
{
    private static readonly string FigureParentalSuffix = "Corner";
    private static readonly string FigurePrefab = "Prefabs/GameFigure";
    private static readonly string MaterialDirectory = "Materials/";
    private static readonly string MaterialSuffix = "Player";
    private static readonly string FigureName = "Figure";

    public static readonly int EmptyFieldEntry = -1;

    private static bool figuresAreWalking = false;
    private static ArrayList walkingFigures = new ArrayList();

    private static Dictionary<Figure, Player> figures = new Dictionary<Figure, Player>();

    public static bool FiguresAreWalking
    {
        get { return figuresAreWalking; }
    }

    private static ArrayList GetFigures(Player player)
    {
        ArrayList result = new ArrayList();
        Player element;

        foreach (Figure figure in figures.Keys)
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

    public static Player GetPlayer(Figure figure)
    {
        Player result;

        if (!figures.TryGetValue(figure, out result))
            throw new InvalidGameStateException();
        return result;
    }

    public static void Notify(Figure figure)
    {
        DeactivateAllFigures(GetPlayer(figure));
        GameCtrl.Notify(figure);
    }

    public static void ActivateReleasedFigures(Player player)
    {
        ArrayList figures = GetFiguresOnRegularOrStairField(player);
        foreach (Figure figure in figures)
            ActivateFigure(figure);
    }

    public static void ActivateAllFigures(Player player)
    {
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if(!FieldCtrl.IsGoalField(figure.Field))
                ActivateFigure(figure);
    }

    public static void DeactivateAllFigures(Player player)
    {
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            DeactivateFigure(figure);
    }

    private static void ActivateFigure(Figure figure)
    {
        figure.SetActive(true);
        Debug.Log(figure.name + " activated");
    }

    private static void DeactivateFigure(Figure figure)
    {
        figure.SetActive(false);
        Debug.Log(figure.name + " deactivated");
    }

    public static ArrayList GetFiguresOnGoalField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (FieldCtrl.IsGoalField(figure.Field))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static ArrayList GetFiguresOnHomeField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (FieldCtrl.IsHomeField(figure.Field, figure))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static ArrayList GetFiguresOnRegularOrStairField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (FieldCtrl.IsRegularField(figure.Field)
                || FieldCtrl.IsStairField(figure.Field))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    public static void PlaceFiguresOnStartPosition()
    {
        if (!GameCtrl.GameIsRunning)
        {
            foreach (Figure figure in figures.Keys)
            {
                FieldCtrl.InitiallyPlaceFigure(figure);
            }
        }
    }

    public static void FigureStartsWalking(Figure figure)
    {
        Debug.Log("figuren am laufen: " + walkingFigures.Count);
        if (walkingFigures.Count == 0)
            figuresAreWalking = true;
        walkingFigures.Add(figure);
    }

    public static void FigureStopsWalking(Figure figure)
    {
        if (!walkingFigures.Contains(figure))
            throw new InvalidGameStateException();
        walkingFigures.Remove(figure);
        if (walkingFigures.Count == 0)
            figuresAreWalking = false;
    }

    public static void InitializeFigures(ArrayList players, int nofFigures)
    {
        foreach (Player player in players)
        {
            for (int j = 0; j < nofFigures; ++j)
            {
                // editing GameObject
                GameObject figureObject = (GameObject)GameObject.Instantiate(Resources.Load(FigurePrefab, typeof(GameObject)));
                figureObject.AddComponent<Figure>();
                figureObject.name = player.Color + FigureName + j;
                figureObject.transform.parent = GameObject.Find(player.Color + FigureParentalSuffix).transform;

                Material material = (Material)Resources.Load(MaterialDirectory + player.Color + MaterialSuffix, typeof(Material));
                figureObject.GetComponent<Renderer>().material = material;

                // editing (script) object
                Figure figure = (Figure)figureObject.GetComponent<Figure>();
                figure.Field = EmptyFieldEntry;

                // adding figure to the list
                figures.Add(figure, player);
            }
        }
    }
}