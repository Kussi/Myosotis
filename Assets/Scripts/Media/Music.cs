using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Representation of the music element
/// </summary>
public class Music : MonoBehaviour
{
    public Sprite pauseIcon;
    public Sprite playIcon;

    private static readonly string FileIndicator = "file://";

    public enum ChangeDirection { Next, Previous }
    private AudioSource source;
    private List<AudioClip> clips = new List<AudioClip>();
    private bool isPlaying = false;
    private bool isPaused = false;

    private int index = 0;

    /// <summary>
    /// resets the original setup
    /// </summary>
    public void Reset()
    {
        GameObject.Destroy(source.gameObject.GetComponent<AudioSource>());
        source = gameObject.AddComponent<AudioSource>();
        clips = new List<AudioClip>();
        isPlaying = false;
        isPaused = false;
    }

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlaying)
        {
            if (!source.isPlaying)
            {
                Next();
            }
        }
    }

    /// <summary>
    /// starts loading the tracks
    /// </summary>
    /// <param name="musicFiles"></param>
    public void SetupMusic(FileInfo[] musicFiles)
    {
        LoadMusic(musicFiles);
    }

    /// <summary>
    /// plays or pauses the music
    /// </summary>
    public void PlayPause()
    {
        if (!isPlaying) Play();
        else Pause();
    }

    /// <summary>
    /// pauses the sound
    /// </summary>
    private void Pause()
    {
        AudioListener.pause = true;
        isPlaying = false;
        isPaused = true;
    }

    /// <summary>
    /// plays the sound
    /// </summary>
    private void Play()
    {
        if (source.clip == null)
            source.clip = clips[index];
        AudioListener.pause = false;
        if (isPaused) isPaused = false;
        else source.Play();
        isPlaying = true;
    }

    /// <summary>
    /// returns the suitable play or pause icon (when it's playing, tha pause
    /// button has to be shown and the other way round)
    /// </summary>
    /// <returns></returns>
    public Sprite GetPlayPauseIcon()
    {
        if (isPaused) return playIcon;
        return pauseIcon;
    }

    /// <summary>
    /// skips the music backwards
    /// </summary>
    public void Previous()
    {
        ChangeClip(ChangeDirection.Previous);
        Play();
    }

    /// <summary>
    /// skips the music forwards
    /// </summary>
    public void Next()
    {
        ChangeClip(ChangeDirection.Next);
        Play();
    }

    /// <summary>
    /// changes the track
    /// </summary>
    /// <param name="direction"></param>
    private void ChangeClip(ChangeDirection direction)
    {
        if (direction == ChangeDirection.Next)
        {
            index = (index + 1) % clips.Count;
        }
        else if (--index < 0)
            index = clips.Count - 1;

        if (source.isPlaying)
        {
            source.clip = clips[index];
            source.Play();
        }
        else source.clip = clips[index];
    }

    /// <summary>
    /// changes the music volume
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    /// <summary>
    /// loads all musicfiles
    /// </summary>
    /// <param name="musicFiles"></param>
    private void LoadMusic(FileInfo[] musicFiles)
    {
        clips.Clear();
        if (musicFiles != null)
        {
            foreach (FileInfo musicFile in musicFiles)
                StartCoroutine(LoadFile(musicFile.FullName));
        }
    }

    /// <summary>
    /// loads one musicfile from the directory
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadFile(string path)
    {
        WWW www = new WWW(FileIndicator + path);
        // loading

        AudioClip clip = www.GetAudioClip(false);
        while (clip.loadState == AudioDataLoadState.Unloaded)
            yield return www;

        // loading done
        clip.name = Path.GetFileName(path);
        clips.Add(clip);

        if (!isPlaying)
            Play();
        PersonalizationCtrl.Notify(typeof(MusicCtrl));
    }

    /// <summary>
    /// returns the trackname
    /// </summary>
    /// <returns></returns>
    public string GetTrackName()
    {
        return clips[index].name;
    }
}