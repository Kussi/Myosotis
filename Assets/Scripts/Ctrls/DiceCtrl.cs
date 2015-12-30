using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DiceCtrl
{

    private static readonly string GameFigureParentalSuffix = "DicePosition";
    private static readonly string DicePrefab = "Prefabs/_Dice";
    private static readonly string DiceName = "Dice";

    private static Dictionary<Player, Dice> dices = new Dictionary<Player, Dice>();

    public static void ActivateDice(Player player)
    {
        Dice dice;
        if (!dices.TryGetValue(player, out dice))
            throw new InvalidGameStateException();
        ActivateDice(dice);
    }

    private static void ActivateDice(Dice dice)
    {
        dice.SetActive(true);
    }

    public static void Notify(Dice dice)
    {
        GameCtrl.Notify(dice.Value);
    }

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
            dices.Add(player, dice);
        }
    }

    public static void PlaceDices()
    {
        foreach (Player player in dices.Keys)
        {
            Dice dice;
            if (!dices.TryGetValue(player, out dice))
                throw new InvalidGameStateException();
            dice.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            Vector3 diceRotation = dice.transform.eulerAngles;
            diceRotation = new Vector3(diceRotation.x, player.PlayerAngle, diceRotation.z);
            dice.transform.eulerAngles = diceRotation;
        }
    }
}