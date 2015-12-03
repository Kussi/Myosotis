using UnityEngine;

public class LogicStateChoosingFigureFive : ILogicState {

    public LogicStateChoosingFigureFive()
    {
        GameLogic.ActivateFiguresOnRegularOrHomeOrStairField();
    }
}