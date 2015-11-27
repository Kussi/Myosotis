using UnityEngine;

public class LogicStateChoosingFigureSix : AbstractLogicState {

    public LogicStateChoosingFigureSix()
    {
        GameFigure[] figures = GameLogic.GetFiguresOnGameOrStairField(GameLogic.PlayerOnTurn.GameFigures);
        foreach (GameFigure figure in figures) figure.SetActive(true);
    }
}