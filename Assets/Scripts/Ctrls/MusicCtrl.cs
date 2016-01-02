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

    public static void InitializeMusic(string player)
    {
        FileInfo[] musicFiles = FileCtrl.GetCheckedMusicFileInfos(player, ValidExtensions);
        if (musicFiles != null)
        {
            GameObject musicObject = GameObject.Find(MusicObject);
            musicObject.AddComponent<Music>();
            music = musicObject.GetComponent<Music>();
            music.SetupMusic(musicFiles);
            isAvailable = true;
        }
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