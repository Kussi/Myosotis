using UnityEngine;

public class LogicStateChoosingFigureRegular : AbstractLogicState {

    public LogicStateChoosingFigureRegular()
    {
        GameFigure[] figures = GameLogic.GetFiguresOnGameOrStairField(GameLogic.PlayerOnTurn.GameFigures);
        foreach (GameFigure figure in figures) figure.SetActive(true);
    }
}