using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DiceCtrl
{

    private static readonly string GameFigureParentalSuffix = "DicePosition";
    private static readonly string DicePrefab = "Prefabs/_Dice";
    private static readonly string DiceName = "Dice";

    private static List<KeyValuePair<Player, Dice>> dices = new List<KeyValuePair<Player, Dice>>();

    public static void InitializeDices(ArrayList players)
    {
        foreach (Player player in players)
        {
            // editing GameObject
            GameObject diceObject = (GameObject)GameObject.Instantiate(Resources.Load(DicePrefab, typeof(GameObject)));
            diceObject.AddComponent<Dice>();
            diceObject.name = (player.Color + DiceName);
            diceObject.transform.parent = GameObject.Find(player.Color + GameFigureParentalSuffix).transform;

            // editing (script) object
            Dice dice = (Dice)diceObject.GetComponent<Dice>();

            // adding figure to the list
            dices.Add(new KeyValuePair<Player, Dice>(player, dice));
        }
    }
}