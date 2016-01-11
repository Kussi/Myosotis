using UnityEngine;
using System.Collections;
using System.IO;

public class MusicCtrl
{
    private static readonly string MusicObject = "Scene";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".ogg", ".wav" };

    private static bool isAvailable = false;

    private static Music music;

    public static bool IsAvailable
    {
        get { return isAvailable; }
    }

    public static void Reset()
    {
        isAvailable = false;
        if(music != null)
            music.Reset();
    }

    public static void InitializeMusic(string playerName)
    {
        FileInfo[] musicFiles = FileCtrl.GetCheckedMusicFileInfos(playerName, ValidExtensions);

        if (musicFiles != null)
        {
            GameObject musicObject = GameObject.Find(MusicObject);
            music = musicObject.GetComponent<Music>();
            music.SetupMusic(musicFiles);
            isAvailable = true;
        }
        if (!isAvailable) PersonalizationCtrl.Notify(typeof(MusicCtrl));
    }

    public static Sprite GetPlayPauseIcon()
    {
        return music.GetPlayPauseIcon();
    }

    public static void PlayPause()
    {
        music.PlayPause();
    }

    public static void Next()
    {
        music.Next();
    }

    public static void Previous()
    {
        music.Previous();
    }

    public static void ChangeVolume(float volume)
    {
        music.ChangeVolume(volume);
    }

    public static string GetTrackName()
    {
        return music.GetTrackName();
    }
}