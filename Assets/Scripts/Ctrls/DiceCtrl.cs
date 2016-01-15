using UnityEngine;

/// <summary>
/// Responsible for the control of the dice
/// </summary>
public static class DiceCtrl
{
    private static readonly string DicePositionSuffix = "DicePosition";
    private static readonly string DiceParentObject = "GameTable";
    private static readonly string DicePrefab = "Prefabs/_Dice";
    private static readonly string DiceName = "Dice";

    private static Dice dice;

    /// <summary>
    /// launches the dice throw
    /// </summary>
    /// <param name="playerColor"></param>
    public static void StartDiceThrowingProcess(string playerColor)
    {
        Debug.Log("dice starts moving");
        dice.StartPassingOn(GetDicePosition(playerColor));
        dice.ActivateDice();
    }

    /// <summary>
    /// Gets the notification that the dice has been thrown
    /// </summary>
    public static void Notify()
    {
        GameCtrl.Notify(dice.Value);
    }

    /// <summary>
    /// Initializes the dice object
    /// </summary>
    /// <param name="firstPlayerColor"></param>
    public static void InitializeDice(string firstPlayerColor)
    {
        GameObject diceObject = (GameObject)GameObject.Instantiate(Resources.Load(DicePrefab, typeof(GameObject)));
        diceObject.name = (DiceName);
        diceObject.transform.parent = GameObject.Find(DiceParentObject).transform;
        diceObject.transform.position = GetDicePosition(firstPlayerColor).position;

        diceObject.AddComponent<Dice>();
        dice = diceObject.GetComponent<Dice>();
    }

    /// <summary>
    /// gets the diceposition of a player (this are the positions at the players home, where
    /// the dice is placed)
    /// </summary>
    /// <param name="playerColor"></param>
    /// <returns></returns>
    private static Transform GetDicePosition(string playerColor)
    {
        return GameObject.Find(playerColor + DicePositionSuffix).transform;
    }

    /// <summary>
    /// destroy the dice object
    /// </summary>
    public static void DestroyDice()
    {
        GameObject.Destroy(dice.gameObject);
    }
}