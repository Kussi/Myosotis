public static class Initializer
{
    public static void SetupGame(string playerName, int nofPlayers, int nofFigures)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(nofPlayers);
        DiceCtrl.InitializeDices(PlayerCtrl.players);
        FigureCtrl.InitializeFigures(PlayerCtrl.players, nofFigures);
        SunCtrl.InitializeSun();
        //InitializePersonalisation(playerName);

        FigureCtrl.PlaceFiguresOnStartPosition();
        DiceCtrl.PlaceDices();

        GameCtrl.StartGame();
    }

    private static void InitializePersonalisation(string playerName)
    {
        ImageCtrl.InitializeImages(playerName);
        MusicCtrl.InitializeMusic(playerName);
        TextCtrl.InitializeTexts(playerName);
    }   
}