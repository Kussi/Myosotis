using UnityEngine;

public class LogicStateChoosingFigureFive : AbstractLogicState {

    public LogicStateChoosingFigureFive()
    {
        GameFigure[] figures = GameLogic.GetFiguresOnGameOrHomeOrStairField(GameLogic.PlayerOnTurn.GameFigures);
        foreach (GameFigure figure in figures) figure.SetActive(true);
    }
}