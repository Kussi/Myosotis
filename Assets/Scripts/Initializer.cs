using System.Collections;

public static class Initializer
{
    public static void SetupGame(string playerName, ArrayList playerColors, int nofFigures, bool hasPersonalization)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(playerColors);
        DiceCtrl.InitializeDices(PlayerCtrl.players);
        FigureCtrl.InitializeFigures(PlayerCtrl.players, nofFigures);
        SunCtrl.InitializeSun();
        BulpCtrl.InitializeBulps(PlayerCtrl.players);

        if(hasPersonalization)
            PersonalizationCtrl.InitializePersonalization(playerName);

        FigureCtrl.PlaceFiguresOnStartPosition();
        DiceCtrl.PlaceDices();

        GameCtrl.StartGame();
    }

    public static void ExitGame()
    {

    } 
}