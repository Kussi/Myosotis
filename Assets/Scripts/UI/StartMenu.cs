using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private static readonly string DropdownGameObject = "PlayerSelection";
    private static readonly string PlayerSelectionSuffix = "Players";
    private static readonly string FigureSelectionSuffix = "Figures";

    // Colors of selected button
    private static readonly Color32 SelectedNormal = new Color32(120, 120, 255, 255);
    private static readonly Color32 SelectedHighlighted = new Color32(120, 120, 255, 255);
    private static readonly Color32 SelectedPressed = new Color32(165, 165, 200, 255);

    // Colors of deselected buttons
    private static readonly Color32 DeselectedNormal = new Color32(255, 255, 255, 255);
    private static readonly Color32 DeselectedHighlighted = new Color32(255, 255, 255, 255);
    private static readonly Color32 DeselectedPressed = new Color32(200, 200, 200, 255);

    private static readonly int MinNofPlayers = 2;
    private static readonly int MaxNofPlayers = 4;
    private static readonly int MinNofFigures = 1;
    private static readonly int MaxNofFigures = 4;

    public static readonly string UnpersonalizedPlayerName = "Spieler ohne Personalisierung";

    private static int nofPlayers = MinNofPlayers;
    private static int nofFigures = MaxNofFigures;

    private static int playerIndex;
    private static string playerName;

    void Awake()
    {
        UpdateDropdown();
        ChangeNofPlayers(nofPlayers);
        ChangeNofFigures(nofFigures);
    }

    /// <summary>
    /// starts a game with the specified player and number of players.
    /// </summary>
    public void StartGame()
    {
        bool hasPersonalization = !playerName.Equals(UnpersonalizedPlayerName);
        Initializer.SetupGame(playerName, nofPlayers, nofFigures, hasPersonalization);
        Hide();
    }

    /// <summary>
    /// changes the number of players, who want to play a game.
    /// </summary>
    /// <param name="number">number of players</param>
    public void ChangeNofPlayers(int number)
    {
        if (number < MinNofPlayers || number > MaxNofPlayers) throw new InvalidGameStateException();
        nofPlayers = number;

        for (int i = MinNofPlayers; i <= MaxNofPlayers; ++i)
        {
            Button button = GameObject.Find(i + PlayerSelectionSuffix).GetComponent<Button>();
            if (i == nofPlayers) ChangeButtonColors(button, true);
            else ChangeButtonColors(button, false);
        }
    }

    public void ChangeNofFigures(int number)
    {
        if (number < MinNofFigures || number > MaxNofFigures) throw new InvalidGameStateException();
        nofFigures = number;

        for (int i = MinNofFigures; i <= MaxNofFigures; ++i)
        {
            Button button = GameObject.Find(i + FigureSelectionSuffix).GetComponent<Button>();
            if (i == nofFigures) ChangeButtonColors(button, true);
            else ChangeButtonColors(button, false);
        }
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

    private void ChangeButtonColors(Button button, bool isSelected)
    {
        ColorBlock result = button.colors;
        if (isSelected)
        {
            result.normalColor = SelectedNormal;
            result.highlightedColor = SelectedHighlighted;
            result.pressedColor = SelectedPressed;
        }
        else
        {
            result.normalColor = DeselectedNormal;
            result.highlightedColor = DeselectedHighlighted;
            result.pressedColor = DeselectedPressed;
        }
        button.colors = result;
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