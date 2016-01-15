using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of the startmenu, which appears at the very beginning of the
/// game
/// </summary>
public class StartMenu : MonoBehaviour
{
    public PlayerButton[] playerButtons;
    public FigureButton[] figureButtons;
    public UnityEngine.UI.Text nofPlayersDisplay;
    public Color32 alertColor;
    public GameObject SplashScreen;

    private static readonly string DropdownGameObject = "PlayerSelection";
    private static readonly string PlayerSelectionSuffix = "Players";
    private static readonly string FigureSelectionSuffix = "Figures";

    public static readonly string UnpersonalizedPlayerName = "Spieler ohne persönliche Inhalte";

    private static int nofPlayers = 0;
    private static int nofFigures = 4;

    private static int playerIndex;
    private static string playerName;
    private static bool isVisible = true;

    void Awake()
    {
        UpdateDropdown();
        SelectFigures(nofFigures);
    }

    void Update()
    {
        if (isVisible && TouchInputCtrl.isActive()) TouchInputCtrl.Deactivate();
        else if (!isVisible && !TouchInputCtrl.isActive()) TouchInputCtrl.Activate();

        int number = 0;
        foreach (PlayerButton button in playerButtons)
            if (button.IsSelected) number++;
        nofPlayers = number;
        nofPlayersDisplay.text = "(" + nofPlayers + " Spieler ausgewählt)";
    }

    /// <summary>
    /// starts a game with the specified player and number of players.
    /// </summary>
    public void StartGame()
    {
        bool tooFewPlayers = nofPlayers == 0;

        if (tooFewPlayers) nofPlayersDisplay.color = alertColor;
        else
        {
            ShowSplashScreen();
            Initializer.SetupGame(playerName, GetSelectedPlayers(), nofFigures);
        }
    }

    /// <summary>
    /// returns all players that are selected and will join the game
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, int> GetSelectedPlayers()
    {
        Dictionary<string, int> players = new Dictionary<string, int>();
        foreach (PlayerButton button in playerButtons)
            if (button.IsSelected)
                players.Add(button.color, button.GetSelectedChair());
        return players;
    }

    /// <summary>
    /// hides the splashscreen
    /// </summary>
    private void HideSplashScreen()
    {
        SplashScreen.SetActive(false);
    }

    /// <summary>
    /// shows the splashscreen
    /// </summary>
    private void ShowSplashScreen()
    {
        SplashScreen.SetActive(true);
    }

    /// <summary>
    /// changes the player, whose personaldata shall be loaded
    /// </summary>
    /// <param name="value">index of dropdown list</param>
    public void ChangePlayer(int value)
    {
        playerIndex = value;
        playerName = GetSelectedPlayerName();
    }

    /// <summary>
    /// Gets the name of the selected player in the dropdown (the player whose media will be
    /// loaded)
    /// </summary>
    /// <returns></returns>
    private string GetSelectedPlayerName()
    {
        Dropdown dropdown = GameObject.Find(DropdownGameObject).GetComponent<Dropdown>();
        return dropdown.options[playerIndex].text;
    }

    /// <summary>
    /// fills the PlayerSelection dropdown with the player names. Only players will be added, whose playerdata directory is valid.
    /// </summary>
    private void UpdateDropdown()
    {
        Dropdown dropdown = GameObject.Find(DropdownGameObject).GetComponent<Dropdown>();
        Dropdown.OptionData element;

        // add unpersonalized player first
        element = new Dropdown.OptionData(UnpersonalizedPlayerName);
        dropdown.options.Add(element);

        // add personalized players after unpersonlized one
        foreach (string playerName in FileCtrl.GetPlayerList())
        {
            element = new Dropdown.OptionData(playerName);
            dropdown.options.Add(element);
        }

        dropdown.value = playerIndex;
        ChangePlayer(dropdown.value);
        GameObject.Find(DropdownGameObject + "/Label").GetComponent<UnityEngine.UI.Text>().text = GetSelectedPlayerName();
    }

    /// <summary>
    /// selects one of the figure buttons and deselects the others
    /// </summary>
    /// <param name="value"></param>
    public void SelectFigures(int value)
    {
        for (int i = 0; i < figureButtons.Length; ++i)
        {
            if (i + 1 == value) figureButtons[i].SetSelected(true);
            else figureButtons[i].SetSelected(false);
        }
        nofFigures = value;
    }

    /// <summary>
    /// displays the menu
    /// </summary>
    public void Show()
    {
        isVisible = true;
        HideSplashScreen();
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// hides the menu
    /// </summary>
    public void Hide()
    {
        isVisible = false;
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}