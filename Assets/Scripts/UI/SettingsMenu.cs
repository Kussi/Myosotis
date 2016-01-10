﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button playPause;

    private static readonly string MusicSettingsLayer = "MusicSettings";
    private static readonly string NoMusicSettingsLayer = "NoMusicSettings";
    private static readonly string MusicTitle = "MusicTitle";
    private static readonly string StartMenu = "StartMenu";

    void Awake()
    {
        Hide();
    }

    public void Show()
    {

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

    public void Hide()
    {
        SetInteractable(gameObject, false);
    }

    public void PlayPause()
    {
        MusicCtrl.PlayPause();
        playPause.GetComponent<UnityEngine.UI.Image>().sprite = MusicCtrl.GetPlayPauseIcon();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    public void Previous()
    {
        MusicCtrl.Previous();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    public void Next()
    {
        MusicCtrl.Next();
        SetTrackTitle(MusicCtrl.GetTrackName());
    }

    public void ChangeVolume(float volume)
    {
        MusicCtrl.ChangeVolume(volume);
    }

    public void SetTrackTitle(string title)
    {
        GameObject.Find(MusicTitle).GetComponent<UnityEngine.UI.Text>().text = title;
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

    private static void SetInteractable(GameObject[] gameObjects, bool value)
    {
        foreach (GameObject gameObject in gameObjects)
            SetInteractable(gameObject, value);
    }

    private static void SetInteractable(GameObject gameObject, bool value)
    {
        if(value)
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

    private GameObject[] FindGameObjectsOnLayer(string layerName)
    {

        int layer = LayerMask.NameToLayer(layerName);
        GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
        List<GameObject> list = new List<GameObject>();
        foreach(GameObject gameObject in gameObjects)
        {
            if (gameObject.layer == layer)
                list.Add(gameObject);
        }
        return list.ToArray();
    }
}