using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

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

    private static int NofPlayers = MinNofPlayers;


    void Awake()
    {
        SetNofPlayers(NofPlayers);
    }

    /// <summary>
    /// starts a game with the specified player and number of players.
    /// </summary>
    public void StartGame()
    {


        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        //initialisieren

    }

    /// <summary>
    /// Sets the number of Players, who want to play a game.
    /// </summary>
    /// <param name="number"></param>
    public void SetNofPlayers(int number)
    {
        if (number < MinNofPlayers || number > MaxNofPlayers) throw new InvalidGameStateException();
        NofPlayers = number;

        for(int i = MinNofPlayers; i <= MaxNofPlayers; ++i)
        {
            Button button = GameObject.Find(i + "Players").GetComponent<Button>();
            ColorBlock colorBlock = button.colors;
            if (i == NofPlayers)
            {
                colorBlock.normalColor = SelectedNormal;
                colorBlock.highlightedColor = SelectedHighlighted;
                colorBlock.pressedColor = SelectedPressed;
            }
            else
            {
                colorBlock.normalColor = DeselectedNormal;
                colorBlock.highlightedColor = DeselectedHighlighted;
                colorBlock.pressedColor = DeselectedPressed;
            }
            button.colors = colorBlock;
        }

    }

    /// <summary>
    /// Makes a color fully transparent. If it is transparent already, it returns the same color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns>return the given color fully transparent</returns>
    private static Color SetTransparent(Color color)
    {
        return new Color(color.r, color.g, color.b, 0);
    } 

    /// <summary>
    /// Makes a color fully opaque. If it is opaque already, it returns the same color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns>return the given color fully opaque</returns>
    public static Color SetOpaque(Color color)
    {
        return new Color(color.r, color.g, color.b, 255);
    }
}
