using UnityEngine;

/// <summary>
/// Representation of the endmenu, which is shown at the end of the game,
/// when all players finished all of their figures
/// </summary>
public class EndMenu : MonoBehaviour {

    private static readonly string StartMenu = "StartMenu";

    void Awake()
    {
        Hide();
    }

    /// <summary>
    /// displays the menu
    /// </summary>
    public void Show()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// hides the menu
    /// </summary>
    public void Hide()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// starts e new game
    /// </summary>
    public void NewGame()
    {
        GameObject.Find(StartMenu).GetComponent<StartMenu>().Show();
        Hide();
        Initializer.DestroyGame();
    }

    /// <summary>
    /// exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}