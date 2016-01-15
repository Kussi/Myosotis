using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of the Settingsmenu, which can be activated with the settingsbutton
/// at the middle of the goal field
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    public Button playPause;

    private static readonly string MusicSettingsLayer = "MusicSettings";
    private static readonly string NoMusicSettingsLayer = "NoMusicSettings";
    private static readonly string MusicTitle = "MusicTitle";
    private static readonly string StartMenu = "StartMenu";

    private bool isVisible = false;

    void Awake()
    {
        Hide();
    }

    void Update()
    {
        if (isVisible && TouchInputCtrl.isActive()) TouchInputCtrl.Deactivate();
        else if (!isVisible && !TouchInputCtrl.isActive()) TouchInputCtrl.Activate();
    }

    /// <summary>
    /// displays the menu
    /// </summary>
    public void Show()
    {
        isVisible = true;
        GameObject[] musicSettingsObjects = FindGameObjectsOnLayer(MusicSettingsLayer);
        GameObject[] noMusicSettingsObjects = FindGameObjectsOnLayer(NoMusicSettingsLayer);

        if (MusicCtrl.IsAvailable)
        {
            SetInteractable(musicSettingsObjects, true);
            SetInteractable(noMusicSettingsObjects, false);
            SetTrackTitle(MusicCtrl.GetTrackName());
        }
        else
        {
            SetInteractable(musicSettingsObjects, false);
            SetInteractable(noMusicSettingsObjects, true);
        }
        SetInteractable(gameObject, true);
    }

    /// <summary>
    /// hides the menu
    /// </summary>
    public void Hide()
    {
        isVisible = false;
        SetInteractable(gameObject, false);
    }

    /// <summary>
    /// plays or pauses the music
    /// </summary>
    public void PlayPause()
    {
        MusicCtrl.PlayPause();
        playPause.GetComponent<UnityEngine.UI.Image>().sprite = MusicCtrl.GetPlayPauseIcon();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    /// <summary>
    /// skips the music backwards
    /// </summary>
    public void Previous()
    {
        MusicCtrl.Previous();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    /// <summary>
    /// skips the music forwards
    /// </summary>
    public void Next()
    {
        MusicCtrl.Next();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    /// <summary>
    /// changes the music volume
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolume(float volume)
    {
        MusicCtrl.ChangeVolume(volume);
    }

    /// <summary>
    /// sets and displays the trackname at the label
    /// </summary>
    /// <param name="title"></param>
    public void SetTrackTitle(string title)
    {
        GameObject.Find(MusicTitle).GetComponent<UnityEngine.UI.Text>().text = title;
    }

    /// <summary>
    /// Starts a new game
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

    /// <summary>
    /// Sets multiple gameobjects interactable (that they can fetch touches)
    /// </summary>
    /// <param name="gameObjects"></param>
    /// <param name="value"></param>
    private static void SetInteractable(GameObject[] gameObjects, bool value)
    {
        foreach (GameObject gameObject in gameObjects)
            SetInteractable(gameObject, value);
    }

    /// <summary>
    /// Sets one gameobjecte interactable (that it can fetch touches)
    /// </summary>
    /// <param name="gameObjects"></param>
    /// <param name="value"></param>
    private static void SetInteractable(GameObject gameObject, bool value)
    {
        if (value)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    /// <summary>
    /// returns all gameobjects on a specific layer
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns></returns>
    private GameObject[] FindGameObjectsOnLayer(string layerName)
    {

        int layer = LayerMask.NameToLayer(layerName);
        GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.layer == layer)
                list.Add(gameObject);
        }
        return list.ToArray();
    }
}