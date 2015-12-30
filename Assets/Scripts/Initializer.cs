public static class Initializer
{
    public static void SetupGame(string playerName, int nofPlayers, int nofGameFigures)
    {
        FieldCtrl.InitializeFields();
        PlayerCtrl.InitializePlayers(nofPlayers);
        DiceCtrl.InitializeDices(PlayerCtrl.players);
        GameFigureCtrl.InitializeGameFigures(PlayerCtrl.players, nofGameFigures);
        SunCtrl.InitializeSun();
        //InitializePersonalisation(playerName);

        GameFigureCtrl.PlaceGameFiguresAtHome();
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