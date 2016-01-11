using UnityEngine;

public class EndMenu : MonoBehaviour {

    private static readonly string StartMenu = "StartMenu";

    void Awake()
    {
        Hide();
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

    public void NewGame()
    {
        GameObject.Find(StartMenu).GetComponent<StartMenu>().Show();
        Hide();
        Initializer.DestroyGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}