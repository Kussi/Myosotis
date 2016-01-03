public static class Initializer
{
    public static void SetupGame(string playerName, int nofPlayers, int nofFigures, bool hasPersonalization)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(nofPlayers);
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