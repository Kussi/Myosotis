using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for the figure control
/// </summary>
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
    private static int nofFigures;

    private static Dictionary<Figure, Player> figures = new Dictionary<Figure, Player>();

    public static int NofFigures
    {
        get { return nofFigures; }
    }

    public static bool FiguresAreWalking
    {
        get { return figuresAreWalking; }
    }

    /// <summary>
    /// returns all figures of all players in game
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
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

    /// <summary>
    /// returns all players in game
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    public static Player GetPlayer(Figure figure)
    {
        Player result;

        if (!figures.TryGetValue(figure, out result))
            throw new InvalidGameStateException();
        return result;
    }

    /// <summary>
    /// gets the notification of the touched figure and
    /// hands it over to the gamectrl
    /// </summary>
    /// <param name="figure"></param>
    public static void Notify(Figure figure)
    {
        DeactivateAllFigures(GetPlayer(figure));
        GameCtrl.Notify(figure);
    }

    /// <summary>
    /// activates all figures on regular or stair field (not in goal or
    /// at home)
    /// </summary>
    /// <param name="player"></param>
    public static void ActivateReleasedFigures(Player player)
    {
        ArrayList figures = GetFiguresOnRegularOrStairField(player);
        foreach (Figure figure in figures)
            ActivateFigure(figure);
    }

    /// <summary>
    /// activates all figures except from them who are in goal
    /// </summary>
    /// <param name="player"></param>
    public static void ActivateAllFigures(Player player)
    {
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (!FieldCtrl.IsGoalField(figure.Field))
                ActivateFigure(figure);
    }

    /// <summary>
    /// deactivates all figures after one has been touched
    /// </summary>
    /// <param name="player"></param>
    public static void DeactivateAllFigures(Player player)
    {
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            DeactivateFigure(figure);
    }

    /// <summary>
    /// activates a figure
    /// </summary>
    /// <param name="figure"></param>
    private static void ActivateFigure(Figure figure)
    {
        figure.SetActive(true);
        Debug.Log(figure.name + " activated");
    }

    /// <summary>
    /// deactivates a figure
    /// </summary>
    /// <param name="figure"></param>
    private static void DeactivateFigure(Figure figure)
    {
        figure.SetActive(false);
        Debug.Log(figure.name + " deactivated");
    }

    /// <summary>
    /// returns all figures that are in goal
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static ArrayList GetFiguresOnGoalField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (FieldCtrl.IsGoalField(figure.Field))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    /// <summary>
    /// returns all figures that are at home
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static ArrayList GetFiguresOnHomeField(Player player)
    {
        ArrayList figuresOnGoalField = new ArrayList();
        ArrayList figures = GetFigures(player);
        foreach (Figure figure in figures)
            if (FieldCtrl.IsHomeField(figure.Field, figure))
                figuresOnGoalField.Add(figure);
        return figuresOnGoalField;
    }

    /// <summary>
    /// returns all figures that are on a regular or stair field
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Places all figures at the very beginning of the game at their home fields
    /// </summary>
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

    /// <summary>
    /// Starts a figure to walk
    /// </summary>
    /// <param name="figure"></param>
    public static void FigureStartsWalking(Figure figure)
    {
        if (walkingFigures.Count == 0)
            figuresAreWalking = true;
        walkingFigures.Add(figure);
    }

    /// <summary>
    /// stops a figure to walk
    /// </summary>
    /// <param name="figure"></param>
    public static void FigureStopsWalking(Figure figure)
    {
        if (!walkingFigures.Contains(figure))
            throw new InvalidGameStateException();
        walkingFigures.Remove(figure);
        if (walkingFigures.Count == 0)
        {
            figuresAreWalking = false;
            TurnCtrl.MakeNextStep();
        }
    }

    /// <summary>
    /// Initializes the number of figures that are selected in the startmenu
    /// </summary>
    /// <param name="players"></param>
    /// <param name="nofFigures"></param>
    public static void InitializeFigures(ArrayList players, int nofFigures)
    {
        FigureCtrl.nofFigures = nofFigures;
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

    /// <summary>
    /// Destroys figure objects
    /// </summary>
    public static void DestroyFigures()
    {
        foreach (Figure figure in figures.Keys)
        {
            GameObject.Destroy(figure.gameObject);
        }
        figures = new Dictionary<Figure, Player>();
    }
}