using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public PlayerButton[] playerButtons;
    public FigureButton[] figureButtons;
    public Text nofPlayersDisplay;
    public Color32 alertColor;

    private static readonly string DropdownGameObject = "PlayerSelection";
    private static readonly string PlayerSelectionSuffix = "Players";
    private static readonly string FigureSelectionSuffix = "Figures";

    public static readonly string UnpersonalizedPlayerName = "Spieler ohne persönliche Inhalte";

    private static int nofPlayers = 0;
    private static int nofFigures = 4;

    private static int playerIndex;
    private static string playerName;

    void Awake()
    {
        UpdateDropdown();
        SelectFigures(nofFigures);
    }

    void Update()
    {
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
        bool hasPersonalization = !playerName.Equals(UnpersonalizedPlayerName);

        if (tooFewPlayers) nofPlayersDisplay.color = alertColor;
        else
        {

            Initializer.SetupGame(playerName, GetSelectedPlayers(), nofFigures, hasPersonalization);
            Hide();
        }
    }

    private ArrayList GetSelectedPlayers()
    {
        ArrayList players = new ArrayList();
        foreach (PlayerButton button in playerButtons)
            if (button.IsSelected)
                players.Add(button.color);
        return players;
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
        GameObject.Find(DropdownGameObject + "/Label").GetComponent<Text>().text = GetSelectedPlayerName();
    }

    public void SelectFigures(int value)
    {
        for (int i = 0; i < figureButtons.Length; ++i)
        {
            if (i + 1 == value) figureButtons[i].SetSelected(true);
            else figureButtons[i].SetSelected(false); 
        }
        nofFigures = value;
    }

    public void Show()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Hide()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}