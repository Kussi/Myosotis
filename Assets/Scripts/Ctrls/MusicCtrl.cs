using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Responsible for the personalized background music
/// </summary>
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

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        isAvailable = false;
        if (music != null)
            music.Reset();
    }

    /// <summary>
    /// initializes Music
    /// </summary>
    /// <param name="playerName"></param>
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

    /// <summary>
    /// returns play or pause icon according to the actual state
    /// </summary>
    /// <returns></returns>
    public static Sprite GetPlayPauseIcon()
    {
        return music.GetPlayPauseIcon();
    }

    /// <summary>
    /// plays or pauses music
    /// </summary>
    public static void PlayPause()
    {
        music.PlayPause();
    }

    /// <summary>
    /// skips music forwards
    /// </summary>
    public static void Next()
    {
        music.Next();
    }

    /// <summary>
    /// skips music backwards
    /// </summary>
    public static void Previous()
    {
        music.Previous();
    }

    /// <summary>
    /// changes the musics volume
    /// </summary>
    /// <param name="volume"></param>
    public static void ChangeVolume(float volume)
    {
        music.ChangeVolume(volume);
    }

    /// <summary>
    /// returns the track name
    /// </summary>
    /// <returns></returns>
    public static string GetTrackName()
    {
        return music.GetTrackName();
    }
}