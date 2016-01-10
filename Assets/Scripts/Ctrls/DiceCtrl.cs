using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DiceCtrl
{

    private static readonly string DicePositionSuffix = "DicePosition";
    private static readonly string DiceParentObject = "GameTable";
    private static readonly string DicePrefab = "Prefabs/_Dice";
    private static readonly string DiceName = "Dice";

    private static Dice dice;

    public static void StartDiceThrowingProcess(string playerColor)
    {
        Debug.Log("dice starts moving");
        dice.StartPassingOn(GetDicePosition(playerColor));
        dice.ActivateDice();
    }

    public static void Notify()
    {
        GameCtrl.Notify(dice.Value);
    }

    public static void InitializeDices(string firstPlayerColor)
    {
        GameObject diceObject = (GameObject)GameObject.Instantiate(Resources.Load(DicePrefab, typeof(GameObject)));
        diceObject.name = (DiceName);
        diceObject.transform.parent = GameObject.Find(DiceParentObject).transform;
        diceObject.transform.position = GetDicePosition(firstPlayerColor).position;

        diceObject.AddComponent<Dice>();
        dice = (Dice)diceObject.GetComponent<Dice>();
    }

    private static Transform GetDicePosition(string playerColor)
    {
        return GameObject.Find(playerColor + DicePositionSuffix).transform;
    }
}