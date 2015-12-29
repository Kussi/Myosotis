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

    public static List<KeyValuePair<Player, GameFigure>> figures = new List<KeyValuePair<Player, GameFigure>>();

    public static ArrayList GetGameFigures(Player player)
    {
        ArrayList result = new ArrayList();

        foreach (KeyValuePair<Player, GameFigure> figure in figures)
        {
            if (figure.Key.Equals(player))
            {
                result.Add(figure.Value);
            }
        }
        if(figures.Count % result.Count != 0) throw new InvalidGameStateException();
        return result;
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
                figures.Add(new KeyValuePair<Player, GameFigure>(player, figure));
            }
        }
    }
}